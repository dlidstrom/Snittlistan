namespace Snittlistan.Test
{
    using System;
    using System.Web.Mvc;
    using System.Web.Routing;
    using NUnit.Framework;
    using Web;
    using Web.Areas.V1;
    using Web.Areas.V2;

    [TestFixture]
    public class RoutesTest
    {
        [SetUp]
        public void SetUp()
        {
            new RouteConfig(RouteTable.Routes).Configure();
            RegisterArea<V1AreaRegistration>(RouteTable.Routes, null);
            RegisterArea<V2AreaRegistration>(RouteTable.Routes, null);
        }

        [TearDown]
        public void TearDown()
        {
            RouteTable.Routes.Clear();
        }

        [Test]
        public void DefaultRoute()
        {
            RouteTable.Routes.Maps("GET", "~/", new { controller = "Roster", action = "Index" });
        }

        [Test]
        public void LegacyRoutes()
        {
            RouteTable.Routes.Maps("GET", "~/v1", new { controller = "Home", action = "Index" });
            RouteTable.Routes.Maps("GET", "~/v1/Match", new { controller = "Match", action = "Index" });
            RouteTable.Routes.Maps("GET", "~/v1/Match/Register8x4", new { controller = "Match", action = "Register8x4" });
            RouteTable.Routes.Maps("GET", "~/v1/Match/Register4x4", new { controller = "Match", action = "Register4x4" });
            RouteTable.Routes.Maps("GET", "~/v1/About", new { controller = "Home", action = "About" });
            RouteTable.Routes.Maps("GET", "~/Home/About", new { controller = "Home", action = "About" });
        }

        [Test]
        public void V2Route()
        {
            RouteTable.Routes.Maps("GET", "~/", new { controller = "Roster", action = "Index" });
            RouteTable.Routes.Maps("GET", "~/Roster/Create", new { controller = "Roster", action = "Create" });
            RouteTable.Routes.Maps("GET", "~/Roster/Edit/roster-1", new { controller = "Roster", action = "Edit", id = "roster-1" });
            RouteTable.Routes.Maps("GET", "~/Roster/Delete/roster-1", new { controller = "Roster", action = "Delete", id = "roster-1" });
            RouteTable.Routes.Maps("GET", "~/RosterView/2012", new { controller = "Roster", action = "View", season = 2012, turn = (int?)null });
            RouteTable.Routes.Maps("GET", "~/Roster/Players/roster-1", new { controller = "Roster", action = "EditPlayers", id = "roster-1" });
        }

        [Test]
        public void SearchRoute()
        {
            RouteTable.Routes.Maps("GET", "~/search/teams", new { controller = "SearchTerms", action = "Teams" });
            RouteTable.Routes.Maps("GET", "~/search/opponents", new { controller = "SearchTerms", action = "Opponents" });
            RouteTable.Routes.Maps("GET", "~/search/locations", new { controller = "SearchTerms", action = "Locations" });
        }

        [Test]
        public void LowerCaseRoutes()
        {
            RouteTable.Routes.Maps("GET", "~/v1/account/register", new { controller = "Account", action = "Register" });
        }

        [Test]
        public void Shortcuts()
        {
            RouteTable.Routes.Maps("GET", "~/v1/register", new { controller = "Account", action = "Register" });
            RouteTable.Routes.Maps("GET", "~/v1/logon", new { controller = "Account", action = "LogOn" });
            RouteTable.Routes.Maps("GET", "~/v1/about", new { controller = "Home", action = "About" });
        }

        [Test]
        public void Verify()
        {
            RouteTable.Routes.Maps("GET", "~/v1/verify", new { controller = "Account", action = "Verify" });
        }

        [Test]
        public void Welcome()
        {
            RouteTable.Routes.Maps("GET", "~/v1/welcome", new { controller = "Welcome", action = "Index" });
        }

        [Test]
        public void Reset()
        {
            RouteTable.Routes.Maps("GET", "~/v1/reset", new { controller = "Welcome", action = "Reset" });
        }

        [Test]
        public void ElmahRoute()
        {
            RouteTable.Routes.Maps("GET", "~/v1/admin/elmah", new { controller = "Elmah", action = "Index" });
            RouteTable.Routes.Maps("GET", "~/v1/admin/elmah/detail", new { controller = "Elmah", action = "Index" });
            RouteTable.Routes.Maps("GET", "~/v1/admin/elmah/stylesheet", new { controller = "Elmah", action = "Index" });
        }

        [Test]
        public void RedirectRoutes()
        {
            RouteTable.Routes.Maps("GET", "~/Home/Player", new { controller = "Redirect", action = "Redirect" });
            RouteTable.Routes.Maps("GET", "~/register", new { controller = "Redirect", action = "Redirect" });
            RouteTable.Routes.Maps("GET", "~/Account/Register", new { controller = "Redirect", action = "Redirect" });
            RouteTable.Routes.Maps("GET", "~/Roster/2012", new { controller = "Redirect", action = "RedirectNewView", season = 2012, turn = (int?)null });
            RouteTable.Routes.Maps("GET", "~/Roster/2012/12", new { controller = "Redirect", action = "RedirectNewView", season = 2012, turn = (int?)12 });
        }

        [Test]
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
            RouteTable.Routes.Maps("GET", "~/phpmyadmin", new { controller = "Hacker", action = "Index" });
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

        private static void RegisterArea<T>(RouteCollection routes, object state) where T : AreaRegistration
        {
            var registration = (AreaRegistration)Activator.CreateInstance(typeof(T));
            var context = new AreaRegistrationContext(registration.AreaName, routes, state);
            var typeNamespace = registration.GetType().Namespace;
            if (typeNamespace != null)
            {
                context.Namespaces.Add(typeNamespace + ".*");
            }

            registration.RegisterArea(context);
        }
    }
}