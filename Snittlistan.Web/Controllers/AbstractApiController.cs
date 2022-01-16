#nullable enable

using System.Web.Http;
using Castle.Core.Logging;
using Snittlistan.Web.Infrastructure;
using Snittlistan.Web.Infrastructure.Attributes;

namespace Snittlistan.Web.Controllers;

[SaveChanges]
public abstract class AbstractApiController : ApiController
{
    public CompositionRoot CompositionRoot { get; set; } = null!;

    public ILogger Logger { get; set; } = NullLogger.Instance;

    [NonAction]
    public async Task SaveChangesAsync()
    {
        int changesSaved = await CompositionRoot.Databases.Snittlistan.SaveChangesAsync();
        if (changesSaved > 0)
        {
            Logger.InfoFormat(
                "saved {changesSaved} to database",
                changesSaved);
        }

        CompositionRoot.EventStoreSession.SaveChanges();
    }
}
