#nullable enable

namespace Snittlistan.Web.Infrastructure.Database;

public class DatabasesFactory
{
    private readonly Func<Databases> databasesFactory;

    public DatabasesFactory(Func<Databases> databasesFactory)
    {
        this.databasesFactory = databasesFactory;
    }

    public Databases Create()
    {
        Databases databases = databasesFactory.Invoke();
        return databases;
    }
}
