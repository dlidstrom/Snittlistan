namespace Snittlistan.Test
{
    using System;
    using System.Web.Mvc;
    using System.Web.Routing;
    using MvcContrib.TestHelper;
    using Snittlistan.Controllers;
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
        public void ElmahRoute()
        {
            "~/admin/elmah".ShouldMapTo<ElmahController>(c => c.Index(null));
            "~/admin/elmah/detail".ShouldMapTo<ElmahController>(c => c.Index("detail"));
            "~/admin/elmah/stylesheet".ShouldMapTo<ElmahController>(c => c.Index("stylesheet"));
        }

        [Fact]
        public void HackerRoutes()
        {
            "~/awstats/awstats.pl".ShouldMapTo<HackerController>(c => c.Index());
            "~/awstats/awstats.pl?configdir=|echo;echo%20YYYAAZ;uname;id;echo%20YYY;echo|".ShouldMapTo<HackerController>(c => c.Index());
        }

        [Fact]
        public void Redirects()
        {
            "~/dlcoubfux.html".ShouldMapTo<GoogleController>(c => c.Index());
            var result = new GoogleController().Index();
            result.AssertActionRedirect().ToController("Home").ToAction("Index");
        }
    }
}