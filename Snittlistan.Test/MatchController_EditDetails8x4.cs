namespace Snittlistan.Test
{
    using System;
    using System.Web;
    using Controllers;
    using Models;
    using MvcContrib.TestHelper;
    using ViewModels.Match;
    using Xunit;

    public class MatchController_EditDetails8x4 : DbTest
    {
        [Fact]
        public void CanEditDetails()
        {
            // Arrange
            var then = DateTime.Now.AddDays(-1);
            Match8x4 originalMatch = new Match8x4("Place", then, 1, new Team8x4("Home", 13), new Team8x4("Away", 6));
            Session.Store(originalMatch);
            Session.SaveChanges();

            // Act
            var controller = new MatchController(Session);
            var now = DateTimeOffset.Now;
            var result = controller.EditDetails8x4(new Match8x4ViewModel.MatchDetails
            {
                Id = originalMatch.Id,
                Location = "NewPlace",
                Date = now,
                BitsMatchId = 2
            });

            // Assert
            result.AssertActionRedirect().ToAction("Details8x4").WithParameter("id", originalMatch.Id);
            var match = Session.Load<Match8x4>(originalMatch.Id);
            match.Location.ShouldBe("NewPlace");
            match.Date.ShouldBe(now);
            match.BitsMatchId.ShouldBe(2);
        }

        [Fact]
        public void CannotEditNonExistingMatch()
        {
            var controller = new MatchController(Session);
            try
            {
                controller.EditDetails8x4(1);
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
                controller.EditDetails8x4(new Match8x4ViewModel.MatchDetails { Id = 1 });
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
            var match = DbSeed.Create8x4Match();
            Session.Store(match);

            // Act
            var controller = new MatchController(Session);
            var result = controller.EditDetails8x4(match.Id);

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
            var result = controller.EditDetails8x4(null);

            // Assert
            result.AssertViewRendered().ForView(string.Empty);
        }
    }
}