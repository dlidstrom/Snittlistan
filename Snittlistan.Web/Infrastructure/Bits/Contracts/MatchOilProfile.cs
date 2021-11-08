#nullable enable

namespace Snittlistan.Web.Infrastructure.Bits.Contracts
{
    using Newtonsoft.Json;

    public class MatchOilProfile
    {
        [JsonProperty("oilPatternId")]
        public long OilPatternId { get; set; }

        [JsonProperty("oilPatternName")]
        public string? OilPatternName { get; set; }
    }
}
