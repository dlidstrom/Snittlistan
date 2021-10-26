namespace Snittlistan.Tool.Tasks
{
    using System.Linq;
    using Snittlistan.Queue.Infrastructure;

    public static class CommandLineTaskHelper
    {
        public static Tenant[] Tenants()
        {
            using DatabaseContext context = new();
            Tenant[] tenants = context.Tenants.ToArray();
            return tenants;
        }
    }
}
