
using Newtonsoft.Json;

#nullable enable

namespace Snittlistan.Web.Infrastructure.Bits.Contracts;
public class MatchOilProfile
{
    [JsonProperty("oilPatternId")]
    public long OilPatternId { get; set; }

    [JsonProperty("oilPatternName")]
    public string? OilPatternName { get; set; }
}
