namespace Snittlistan.Web.Models
{
    public class SiteWideConfiguration
    {
        public const string GlobalId = "SiteWideConfig";

        public SiteWideConfiguration(string databaseUrl, TenantConfiguration[] tenantConfigurations)
        {
            Id = GlobalId;
            DatabaseUrl = databaseUrl;
            TenantConfigurations = tenantConfigurations ?? new TenantConfiguration[0];
        }

        public string Id { get; }

        public string DatabaseUrl { get; }

        public TenantConfiguration[] TenantConfigurations { get; }
    }
}