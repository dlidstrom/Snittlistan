namespace Snittlistan.Web.Areas.V2.Migration
{
    using EventStoreLite;

    public interface IEventMigratorWithResults : IEventMigrator
    {
        string GetResults();
    }
}