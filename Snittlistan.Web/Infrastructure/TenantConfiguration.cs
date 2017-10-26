using System;

namespace Snittlistan.Web.Infrastructure
{
    public class TenantConfiguration
    {
        public TenantConfiguration(string database)
        {
            if (database == null) throw new ArgumentNullException(nameof(database));
            Database = database;
        }

        public string Database { get; }
    }
}