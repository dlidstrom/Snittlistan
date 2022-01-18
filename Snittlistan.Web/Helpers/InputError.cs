using Raven.Imports.Newtonsoft.Json;

namespace Snittlistan.Web.Helpers;
public class InputError
{
    public InputError(string field, string reason)
    {
        Field = field ?? throw new ArgumentNullException(nameof(field));
        Reason = reason ?? throw new ArgumentNullException(nameof(reason));
    }

    [JsonProperty(PropertyName = "field")]
    public string Field { get; set; }

    [JsonProperty(PropertyName = "reason")]
    public string Reason { get; set; }
}
