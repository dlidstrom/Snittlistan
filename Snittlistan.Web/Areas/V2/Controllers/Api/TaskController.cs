﻿#nullable enable

using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Web.Caching;
using System.Web.Http;
using Newtonsoft.Json;
using Snittlistan.Queue;
using Snittlistan.Queue.Messages;
using Snittlistan.Web.Controllers;
using Snittlistan.Web.Infrastructure;
using Snittlistan.Web.Infrastructure.Attributes;
using Snittlistan.Web.Infrastructure.Database;
using Snittlistan.Web.Models;
using Snittlistan.Web.TaskHandlers;

namespace Snittlistan.Web.Areas.V2.Controllers.Api;

[OnlyLocalAllowed]
public class TaskController : AbstractApiController
{
    public async Task<IHttpActionResult> Post(TaskRequest request)
    {
        Logger.InfoFormat("Received task {taskJson}", request.TaskJson);

        TaskBase taskObject = request.TaskJson.FromJson<TaskBase>();

        // check error rate
        string cacheKey = $"error-rate:{taskObject.GetType().Name}";
        if (CurrentHttpContext.Instance().Cache.Get(cacheKey) is not RateLimit cacheItem)
        {
            // allow 1 error per hour
            cacheItem = RateLimit.Create(cacheKey, 1, 1, 3600);
            CurrentHttpContext.Instance().Cache.Insert(
                cacheKey,
                cacheItem,
                null,
                Cache.NoAbsoluteExpiration,
                Cache.NoSlidingExpiration,
                CacheItemPriority.Default,
                (key, item, reason) =>
                    Logger.InfoFormat(
                        "cache item '{key}' removed due to '{reason}'",
                        key,
                        reason));
        }

        _ = cacheItem.UpdateAllowance(DateTime.Now);
        if (cacheItem.Allowance < 1)
        {
            Logger.InfoFormat(
                "throttling {cacheKey} due to allowance = {allowance}",
                cacheKey,
                cacheItem.Allowance.ToString("N2"));
            return Ok(
                $"throttled due to unhandled exception (allowance = {cacheItem.Allowance:N2})");
        }

        IDisposable scope = NLog.NestedDiagnosticsLogicalContext.Push(taskObject.BusinessKey);
        try
        {
            Logger.Info("begin");
            IHttpActionResult result = await Transact<IHttpActionResult>(async databases =>
            {
                // check for published task
                PublishedTask? publishedTask =
                await databases.Snittlistan.PublishedTasks.SingleOrDefaultAsync(
                    x => x.MessageId == request.MessageId);
                if (publishedTask is null)
                {
                    return BadRequest($"no published task found with message id {request.MessageId}");
                }

                if (publishedTask.HandledDate.HasValue)
                {
                    return Ok($"task with message id {publishedTask.MessageId} already handled");
                }

                bool handled = await HandleMessage(
                    taskObject,
                    request.CorrelationId ?? default,
                    request.MessageId ?? default,
                    databases);
                if (handled)
                {
                    publishedTask.MarkHandled(DateTime.Now);
                }

                return Ok();
            });

            return result;
        }
        catch (Exception ex)
        {
            Logger.WarnFormat(
                ex,
                "decreasing allowance for {cacheKey}",
                cacheKey);
            _ = cacheItem.DecreaseAllowance();
            throw;
        }
        finally
        {
            Logger.Info("end");
            scope.Dispose();
        }
    }

    private async Task<bool> HandleMessage(
        TaskBase taskObject,
        Guid correlationId,
        Guid causationId,
        Databases databases)
    {
        Type handlerType = typeof(ITaskHandler<>).MakeGenericType(taskObject.GetType());

        MethodInfo handleMethod = handlerType.GetMethod(nameof(ITaskHandler<TaskBase>.Handle));
        TaskPublisher taskPublisher = new(
            CompositionRoot.CurrentTenant,
            databases,
            CompositionRoot.MsmqFactory,
            correlationId,
            causationId);
        IHandlerContext handlerContext = (IHandlerContext)Activator.CreateInstance(
            typeof(HandlerContext<>).MakeGenericType(taskObject.GetType()),
            CompositionRoot,
            databases,
            taskObject,
            CompositionRoot.CurrentTenant,
            correlationId,
            causationId);
        handlerContext.PublishMessage = (task, publishDate) =>
        {
            if (publishDate != null)
            {
                taskPublisher.PublishDelayedTask(task, publishDate.Value, "system");
            }
            else
            {
                taskPublisher.PublishTask(task, "system");
            }
        };

        object handler = CompositionRoot.Kernel.Resolve(handlerType);
        try
        {
            Task task = (Task)handleMethod.Invoke(handler, new[] { handlerContext });
            await task;
        }
        catch (HandledException ex)
        {
            Logger.InfoFormat("task cannot be handled: {reason}", ex.Message);
            return false;
        }

        return true;
    }

    private async Task<TResult> Transact<TResult>(Func<Databases, Task<TResult>> func)
    {
        using Databases databases = CompositionRoot.DatabasesFactory.Create();
        TResult result = await func.Invoke(databases);
        int changesSaved = databases.Snittlistan.SaveChanges();
        if (changesSaved > 0)
        {
            Logger.InfoFormat(
                "saved {changesSaved} to database",
                changesSaved);
        }

        return result;
    }
}

public class TaskRequest
{
    public TaskRequest(string taskJson, Guid? correlationId, Guid? messageId)
    {
        TaskJson = taskJson;
        CorrelationId = correlationId;
        MessageId = messageId;
    }

    [Required]
    public string TaskJson { get; }

    [Required]
    public Guid? CorrelationId { get; }

    [Required]
    public Guid? MessageId { get; }
}
