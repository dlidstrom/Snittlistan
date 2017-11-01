using System;
using Raven.Imports.Newtonsoft.Json;

namespace Snittlistan.Web.Helpers
{
    public class InputError
    {
        public InputError(string field, string reason)
        {
            if (field == null) throw new ArgumentNullException(nameof(field));
            if (reason == null) throw new ArgumentNullException(nameof(reason));
            Field = field;
            Reason = reason;
        }

        [JsonProperty(PropertyName = "field")]
        public string Field { get; set; }

        [JsonProperty(PropertyName = "reason")]
        public string Reason { get; set; }
    }
}