using System;
using System.Linq;
using Snittlistan.Web.Areas.V1.Controllers;
using Snittlistan.Web.Areas.V1.Models;
using Snittlistan.Web.Areas.V1.ViewModels.Match;
using Xunit;

namespace Snittlistan.Test.Controllers
{
    public class MatchController_Register8x4 : DbTest
    {
        [Fact]
        public void ViewIsCreate()
        {
            // Arrange
            var controller = new MatchController { DocumentSession = Session };

            // Act
            var now = DateTimeOffset.Now;
            controller.Register8x4(new Register8x4MatchViewModel
                {
                    Location = "Somewhere",
                    Date = now,
                    BitsMatchId = 1,
                    HomeTeam = new Team8x4ViewModel
                        {
                            Name = "HomeTeam",
                            Score = 13
                        },
                    AwayTeam = new Team8x4ViewModel
                        {
                            Name = "AwayTeam",
                            Score = 6
                        }
                });
            Session.SaveChanges();

            // Assert
            var match = Session.Query<Match8x4>().Single();
            Assert.Equal("Somewhere", match.Location);
            Assert.Equal(1, match.BitsMatchId);
            Assert.Equal(now, match.Date);
            Assert.Equal("HomeTeam", match.HomeTeam.Name);
            Assert.Equal(13, match.HomeTeam.Score);
            Assert.Equal(6, match.AwayTeam.Score);
        }

        [Fact]
        public void WhenErrorReturnView()
        {
            // Arrange
            var controller = new MatchController { DocumentSession = Session };
            controller.ModelState.AddModelError("key", "error");

            // Act
            var result = controller.Register8x4(new Register8x4MatchViewModel());

            // Assert
            result.AssertViewRendered().ForView(string.Empty);
        }

        [Fact]
        public void CannotRegisterSameBitsIdTwice()
        {
            // Arrange
            var controller = new MatchController { DocumentSession = Session };
            var now = DateTime.Now;
            var vm = new Register8x4MatchViewModel
            {
                Location = "Somewhere",
                Date = now,
                BitsMatchId = 1,
                HomeTeam = new Team8x4ViewModel
                {
                    Name = "HomeTeam",
                    Score = 13
                },
                AwayTeam = new Team8x4ViewModel
                {
                    Name = "AwayTeam",
                    Score = 6
                }
            };
            controller.Register8x4(vm);
            Session.SaveChanges();

            // Act
            var result = controller.Register8x4(vm);

            // Assert
            result.AssertViewRendered().ForView(string.Empty);
        }
    }
}