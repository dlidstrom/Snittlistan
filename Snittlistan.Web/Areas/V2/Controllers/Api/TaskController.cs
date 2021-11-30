using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Reflection;
using System.Web.Http;
using Newtonsoft.Json;
using NLog;
using Snittlistan.Queue.Messages;
using Snittlistan.Web.Areas.V2.Tasks;
using Snittlistan.Web.Controllers;
using Snittlistan.Web.Infrastructure;
using Snittlistan.Web.Infrastructure.Attributes;
using Snittlistan.Web.Infrastructure.Database;

#nullable enable

namespace Snittlistan.Web.Areas.V2.Controllers.Api;
[OnlyLocalAllowed]
public class TaskController : AbstractApiController
{
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    public async Task<IHttpActionResult> Post(TaskRequest request)
    {
        Log.Info($"Received task {request.TaskJson}");

        TaskBase? taskObject = JsonConvert.DeserializeObject<TaskBase>(
            request.TaskJson,
            new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
        if (taskObject is null)
        {
            return BadRequest("could not deserialize task json");
        }

        // check for published task
        PublishedTask? publishedTask = await Databases.Snittlistan.PublishedTasks.SingleOrDefaultAsync(x => x.MessageId == request.MessageId);
        if (publishedTask is null)
        {
            return BadRequest($"No published task found with message id {request.MessageId}");
        }

        if (publishedTask.HandledDate.HasValue)
        {
            return Ok($"task with message id {publishedTask.MessageId} already handled");
        }

        Type handlerType = typeof(ITaskHandler<>).MakeGenericType(taskObject.GetType());

        MethodInfo handleMethod = handlerType.GetMethod("Handle");
        using IDisposable scope = NestedDiagnosticsLogicalContext.Push(taskObject.BusinessKey);
        Log.Info("Begin");
        Tenant tenant = await GetCurrentTenant();
        Guid correlationId = request.CorrelationId ?? default;
        Guid causationId = request.MessageId ?? default;
        TaskPublisher taskPublisher = new(tenant, Databases, correlationId, causationId);
        IMessageContext messageContext = (IMessageContext)Activator.CreateInstance(
            typeof(MessageContext<>).MakeGenericType(taskObject.GetType()),
            taskObject,
            tenant,
            correlationId,
            causationId);
        messageContext.PublishMessageDelegate = task =>
            taskPublisher.PublishTask(task, "system");

        object handler = Kernel.Resolve(handlerType);
        Task task = (Task)handleMethod.Invoke(handler, new[] { messageContext });
        await task;
        Log.Info("End");
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
