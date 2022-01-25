#nullable enable

using Newtonsoft.Json;

namespace Snittlistan.Queue;

public static class Extensions
{
    private static readonly JsonSerializerSettings defaultSettings = new()
    {
        TypeNameHandling = TypeNameHandling.All,
        MetadataPropertyHandling = MetadataPropertyHandling.ReadAhead,
        Formatting = Formatting.Indented
    };

    public static string ToJson(this object o, JsonSerializerSettings? settings = null)
    {
        string json = JsonConvert.SerializeObject(o, settings ?? defaultSettings);
        return json;
    }

    public static TResult FromJson<TResult>(this string str)
    {
        TResult? ob = JsonConvert.DeserializeObject<TResult>(str, defaultSettings);
        return ob == null ? throw new Exception("Failed to deserialize") : ob;
    }
}
