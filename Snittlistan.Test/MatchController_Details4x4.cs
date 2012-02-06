namespace Snittlistan.Test
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using MvcContrib.TestHelper;
    using Snittlistan.Controllers;
    using Snittlistan.Models;
    using Snittlistan.ViewModels.Match;
    using Xunit;

    public class MatchController_Details4x4 : DbTest
    {
        [Fact]
        public void ShouldViewMatch()
        {
            // Arrange
            Session.Store(new Match4x4("P1", DateTime.Now, new Team4x4("Home1", 4), new Team4x4("Away1", 7)) { Id = 1 });
            Session.Store(new Match4x4("P2", DateTime.Now, new Team4x4("Home2", 5), new Team4x4("Away2", 8)) { Id = 2 });
            Session.Store(new Match4x4("P3", DateTime.Now, new Team4x4("Home3", 6), new Team4x4("Away3", 9)) { Id = 3 });
            WaitForNonStaleResults<Match4x4>();
            var controller = new MatchController(Session);

            // Act
            var view1 = controller.Details4x4(1) as ViewResult;
            view1.ShouldNotBeNull("Must return ViewResult");
            var model1 = view1.Model as Match4x4ViewModel;
            model1.Match.Id.ShouldBe(1);

            var view2 = controller.Details4x4(2) as ViewResult;
            view2.ShouldNotBeNull("Must return ViewResult");
            var model2 = view2.Model as Match4x4ViewModel;
            model2.Match.Id.ShouldBe(2);

            var view3 = controller.Details4x4(3) as ViewResult;
            view3.ShouldNotBeNull("Must return ViewResult");
            var model3 = view3.Model as Match4x4ViewModel;
            model3.Match.Id.ShouldBe(3);
        }

        [Fact]
        public void CannotViewNonExistingMatch()
        {
            var controller = new MatchController(Session);
            try
            {
                controller.Details4x4(1);
                Assert.False(true, "Should throw");
            }
            catch (HttpException ex)
            {
                ex.GetHttpCode().ShouldBe(404);
            }
        }
    }
}
