#nullable enable

using System.Data.Entity;
using System.Web.Http;
using Castle.MicroKernel;
using EventStoreLite;
using NLog;
using Snittlistan.Web.Infrastructure;
using Snittlistan.Web.Infrastructure.Attributes;
using Snittlistan.Web.Infrastructure.Database;

namespace Snittlistan.Web.Controllers;

[SaveChanges]
public abstract class AbstractApiController : ApiController
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    public IKernel Kernel { get; set; } = null!;

    public Databases Databases { get; set; } = null!;

    public EventStore EventStore { get; set; } = null!;

    public IEventStoreSession EventStoreSession { get; set; } = null!;

    [NonAction]
    public async Task SaveChangesAsync()
    {
        int changesSaved = await Databases.Snittlistan.SaveChangesAsync();
        if (changesSaved > 0)
        {
            Logger.Info("saved {changesSaved} to database", changesSaved);
        }

        EventStoreSession.SaveChanges();
    }

    protected async Task<Tenant> GetCurrentTenant()
    {
        string hostname = CurrentHttpContext.Instance().Request.ServerVariables["SERVER_NAME"];
        Tenant tenant = await Databases.Snittlistan.Tenants.SingleOrDefaultAsync(x => x.Hostname == hostname);
        if (tenant == null)
        {
            throw new Exception($"No tenant found for hostname '{hostname}'");
        }

        return tenant;
    }
}
