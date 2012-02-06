namespace Snittlistan.Test
{
    using System;
    using System.Web;
    using MvcContrib.TestHelper;
    using Snittlistan.Controllers;
    using Snittlistan.Models;
    using Snittlistan.ViewModels.Match;
    using Xunit;

    public class MatchController_EditDetails4x4 : DbTest
    {
        [Fact]
        public void CanEditDetails()
        {
            // Arrange
            var then = DateTime.Now.AddDays(-1);
            var originalMatch = new Match4x4("Place", then, new Team4x4("Home", 13), new Team4x4("Away", 6));
            Session.Store(originalMatch);
            Session.SaveChanges();
            WaitForNonStaleResults<Match4x4>();

            // Act
            var controller = new MatchController(Session);
            var now = DateTime.Now;
            var result = controller.EditDetails4x4(new Match4x4ViewModel.MatchDetails
            {
                Id = originalMatch.Id,
                Location = "NewPlace",
                Date = now
            });

            // Assert
            result.AssertActionRedirect().ToAction("Details4x4").WithParameter("id", originalMatch.Id);
            var match = Session.Load<Match4x4>(originalMatch.Id);
            match.Location.ShouldBe("NewPlace");
            match.Date.ShouldBe(now);
        }

        [Fact]
        public void CannotEditNonExistingMatch()
        {
            var controller = new MatchController(Session);
            try
            {
                controller.EditDetails4x4(1);
                Assert.False(true, "Should throw");
            }
            catch (HttpException ex)
            {
                ex.GetHttpCode().ShouldBe(404);
            }
        }

        [Fact]
        public void CannotPostNonExistingMatch()
        {
            var controller = new MatchController(Session);
            try
            {
                controller.EditDetails4x4(new Match4x4ViewModel.MatchDetails { Id = 1 });
                Assert.False(true, "Should throw");
            }
            catch (HttpException ex)
            {
                ex.GetHttpCode().ShouldBe(404);
            }
        }

        [Fact]
        public void CorrectView()
        {
            // Arrange
            var match = DbSeed.Create4x4Match();
            Session.Store(match);

            // Act
            var controller = new MatchController(Session);
            var result = controller.EditDetails4x4(match.Id);

            // Assert
            result.AssertViewRendered().ForView(string.Empty);
        }

        [Fact]
        public void WhenErrorReturnView()
        {
            // Arrange
            var controller = new MatchController(Session);
            controller.ModelState.AddModelError("key", "error");

            // Act
            var result = controller.EditDetails4x4(null);

            // Assert
            result.AssertViewRendered().ForView(string.Empty);
        }
    }
}