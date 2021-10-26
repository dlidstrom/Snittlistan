#nullable enable


namespace Snittlistan.Queue.Infrastructure
{
    public class Tenant
    {
        public int TenantId { get; set; }

        public string Hostname { get; set; } = null!;
    }
}
