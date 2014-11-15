namespace Snittlistan.Web.Infrastructure
{
    public interface IBitsClient
    {
        string DownloadMatchResult(int bitsMatchId);
    }
}