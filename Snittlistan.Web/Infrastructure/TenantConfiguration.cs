using System;

namespace Snittlistan.Web.Infrastructure
{
    public class TenantConfiguration
    {
        public TenantConfiguration(string name, string database, string connectionStringName, string[] teamNames, bool hasV1)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (database == null) throw new ArgumentNullException(nameof(database));
            if (connectionStringName == null) throw new ArgumentNullException(nameof(connectionStringName));
            if (teamNames == null) throw new ArgumentNullException(nameof(teamNames));
            Name = name;
            Database = database;
            ConnectionStringName = connectionStringName;
            TeamNames = teamNames;
            HasV1 = hasV1;
        }

        public string Name { get; }

        public string Database { get; }

        public string ConnectionStringName { get; }

        public string[] TeamNames { get; }

        public bool HasV1 { get; }
    }
}