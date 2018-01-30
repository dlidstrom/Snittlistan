using System;
using System.IO;
using Snittlistan.Web.Infrastructure;

namespace Snittlistan.Test
{
    public static class BitsGateway
    {
        public static string GetMatch(int bitsMatchId)
        {
            var bitsClient = new BitsClient();
            string content;
            var directoryName = Directory.GetCurrentDirectory();
            var path = Path.Combine(
                directoryName,
                $"BitsMatch-{bitsMatchId}.html");
            try
            {
                content = File.ReadAllText(path);
            }
            catch (Exception)
            {
                content = bitsClient.DownloadMatchResult(bitsMatchId);
                File.WriteAllText(path, content);
            }

            return content;
        }
    }
}