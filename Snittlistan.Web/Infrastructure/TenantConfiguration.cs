using System;

namespace Snittlistan.Web.Infrastructure
{
    public class TenantConfiguration
    {
        public TenantConfiguration(
            string name,
            string database,
            string connectionStringName,
            string favicon,
            string appleTouchIcon,
            string appleTouchIconSize,
            string webAppTitle)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (database == null) throw new ArgumentNullException(nameof(database));
            if (connectionStringName == null) throw new ArgumentNullException(nameof(connectionStringName));
            if (favicon == null) throw new ArgumentNullException(nameof(favicon));
            if (appleTouchIcon == null) throw new ArgumentNullException(nameof(appleTouchIcon));
            if (appleTouchIconSize == null) throw new ArgumentNullException(nameof(appleTouchIconSize));
            if (webAppTitle == null) throw new ArgumentNullException(nameof(webAppTitle));
            Name = name;
            Database = database;
            ConnectionStringName = connectionStringName;
            Favicon = favicon;
            AppleTouchIcon = appleTouchIcon;
            AppleTouchIconSize = appleTouchIconSize;
            WebAppTitle = webAppTitle;
        }

        public string Name { get; }

        public string Database { get; }

        public string ConnectionStringName { get; }

        public string Favicon { get; }

        public string AppleTouchIcon { get; }

        public string AppleTouchIconSize { get; }

        public string WebAppTitle { get; }
    }
}