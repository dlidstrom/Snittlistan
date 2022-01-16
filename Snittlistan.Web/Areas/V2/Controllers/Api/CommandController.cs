#nullable enable

using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Web.Http;
using Snittlistan.Web.Infrastructure.Attributes;
using Newtonsoft.Json;
using Snittlistan.Web.ExternalCommands;
using Snittlistan.Queue.ExternalCommands;
using Castle.MicroKernel;
using Snittlistan.Web.Infrastructure.Database;
using Snittlistan.Queue;
using Castle.Core.Logging;

namespace Snittlistan.Web.Areas.V2.Controllers.Api;

[OnlyLocalAllowed]
public class CommandController : ApiController
{
    public IKernel Kernel { get; set; } = null!;

    public Databases Databases { get; set; } = null!;

    public ILogger Logger { get; set; } = NullLogger.Instance;

    public async Task<IHttpActionResult> Post(CommandRequest request)
    {
        object commandObject = request.CommandJson.FromJson<object>();
        Type handlerType = typeof(ICommandHandler<>).MakeGenericType(commandObject.GetType());
        object handler = Kernel.Resolve(handlerType);
        MethodInfo handleMethod = handler.GetType().GetMethod(nameof(ICommandHandler<CommandBase>.Handle));
        Task task = (Task)handleMethod.Invoke(handler, new[] { commandObject });
        await task;

        int changesSaved = await Databases.Snittlistan.SaveChangesAsync();
        if (changesSaved > 0)
        {
            Logger.InfoFormat("saved {changesSaved} to database", changesSaved);
        }

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
