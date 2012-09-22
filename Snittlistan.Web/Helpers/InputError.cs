namespace Snittlistan.Web.Helpers
{
    using System;

    using Raven.Imports.Newtonsoft.Json;

    public class InputError
    {
        public InputError(string field, string reason)
        {
            if (field == null) throw new ArgumentNullException("field");
            if (reason == null) throw new ArgumentNullException("reason");
            this.Field = field;
            this.Reason = reason;
        }

        [JsonProperty(PropertyName = "field")]
        public string Field { get; set; }

        [JsonProperty(PropertyName = "reason")]
        public string Reason { get; set; }
    }
}