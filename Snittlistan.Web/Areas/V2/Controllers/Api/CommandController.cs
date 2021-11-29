#nullable enable

namespace Snittlistan.Web.Areas.V2.Controllers.Api
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Infrastructure.Attributes;
    using Newtonsoft.Json;
    using Snittlistan.Web.Commands;
    using Web.Controllers;

    [OnlyLocalAllowed]
    public class CommandController : AbstractApiController
    {
        public async Task<IHttpActionResult> Post(CommandRequest request)
        {
            object? commandObject = JsonConvert.DeserializeObject(
                request.CommandJson,
                new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
            if (commandObject is null)
            {
                return BadRequest("failed to deserialize");
            }

            Type handlerType = typeof(ICommandHandler<>).MakeGenericType(commandObject.GetType());
            object handler = Kernel.Resolve(handlerType);
            MethodInfo handleMethod = handler.GetType().GetMethod("Handle");
            Task task = (Task)handleMethod.Invoke(handler, new[] { commandObject });
            await task;
            return Ok();
        }

        public class CommandRequest
        {
            public CommandRequest(string commandJson)
            {
                CommandJson = commandJson;
            }

            [Required]
            public string CommandJson { get; }
        }
    }
}
