#nullable enable

namespace Snittlistan.Web.Infrastructure.Database
{
    public class Databases
    {
        public Databases(
            SnittlistanContext snittlistanContext,
            BitsContext bitsContext)
        {
            Snittlistan = snittlistanContext;
            Bits = bitsContext;
        }

        public SnittlistanContext Snittlistan { get; }

        public BitsContext Bits { get; }
    }
}
