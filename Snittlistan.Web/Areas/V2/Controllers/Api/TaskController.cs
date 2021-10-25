#nullable enable

namespace Snittlistan.Web.Areas.V2.Controllers.Api
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity;
    using System.Linq;
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

        public async Task<IHttpActionResult> Get()
        {
            return Ok(await Database.DelayedTasks.Take(10).ToListAsync());
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
                MessageContext messageContext = new(
                    taskObject,
                    TenantConfiguration.TenantId,
                    request.CorrelationId,
                    request.CausationId,
                    MsmqTransaction);
                Task task = (Task)handleMethod.Invoke(handler, new[] { taskObject });
                await task;
                Log.Info("End");
            }
            finally
            {
                scope.Dispose();
            }

            return Ok();
        }

        public class TaskRequest
        {
            public TaskRequest(string taskJson)
            {
                TaskJson = taskJson;
            }

            [Required]
            public string TaskJson { get; }
        }
    }
}
