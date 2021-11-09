#nullable enable

namespace Snittlistan.Web.Areas.V2.Controllers.Api
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Castle.MicroKernel;
    using Newtonsoft.Json;
    using NLog;
    using Snittlistan.Queue.Messages;
    using Snittlistan.Web.Areas.V2.Tasks;
    using Snittlistan.Web.Controllers;
    using Snittlistan.Web.Infrastructure.Attributes;

    [OnlyLocalAllowed]
    public class TaskController : AbstractApiController
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private readonly IKernel kernel;

        public TaskController(IKernel kernel)
        {
            this.kernel = kernel;
        }

        public async Task<IHttpActionResult> Post(TaskRequest request)
        {
            Log.Info($"Received task {request.TaskJson}");
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            ITask? taskObject = JsonConvert.DeserializeObject<ITask>(
                request.TaskJson,
                new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
            if (taskObject is null)
            {
                return BadRequest("could not deserialize task json");
            }

            Type handlerType = typeof(ITaskHandler<>).MakeGenericType(taskObject.GetType());
            object handler = kernel.Resolve(handlerType);
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
                IMessageContext messageContext = (IMessageContext)Activator.CreateInstance(
                    typeof(MessageContext<>).MakeGenericType(taskObject.GetType()),
                    taskObject,
                    TenantConfiguration.TenantId,
                    request.CorrelationId,
                    request.MessageId,
                    MsmqTransaction);
                messageContext.PublishMessage = task => DoPublishMessage(request, task);

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

        private void DoPublishMessage(TaskRequest request, ITask task)
        {
            // TODO save to database
            Guid correlationId = request.CorrelationId ?? default;
            MessageEnvelope envelope = new(
                task,
                TenantConfiguration.TenantId,
                correlationId,
                request.MessageId,
                Guid.NewGuid());
            MsmqTransaction.PublishMessage(envelope);
            _ = Databases.Snittlistan.PublishedTasks.Add(new(
                task,
                TenantConfiguration.TenantId,
                correlationId,
                request.MessageId,
                envelope.MessageId,
                "system"));

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
