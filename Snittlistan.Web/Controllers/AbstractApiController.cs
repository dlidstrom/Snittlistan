#nullable enable

using System.Web.Http;
using NLog;
using Snittlistan.Web.Infrastructure;
using Snittlistan.Web.Infrastructure.Attributes;

namespace Snittlistan.Web.Controllers;

[SaveChanges]
public abstract class AbstractApiController : ApiController
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    public CompositionRoot CompositionRoot { get; set; } = null!;

    [NonAction]
    public async Task SaveChangesAsync()
    {
        int changesSaved = await CompositionRoot.Databases.Snittlistan.SaveChangesAsync();
        if (changesSaved > 0)
        {
            Logger.Info("saved {changesSaved} to database", changesSaved);
        }

        CompositionRoot.EventStoreSession.SaveChanges();
    }
}
