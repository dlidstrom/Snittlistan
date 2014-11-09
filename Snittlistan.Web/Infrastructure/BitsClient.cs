using System.Net;
using NLog;

namespace Snittlistan.Web.Infrastructure
{
    public class BitsClient : IBitsClient
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public string DownloadMatchResult(int bitsMatchId)
        {
            using (var client = new WebClient())
            {
                var address = string.Format("http://bits.swebowl.se/Matches/MatchFact.aspx?MatchId={0}", bitsMatchId);
                Log.Info("Downloading {0}", address);
                var content = client.DownloadString(address);
                Log.Info("Match {0} raw content: {1}", bitsMatchId, content);
                return content;
            }
        }
    }
}