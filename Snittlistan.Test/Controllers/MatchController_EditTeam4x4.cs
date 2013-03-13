namespace Snittlistan.Test
{
    using System;
    using System.Web;

    using Snittlistan.Web.Areas.V1.Controllers;
    using Snittlistan.Web.Areas.V1.Models;
    using Snittlistan.Web.Areas.V1.ViewModels.Match;
    using Snittlistan.Web.Controllers;
    using Snittlistan.Web.Infrastructure.AutoMapper;
    using Snittlistan.Web.Models;

    using Xunit;

    public class MatchController_EditTeam4x4 : DbTest
    {
        [Fact]
        public void CanEditTeam()
        {
            // Arrange
            var originalMatch = new Match4x4("Place", DateTime.Now, new Team4x4("Home", 13), new Team4x4("Away", 6));
            Session.Store(originalMatch);
            Session.SaveChanges();

            // Act
            var controller = new MatchController { DocumentSession = Session };
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
            var controller = new MatchController { DocumentSession = Session };
            try
            {
                controller.EditTeam4x4(1, true);
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
            var controller = new MatchController { DocumentSession = Session };
            try
            {
                controller.EditTeam4x4(new EditTeam4x4ViewModel { Id = 1 });
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
            var match = DbSeed.Create4x4Match();
            Session.Store(match);

            // Act
            var controller = new MatchController { DocumentSession = Session };
            var result = controller.EditTeam4x4(id: match.Id, isHomeTeam: true);

            // Assert
            result.AssertViewRendered().ForView(string.Empty);
        }
    }
}