namespace Snittlistan.Test
{
    using System;
    using System.Web.Routing;

    using Snittlistan.Web.App_Start;

    using Xunit;

    public class RoutesTest : IDisposable
    {
        public RoutesTest()
        {
            new RouteConfig(RouteTable.Routes).Configure();
        }

        public void Dispose()
        {
            RouteTable.Routes.Clear();
        }

        [Fact]
        public void DefaultRoute()
        {
            RouteTable.Routes.Maps("GET", "~/", new { controller = "Home", action = "Index" });
        }

        [Fact]
        public void SessionApiRoute()
        {
            RouteTable.Routes.Maps("POST", "~/api/session", new { controller = "SessionApi", action = "Session" });
            RouteTable.Routes.Maps("DELETE", "~/api/session", new { controller = "SessionApi", action = "Session" });
        }

        [Fact]
        public void TurnsApiRoute()
        {
            RouteTable.Routes.Maps("POST", "~/api/turns", new { controller = "TurnsApi", action = "Turns" });
        }

        [Fact]
        public void V2Route()
        {
            RouteTable.Routes.Maps("GET", "~/v2", new { controller = "App", action = "Index" });
            RouteTable.Routes.Maps("GET", "~/v2/turns", new { controller = "App", action = "Index" });
        }

        [Fact]
        public void LowerCaseRoutes()
        {
            RouteTable.Routes.Maps("GET", "~/account/register", new { controller = "Account", action = "Register" });
        }

        [Fact]
        public void Shortcuts()
        {
            RouteTable.Routes.Maps("GET", "~/register", new { controller = "Account", action = "Register" });
            RouteTable.Routes.Maps("GET", "~/logon", new { controller = "Account", action = "LogOn" });
            RouteTable.Routes.Maps("GET", "~/about", new { controller = "Home", action = "About" });
        }

        [Fact]
        public void Verify()
        {
            RouteTable.Routes.Maps("GET", "~/verify", new { controller = "Account", action = "Verify" });
        }

        [Fact]
        public void Welcome()
        {
            RouteTable.Routes.Maps("GET", "~/welcome", new { controller = "Welcome", action = "Index" });
        }

        [Fact]
        public void Reset()
        {
            RouteTable.Routes.Maps("GET", "~/reset", new { controller = "Welcome", action = "Reset" });
        }

        [Fact]
        public void ElmahRoute()
        {
            RouteTable.Routes.Maps("GET", "~/admin/elmah", new { controller = "Elmah", action = "Index" });
            RouteTable.Routes.Maps("GET", "~/admin/elmah/detail", new { controller = "Elmah", action = "Index" });
            RouteTable.Routes.Maps("GET", "~/admin/elmah/stylesheet", new { controller = "Elmah", action = "Index" });
        }

        [Fact]
        public void HackerRoutes()
        {
            RouteTable.Routes.Maps("GET", "~/apps/phpalbum/main.php", new { controller = "Hacker", action = "Index" });
            RouteTable.Routes.Maps("GET", "~/apps/phpalbum/main.php?cmd=setquality&var1=1%27.passthru%28%27id%27%29.%27;", new { controller = "Hacker", action = "Index" });
            RouteTable.Routes.Maps("GET", "~/apps/phpAlbum/main.php", new { controller = "Hacker", action = "Index" });
            RouteTable.Routes.Maps("GET", "~/apps/phpAlbum/main.php?cmd=setquality&var1=1%27.passthru%28%27id%27%29.%27;", new { controller = "Hacker", action = "Index" });
            RouteTable.Routes.Maps("GET", "~/awstatstotals.php", new { controller = "Hacker", action = "Index" });
            RouteTable.Routes.Maps("GET", "~/awstatstotals.php?sort=%7b%24%7bpassthru%28chr(105)%2echr(100)%29%7d%7d%7b%24%7bexit%28%29%7d%7d", new { controller = "Hacker", action = "Index" });
            RouteTable.Routes.Maps("GET", "~/awstats/awstatstotals.php", new { controller = "Hacker", action = "Index" });
            RouteTable.Routes.Maps("GET", "~/awstats/awstatstotals.php?sort=%7b%24%7bpassthru%28chr(105)%2echr(100)%29%7d%7d%7b%24%7bexit%28%29%7d%7d", new { controller = "Hacker", action = "Index" });
            RouteTable.Routes.Maps("GET", "~/awstats/awstats.pl", new { controller = "Hacker", action = "Index" });
            RouteTable.Routes.Maps("GET", "~/awstats/awstats.pl?configdir=|echo;echo%20YYYAAZ;uname;id;echo%20YYY;echo|", new { controller = "Hacker", action = "Index" });
            RouteTable.Routes.Maps("GET", "~/awstatstotals/awstatstotals.php", new { controller = "Hacker", action = "Index" });
            RouteTable.Routes.Maps("GET", "~/awstatstotals/awstatstotals.php?sort=%7b%24%7bpassthru%28chr(105)%2echr(100)%29%7d%7d%7b%24%7bexit%28%29%7d%7d", new { controller = "Hacker", action = "Index" });
            RouteTable.Routes.Maps("GET", "~/catalog", new { controller = "Hacker", action = "Index" });
            RouteTable.Routes.Maps("GET", "~/cgi-bin/awstats.pl", new { controller = "Hacker", action = "Index" });
            RouteTable.Routes.Maps("GET", "~/cgi-bin/awstats.pl?configdir=|echo;echo%20YYYAAZ;uname;id;echo%20YYY;echo|", new { controller = "Hacker", action = "Index" });
            RouteTable.Routes.Maps("GET", "~/cgi-bin/awstats/awstats.pl", new { controller = "Hacker", action = "Index" });
            RouteTable.Routes.Maps("GET", "~/cgi-bin/awstats/awstats.pl?configdir=|echo;echo%20YYYAAZ;uname;id;echo%20YYY;echo|", new { controller = "Hacker", action = "Index" });
            RouteTable.Routes.Maps("GET", "~/cgi-bin/stats/awstats.pl", new { controller = "Hacker", action = "Index" });
            RouteTable.Routes.Maps("GET", "~/cgi-bin/stats/awstats.pl?configdir=|echo;echo%20YYYAAZ;uname;id;echo%20YYY;echo|", new { controller = "Hacker", action = "Index" });
            RouteTable.Routes.Maps("GET", "~/cgi/awstats/awstats.pl", new { controller = "Hacker", action = "Index" });
            RouteTable.Routes.Maps("GET", "~/cgi/awstats/awstats.pl?configdir=|echo;echo%20YYYAAZ;uname;id;echo%20YYY;echo|", new { controller = "Hacker", action = "Index" });
            RouteTable.Routes.Maps("GET", "~/index.php", new { controller = "Hacker", action = "Index" });
            RouteTable.Routes.Maps("GET", "~/index.php?option=com_simpledownload&controller=../../../../../../../../../../../../../../../proc/self/environ%00", new { controller = "Hacker", action = "Index" });
            RouteTable.Routes.Maps("GET", "~/main.php", new { controller = "Hacker", action = "Index" });
            RouteTable.Routes.Maps("GET", "~/main.php?cmd=setquality&var1=1%27.passthru%28%27id%27%29.%27;", new { controller = "Hacker", action = "Index" });
            RouteTable.Routes.Maps("GET", "~/phpalbum/main.php", new { controller = "Hacker", action = "Index" });
            RouteTable.Routes.Maps("GET", "~/phpalbum/main.php?cmd=setquality&var1=1%27.passthru%28%27id%27%29.%27;", new { controller = "Hacker", action = "Index" });
            RouteTable.Routes.Maps("GET", "~/phpAlbum/main.php", new { controller = "Hacker", action = "Index" });
            RouteTable.Routes.Maps("GET", "~/phpAlbum/main.php?cmd=setquality&var1=1%27.passthru%28%27id%27%29.%27;", new { controller = "Hacker", action = "Index" });
            RouteTable.Routes.Maps("GET", "~/scgi-bin/awstats.pl", new { controller = "Hacker", action = "Index" });
            RouteTable.Routes.Maps("GET", "~/scgi-bin/awstats.pl?configdir=|echo;echo%20YYYAAZ;uname;id;echo%20YYY;echo|", new { controller = "Hacker", action = "Index" });
            RouteTable.Routes.Maps("GET", "~/scgi-bin/awstats/awstats.pl", new { controller = "Hacker", action = "Index" });
            RouteTable.Routes.Maps("GET", "~/scgi-bin/awstats/awstats.pl?configdir=|echo;echo%20YYYAAZ;uname;id;echo%20YYY;echo|", new { controller = "Hacker", action = "Index" });
            RouteTable.Routes.Maps("GET", "~/scgi-bin/stats/awstats.pl", new { controller = "Hacker", action = "Index" });
            RouteTable.Routes.Maps("GET", "~/scgi-bin/stats/awstats.pl?configdir=|echo;echo%20YYYAAZ;uname;id;echo%20YYY;echo|", new { controller = "Hacker", action = "Index" });
            RouteTable.Routes.Maps("GET", "~/scgi/awstats/awstats.pl", new { controller = "Hacker", action = "Index" });
            RouteTable.Routes.Maps("GET", "~/scgi/awstats/awstats.pl?configdir=|echo;echo%20YYYAAZ;uname;id;echo%20YYY;echo|", new { controller = "Hacker", action = "Index" });
            RouteTable.Routes.Maps("GET", "~/scripts/awstats.pl", new { controller = "Hacker", action = "Index" });
            RouteTable.Routes.Maps("GET", "~/scripts/awstats.pl?configdir=|echo;echo%20YYYAAZ;uname;id;echo%20YYY;echo|", new { controller = "Hacker", action = "Index" });
            RouteTable.Routes.Maps("GET", "~/site.php", new { controller = "Hacker", action = "Index" });
            RouteTable.Routes.Maps("GET", "~/site.php?a={%24{passthru%28chr%28105%29.chr%28100%29%29}}", new { controller = "Hacker", action = "Index" });
            RouteTable.Routes.Maps("GET", "~/stat/awstatstotals.php", new { controller = "Hacker", action = "Index" });
            RouteTable.Routes.Maps("GET", "~/stat/awstatstotals.php?sort=%7b%24%7bpassthru%28chr(105)%2echr(100)%29%7d%7d%7b%24%7bexit%28%29%7d%7d", new { controller = "Hacker", action = "Index" });
            RouteTable.Routes.Maps("GET", "~/stats/awstats.pl", new { controller = "Hacker", action = "Index" });
            RouteTable.Routes.Maps("GET", "~/stats/awstats.pl?configdir=|echo;echo%20YYYAAZ;uname;id;echo%20YYY;echo|", new { controller = "Hacker", action = "Index" });
            RouteTable.Routes.Maps("GET", "~/shop", new { controller = "Hacker", action = "Index" });
        }
    }
}