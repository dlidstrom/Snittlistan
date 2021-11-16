#nullable enable

namespace Snittlistan.Web.Infrastructure.Database
{
    using System;

    public class Tenant
    {
        public Tenant(
            int tenantId,
            string hostname,
            string favicon,
            string appleTouchIcon,
            string appleTouchIconSize,
            string webAppTitle,
            int clubId)
        {
            TenantId = tenantId;
            ClubId = clubId;
            Hostname = hostname;
            Favicon = favicon;
            AppleTouchIcon = appleTouchIcon;
            AppleTouchIconSize = appleTouchIconSize;
            WebAppTitle = webAppTitle;
            CreatedDate = DateTime.Now;
        }

        private Tenant()
        {
        }

        public int TenantId { get; set; }

        public int ClubId { get; }

        public string Hostname { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string Favicon { get; set; } = null!;

        public string AppleTouchIcon { get; set; } = null!;

        public string AppleTouchIconSize { get; set; } = null!;

        public string WebAppTitle { get; set; } = null!;

        public DateTime CreatedDate { get; private set; }
    }
}
