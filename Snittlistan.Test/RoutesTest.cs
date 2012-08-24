namespace Snittlistan.Test
{
    using System;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Controllers;
    using Infrastructure;
    using MvcContrib.TestHelper;
    using Xunit;

    public class RoutesTest : IDisposable
    {
        public RoutesTest()
        {
            new RouteConfigurator(RouteTable.Routes).Configure();
        }

        public void Dispose()
        {
            RouteTable.Routes.Clear();
        }

        [Fact]
        public void DefaultRoute()
        {
            "~/".ShouldMapTo<HomeController>(c => c.Index());
        }

        [Fact]
        public void ApiRoute()
        {
            RouteTable.Routes.Maps("POST", "~/api/session", new { controller = "Api", action = "Session" });
            RouteTable.Routes.Maps("DELETE", "~/api/session", new { controller = "Api", action = "Session" });
        }

        [Fact]
        public void V2Route()
        {
            "~/v2".ShouldMapTo<AppController>(c => c.Index());
            "~/v2/turns".ShouldMapTo<AppController>(c => c.Index());
        }

        [Fact]
        public void LowerCaseRoutes()
        {
            "~/account/register".ShouldMapTo<AccountController>(c => c.Register());
        }

        [Fact]
        public void Shortcuts()
        {
            "~/register".ShouldMapTo<AccountController>(c => c.Register());
            "~/logon".ShouldMapTo<AccountController>(c => c.LogOn());
            "~/about".ShouldMapTo<HomeController>(c => c.About());
        }

        [Fact]
        public void Verify()
        {
            var verify = "~/verify".WithMethod(HttpVerbs.Post);
            var guid = Guid.NewGuid();
            verify.Values["activationKey"] = guid.ToString();
            verify.ShouldMapTo<AccountController>(c => c.Verify(guid));
        }

        [Fact]
        public void Welcome()
        {
            "~/welcome".ShouldMapTo<WelcomeController>(c => c.Index());
        }

        [Fact]
        public void Reset()
        {
            "~/reset".ShouldMapTo<WelcomeController>(c => c.Reset());
        }

        [Fact]
        public void ElmahRoute()
        {
            "~/admin/elmah".ShouldMapTo<ElmahController>(c => c.Index(null));
            "~/admin/elmah/detail".ShouldMapTo<ElmahController>(c => c.Index("detail"));
            "~/admin/elmah/stylesheet".ShouldMapTo<ElmahController>(c => c.Index("stylesheet"));
        }

        [Fact]
        public void HackerRoutes()
        {
            "~/apps/phpalbum/main.php".ShouldMapTo<HackerController>(c => c.Index());
            "~/apps/phpalbum/main.php?cmd=setquality&var1=1%27.passthru%28%27id%27%29.%27;".ShouldMapTo<HackerController>(c => c.Index());
            "~/apps/phpAlbum/main.php".ShouldMapTo<HackerController>(c => c.Index());
            "~/apps/phpAlbum/main.php?cmd=setquality&var1=1%27.passthru%28%27id%27%29.%27;".ShouldMapTo<HackerController>(c => c.Index());
            "~/awstatstotals.php".ShouldMapTo<HackerController>(c => c.Index());
            "~/awstatstotals.php?sort=%7b%24%7bpassthru%28chr(105)%2echr(100)%29%7d%7d%7b%24%7bexit%28%29%7d%7d".ShouldMapTo<HackerController>(c => c.Index());
            "~/awstats/awstatstotals.php".ShouldMapTo<HackerController>(c => c.Index());
            "~/awstats/awstatstotals.php?sort=%7b%24%7bpassthru%28chr(105)%2echr(100)%29%7d%7d%7b%24%7bexit%28%29%7d%7d".ShouldMapTo<HackerController>(c => c.Index());
            "~/awstats/awstats.pl".ShouldMapTo<HackerController>(c => c.Index());
            "~/awstats/awstats.pl?configdir=|echo;echo%20YYYAAZ;uname;id;echo%20YYY;echo|".ShouldMapTo<HackerController>(c => c.Index());
            "~/awstatstotals/awstatstotals.php".ShouldMapTo<HackerController>(c => c.Index());
            "~/awstatstotals/awstatstotals.php?sort=%7b%24%7bpassthru%28chr(105)%2echr(100)%29%7d%7d%7b%24%7bexit%28%29%7d%7d".ShouldMapTo<HackerController>(c => c.Index());
            "~/catalog".ShouldMapTo<HackerController>(c => c.Index());
            "~/cgi-bin/awstats.pl".ShouldMapTo<HackerController>(c => c.Index());
            "~/cgi-bin/awstats.pl?configdir=|echo;echo%20YYYAAZ;uname;id;echo%20YYY;echo|".ShouldMapTo<HackerController>(c => c.Index());
            "~/cgi-bin/awstats/awstats.pl".ShouldMapTo<HackerController>(c => c.Index());
            "~/cgi-bin/awstats/awstats.pl?configdir=|echo;echo%20YYYAAZ;uname;id;echo%20YYY;echo|".ShouldMapTo<HackerController>(c => c.Index());
            "~/cgi-bin/stats/awstats.pl".ShouldMapTo<HackerController>(c => c.Index());
            "~/cgi-bin/stats/awstats.pl?configdir=|echo;echo%20YYYAAZ;uname;id;echo%20YYY;echo|".ShouldMapTo<HackerController>(c => c.Index());
            "~/cgi/awstats/awstats.pl".ShouldMapTo<HackerController>(c => c.Index());
            "~/cgi/awstats/awstats.pl?configdir=|echo;echo%20YYYAAZ;uname;id;echo%20YYY;echo|".ShouldMapTo<HackerController>(c => c.Index());
            "~/index.php".ShouldMapTo<HackerController>(c => c.Index());
            "~/index.php?option=com_simpledownload&controller=../../../../../../../../../../../../../../../proc/self/environ%00".ShouldMapTo<HackerController>(c => c.Index());
            "~/main.php".ShouldMapTo<HackerController>(c => c.Index());
            "~/main.php?cmd=setquality&var1=1%27.passthru%28%27id%27%29.%27;".ShouldMapTo<HackerController>(c => c.Index());
            "~/phpalbum/main.php".ShouldMapTo<HackerController>(c => c.Index());
            "~/phpalbum/main.php?cmd=setquality&var1=1%27.passthru%28%27id%27%29.%27;".ShouldMapTo<HackerController>(c => c.Index());
            "~/phpAlbum/main.php".ShouldMapTo<HackerController>(c => c.Index());
            "~/phpAlbum/main.php?cmd=setquality&var1=1%27.passthru%28%27id%27%29.%27;".ShouldMapTo<HackerController>(c => c.Index());
            "~/scgi-bin/awstats.pl".ShouldMapTo<HackerController>(c => c.Index());
            "~/scgi-bin/awstats.pl?configdir=|echo;echo%20YYYAAZ;uname;id;echo%20YYY;echo|".ShouldMapTo<HackerController>(c => c.Index());
            "~/scgi-bin/awstats/awstats.pl".ShouldMapTo<HackerController>(c => c.Index());
            "~/scgi-bin/awstats/awstats.pl?configdir=|echo;echo%20YYYAAZ;uname;id;echo%20YYY;echo|".ShouldMapTo<HackerController>(c => c.Index());
            "~/scgi-bin/stats/awstats.pl".ShouldMapTo<HackerController>(c => c.Index());
            "~/scgi-bin/stats/awstats.pl?configdir=|echo;echo%20YYYAAZ;uname;id;echo%20YYY;echo|".ShouldMapTo<HackerController>(c => c.Index());
            "~/scgi/awstats/awstats.pl".ShouldMapTo<HackerController>(c => c.Index());
            "~/scgi/awstats/awstats.pl?configdir=|echo;echo%20YYYAAZ;uname;id;echo%20YYY;echo|".ShouldMapTo<HackerController>(c => c.Index());
            "~/scripts/awstats.pl".ShouldMapTo<HackerController>(c => c.Index());
            "~/scripts/awstats.pl?configdir=|echo;echo%20YYYAAZ;uname;id;echo%20YYY;echo|".ShouldMapTo<HackerController>(c => c.Index());
            "~/site.php".ShouldMapTo<HackerController>(c => c.Index());
            "~/site.php?a={%24{passthru%28chr%28105%29.chr%28100%29%29}}".ShouldMapTo<HackerController>(c => c.Index());
            "~/stat/awstatstotals.php".ShouldMapTo<HackerController>(c => c.Index());
            "~/stat/awstatstotals.php?sort=%7b%24%7bpassthru%28chr(105)%2echr(100)%29%7d%7d%7b%24%7bexit%28%29%7d%7d".ShouldMapTo<HackerController>(c => c.Index());
            "~/stats/awstats.pl".ShouldMapTo<HackerController>(c => c.Index());
            "~/stats/awstats.pl?configdir=|echo;echo%20YYYAAZ;uname;id;echo%20YYY;echo|".ShouldMapTo<HackerController>(c => c.Index());
            "~/shop".ShouldMapTo<HackerController>(c => c.Index());
        }
    }
}