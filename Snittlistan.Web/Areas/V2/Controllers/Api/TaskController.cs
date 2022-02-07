#nullable enable

using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
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
            cacheItem = new(cacheKey, 1, 1, 30);
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

        cacheItem.UpdateAllowance(DateTime.Now);
        if (cacheItem.Allowance < 1)
        {
            Logger.InfoFormat(
                "throttling {cacheKey} due to allowance = {allowance}",
                cacheKey,
                cacheItem.Allowance.ToString("N2"));
            return Ok(
                $"throttled due to unhandled exception (allowance = {cacheItem.Allowance:N2})");
        }

        // check for published task
        PublishedTask? publishedTask =
        await CompositionRoot.Databases.Snittlistan.PublishedTasks.SingleOrDefaultAsync(
            x => x.MessageId == request.MessageId);
        if (publishedTask is null)
        {
            return BadRequest($"No published task found with message id {request.MessageId}");
        }

        if (publishedTask.HandledDate.HasValue)
        {
            return Ok($"task with message id {publishedTask.MessageId} already handled");
        }

        try
        {
            using IDisposable scope = NLog.NestedDiagnosticsLogicalContext.Push(taskObject.BusinessKey);
            Logger.Info("Begin");
            await HandleMessage(
                taskObject,
                request.CorrelationId ?? default,
                request.MessageId ?? default);
            publishedTask.MarkHandled(DateTime.Now);
        }
        catch (Exception ex)
        {
            Logger.WarnFormat(
                ex,
                "decreasing allowance for {cacheKey}",
                cacheKey);
            cacheItem.DecreaseAllowance();
            throw;
        }
        finally
        {
            Logger.Info("End");
        }

        return Ok();
    }

    private async Task HandleMessage(
        TaskBase taskObject,
        Guid correlationId,
        Guid causationId)
    {
        Type handlerType = typeof(ITaskHandler<>).MakeGenericType(taskObject.GetType());

        MethodInfo handleMethod = handlerType.GetMethod(nameof(ITaskHandler<TaskBase>.Handle));
        Tenant tenant = await CompositionRoot.GetCurrentTenant();
        TaskPublisher taskPublisher = new(
            tenant,
            CompositionRoot.Databases,
            correlationId,
            causationId);
        IHandlerContext handlerContext = (IHandlerContext)Activator.CreateInstance(
            typeof(HandlerContext<>).MakeGenericType(taskObject.GetType()),
            CompositionRoot,
            taskObject,
            tenant,
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
        }
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
