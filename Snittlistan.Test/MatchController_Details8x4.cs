namespace Snittlistan.Test
{
    using System;
    using System.Diagnostics;
    using System.Web;
    using System.Web.Mvc;

    using Snittlistan.Web.Controllers;
    using Snittlistan.Web.Models;
    using Snittlistan.Web.ViewModels.Match;

    using Xunit;

    public class MatchController_Details8x4 : DbTest
    {
        [Fact]
        public void ShouldViewMatch()
        {
            // Arrange
            Session.Store(new Match8x4("P1", DateTime.Now, 1, new Team8x4("Home1", 4), new Team8x4("Away1", 7)) { Id = 1 });
            Session.Store(new Match8x4("P2", DateTime.Now, 2, new Team8x4("Home2", 5), new Team8x4("Away2", 8)) { Id = 2 });
            Session.Store(new Match8x4("P3", DateTime.Now, 3, new Team8x4("Home3", 6), new Team8x4("Away3", 9)) { Id = 3 });
            var controller = new MatchController(Session);

            // Act
            var view1 = controller.Details8x4(1) as ViewResult;
            Assert.NotNull(view1);
            Debug.Assert(view1 != null, "view1 != null");
            var model1 = view1.Model as Match8x4ViewModel;
            Debug.Assert(model1 != null, "model1 != null");
            Assert.Equal(1, model1.Match.Id);

            var view2 = controller.Details8x4(2) as ViewResult;
            Assert.NotNull(view2);
            var model2 = view2.Model as Match8x4ViewModel;
            Debug.Assert(model2 != null, "model2 != null");
            Assert.Equal(2, model2.Match.Id);

            var view3 = controller.Details8x4(3) as ViewResult;
            Assert.NotNull(view3);
            Debug.Assert(view3 != null, "view3 != null");
            var model3 = view3.Model as Match8x4ViewModel;
            Debug.Assert(model3 != null, "model3 != null");
            Assert.Equal(3, model3.Match.Id);
        }

        [Fact]
        public void CannotViewNonExistingMatch()
        {
            var controller = new MatchController(Session);
            try
            {
                controller.Details8x4(1);
                Assert.False(true, "Should throw");
            }
            catch (HttpException ex)
            {
                Assert.Equal(404, ex.GetHttpCode());
            }
        }
    }
}
