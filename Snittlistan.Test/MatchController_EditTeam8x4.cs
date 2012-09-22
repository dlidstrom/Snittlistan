namespace Snittlistan.Test
{
    using System;
    using System.Web;

    using Snittlistan.Web.Controllers;
    using Snittlistan.Web.Infrastructure.AutoMapper;
    using Snittlistan.Web.Models;
    using Snittlistan.Web.ViewModels.Match;

    using Xunit;

    public class MatchController_EditTeam8x4 : DbTest
    {
        [Fact]
        public void CanEditTeam()
        {
            // Arrange
            Match8x4 originalMatch = new Match8x4("Place", DateTime.Now, 1, new Team8x4("Home", 13), new Team8x4("Away", 6));
            Session.Store(originalMatch);
            Session.SaveChanges();

            // Act
            var controller = new MatchController(Session);
            var result = controller.EditTeam8x4(new EditTeam8x4ViewModel
            {
                Id = originalMatch.Id,
                IsHomeTeam = false,
                Team = DbSeed.Create8x4Match().AwayTeam.MapTo<Team8x4ViewModel>()
            });
            Session.SaveChanges();

            // Assert
            result.AssertActionRedirect().ToAction("Details8x4").WithParameter("id", originalMatch.Id);
            var match = Session.Load<Match8x4>(originalMatch.Id);
            TestData.VerifyTeam(match.AwayTeam);
        }

        [Fact]
        public void CannotEditNonExistingMatch()
        {
            var controller = new MatchController(Session);
            try
            {
                controller.EditTeam8x4(1, true);
                Assert.False(true, "Should throw");
            }
            catch (HttpException ex)
            {
                Assert.Equal(404, ex.GetHttpCode());
            }
        }

        [Fact]
        public void CannotPostNonExistingMatch()
        {
            var controller = new MatchController(Session);
            try
            {
                controller.EditTeam8x4(new EditTeam8x4ViewModel { Id = 1 });
                Assert.False(true, "Should throw");
            }
            catch (HttpException ex)
            {
                Assert.Equal(404, ex.GetHttpCode());
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
            var result = controller.EditTeam8x4(id: match.Id, isHomeTeam: true);

            // Assert
            result.AssertViewRendered().ForView(string.Empty);
        }
    }
}