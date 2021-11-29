#nullable enable

namespace Snittlistan.Web.Areas.V2.Controllers.Api
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Newtonsoft.Json;
    using NLog;
    using Snittlistan.Queue;
    using Snittlistan.Queue.Messages;
    using Snittlistan.Web.Areas.V2.Tasks;
    using Snittlistan.Web.Controllers;
    using Snittlistan.Web.Infrastructure;
    using Snittlistan.Web.Infrastructure.Attributes;
    using Snittlistan.Web.Infrastructure.Database;

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

            Type handlerType = typeof(ITaskHandler<>).MakeGenericType(taskObject.GetType());
            object handler = Kernel.Resolve(handlerType);
            PropertyInfo? uriInfo = handler.GetType().GetProperty("TaskApiUri");
            if (uriInfo != null)
            {
                string uriString = Url.Link("DefaultApi", new { controller = "Task" });
                Uri uri = new(uriString);
                uriInfo.SetValue(handler, uri);
            }

            MethodInfo handleMethod = handler.GetType().GetMethod("Handle");
            IDisposable scope = NestedDiagnosticsLogicalContext.Push(taskObject.BusinessKey);
            try
            {
                Log.Info("Begin");
                Tenant tenant = await GetCurrentTenant();
                Guid correlationId = request.CorrelationId ?? default;
                Guid messageId = request.MessageId ?? default;
                IMessageContext messageContext = (IMessageContext)Activator.CreateInstance(
                    typeof(MessageContext<>).MakeGenericType(taskObject.GetType()),
                    taskObject,
                    tenant.TenantId,
                    correlationId,
                    messageId,
                    MsmqTransaction);
                messageContext.PublishMessageDelegate = async (task, tenantId, causationId, msmqTransaction) =>
                    await DoPublishMessage(request, task, tenant, causationId, msmqTransaction);

                Task task = (Task)handleMethod.Invoke(handler, new[] { messageContext });
                await task;
                Log.Info("End");
            }
            finally
            {
                scope.Dispose();
            }

            return Ok();
        }

        private Task DoPublishMessage(
            TaskRequest request,
            TaskBase task,
            Tenant tenant,
            Guid causationId,
            IMsmqTransaction msmqTransaction)
        {
            Guid correlationId = request.CorrelationId ?? default;
            MessageEnvelope envelope = new(
                task,
                tenant.TenantId,
                tenant.Hostname,
                correlationId,
                causationId,
                Guid.NewGuid());
            msmqTransaction.PublishMessage(envelope);
            _ = Databases.Snittlistan.PublishedTasks.Add(new(
                task,
                tenant.TenantId,
                correlationId,
                causationId,
                envelope.MessageId,
                "system"));
            return Task.CompletedTask;

            // TODO
            /**
             * All task publishing must be done from this controller. Move this lambda to a method.
             * Tool exe must have access to command/query. No database access, no queue access from the tool.
             * Only handle tasks that have been published. Check that they are in the database, set a date timestamp
             * on successful handling.
             */
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
}
