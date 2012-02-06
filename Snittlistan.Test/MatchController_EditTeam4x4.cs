namespace Snittlistan.Test
{
    using System;
    using System.Web;
    using MvcContrib.TestHelper;
    using Snittlistan.Controllers;
    using Snittlistan.Infrastructure.AutoMapper;
    using Snittlistan.Models;
    using Snittlistan.ViewModels.Match;
    using Xunit;

    public class MatchController_EditTeam4x4 : DbTest
    {
        [Fact]
        public void CanEditTeam()
        {
            // Arrange
            Match4x4 originalMatch = new Match4x4("Place", DateTime.Now, new Team4x4("Home", 13), new Team4x4("Away", 6));
            Session.Store(originalMatch);
            Session.SaveChanges();

            // Act
            var controller = new MatchController(Session);
            var result = controller.EditTeam4x4(new EditTeam4x4ViewModel
            {
                Id = originalMatch.Id,
                IsHomeTeam = false,
                Team = DbSeed.Create4x4Match().HomeTeam.MapTo<Team4x4ViewModel>()
            });
            Session.SaveChanges();

            // Assert
            result.AssertActionRedirect().ToAction("Details4x4").WithParameter("id", originalMatch.Id);
            var match = Session.Load<Match4x4>(originalMatch.Id);
            TestData.VerifyTeam(match.AwayTeam);
        }

        [Fact]
        public void CannotEditNonExistingMatch()
        {
            var controller = new MatchController(Session);
            try
            {
                controller.EditTeam4x4(1, true);
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
                controller.EditTeam4x4(new EditTeam4x4ViewModel { Id = 1 });
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
            var result = controller.EditTeam4x4(id: match.Id, isHomeTeam: true);

            // Assert
            result.AssertViewRendered().ForView(string.Empty);
        }
    }
}