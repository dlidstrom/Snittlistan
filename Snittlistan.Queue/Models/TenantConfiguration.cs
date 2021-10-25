namespace Snittlistan.Queue.Models
{
    using System;

    public class TenantConfiguration
    {
        public TenantConfiguration(
            string hostname,
            string databaseName,
            string favicon,
            string appleTouchIcon,
            string appleTouchIconSize,
            string webAppTitle,
            string fullTeamName,
            int tenantId)
        {
            Hostname = hostname ?? throw new ArgumentNullException(nameof(hostname));
            DatabaseName = databaseName ?? throw new ArgumentNullException(nameof(databaseName));
            Favicon = favicon ?? throw new ArgumentNullException(nameof(favicon));
            AppleTouchIcon = appleTouchIcon ?? throw new ArgumentNullException(nameof(appleTouchIcon));
            AppleTouchIconSize = appleTouchIconSize ?? throw new ArgumentNullException(nameof(appleTouchIconSize));
            WebAppTitle = webAppTitle ?? throw new ArgumentNullException(nameof(webAppTitle));
            FullTeamName = fullTeamName ?? throw new ArgumentNullException(nameof(fullTeamName));
            TenantId = tenantId;
        }

        public string Hostname { get; }

        public string DatabaseName { get; }

        public string Favicon { get; }

        public string AppleTouchIcon { get; }

        public string AppleTouchIconSize { get; }

        public string WebAppTitle { get; }

        public string FullTeamName { get; }

        public int TenantId { get; }
    }
}
