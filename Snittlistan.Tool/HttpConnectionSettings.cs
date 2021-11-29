#nullable enable

namespace Snittlistan.Tool
{
    public class HttpConnectionSettings
    {
        public HttpConnectionSettings(string scheme, int port)
        {
            UrlScheme = scheme;
            Port = port;
        }

        public string UrlScheme { get; }

        public int Port { get; }
    }
}
