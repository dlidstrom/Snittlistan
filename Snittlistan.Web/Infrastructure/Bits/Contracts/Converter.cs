namespace Snittlistan.Web.Infrastructure.Bits.Contracts
{
    using Newtonsoft.Json;

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings();
    }
}