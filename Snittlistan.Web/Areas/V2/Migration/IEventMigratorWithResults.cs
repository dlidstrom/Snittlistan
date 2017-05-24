using EventStoreLite;

namespace Snittlistan.Web.Areas.V2.Migration
{
    public interface IEventMigratorWithResults : IEventMigrator
    {
        string GetResults();
    }
}