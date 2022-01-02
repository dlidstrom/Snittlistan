#nullable enable

using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Web.Http;
using Snittlistan.Web.Infrastructure.Attributes;
using Newtonsoft.Json;
using Snittlistan.Web.Commands;
using Snittlistan.Web.Controllers;

namespace Snittlistan.Web.Areas.V2.Controllers.Api;

[OnlyLocalAllowed]
public class CommandController : AbstractApiController
{
    private readonly JsonSerializerSettings settings = new()
    {
        TypeNameHandling = TypeNameHandling.All,
        MetadataPropertyHandling = MetadataPropertyHandling.ReadAhead
    };

    public async Task<IHttpActionResult> Post(CommandRequest request)
    {
        object? commandObject = JsonConvert.DeserializeObject(
            request.CommandJson,
            settings);
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
