#nullable enable

using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Reflection;
using System.Web.Http;
using Newtonsoft.Json;
using Snittlistan.Queue;
using Snittlistan.Queue.Messages;
using Snittlistan.Web.Controllers;
using Snittlistan.Web.Infrastructure;
using Snittlistan.Web.Infrastructure.Attributes;
using Snittlistan.Web.Infrastructure.Database;
using Snittlistan.Web.TaskHandlers;

namespace Snittlistan.Web.Areas.V2.Controllers.Api;

[OnlyLocalAllowed]
public class TaskController : AbstractApiController
{
    public async Task<IHttpActionResult> Post(TaskRequest request)
    {
        Logger.InfoFormat("Received task {taskJson}", request.TaskJson);

        TaskBase taskObject = request.TaskJson.FromJson<TaskBase>();

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

        Type handlerType = typeof(ITaskHandler<>).MakeGenericType(taskObject.GetType());

        MethodInfo handleMethod = handlerType.GetMethod(nameof(ITaskHandler<TaskBase>.Handle));
        using IDisposable scope = NLog.NestedDiagnosticsLogicalContext.Push(taskObject.BusinessKey);
        Logger.Info("Begin");
        Tenant tenant = await CompositionRoot.GetCurrentTenant();
        Guid correlationId = request.CorrelationId ?? default;
        Guid causationId = request.MessageId ?? default;
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
        Task task = (Task)handleMethod.Invoke(handler, new[] { handlerContext });
        await task;
        Logger.Info("End");
        publishedTask.MarkHandled(DateTime.Now);

        return Ok();
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
