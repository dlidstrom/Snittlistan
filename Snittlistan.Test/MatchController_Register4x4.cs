namespace Snittlistan.Test
{
    using System;
    using System.Linq;

    using Snittlistan.Web.Controllers;
    using Snittlistan.Web.Models;
    using Snittlistan.Web.ViewModels.Match;

    using Xunit;

    public class MatchController_Register4x4 : DbTest
    {
        [Fact]
        public void ViewIsCreate()
        {
            // Arrange
            var controller = new MatchController(Session);

            // Act
            var now = DateTimeOffset.Now;
            controller.Register4x4(new Register4x4MatchViewModel
                {
                    Location = "Somewhere",
                    Date = now,
                    HomeTeam = new Team4x4ViewModel
                        {
                            Name = "HomeTeam",
                            Score = 13,
                            Player1 = new Team4x4ViewModel.Player
                                {
                                    Game1 = new Team4x4ViewModel.Game
                                        {
                                            Player = "Lennart Axelsson",
                                            Pins = 155,
                                            Score = 1
                                        }
                                }
                        },
                    AwayTeam = new Team4x4ViewModel
                        {
                            Name = "AwayTeam",
                            Score = 6
                        }
                });
            Session.SaveChanges();

            // Assert
            var match = Session.Query<Match4x4>().Single();
            Assert.Equal("Somewhere", match.Location);
            Assert.Equal(now, match.Date);
            Assert.Equal("HomeTeam", match.HomeTeam.Name);
            Assert.Equal(13, match.HomeTeam.Score);
            var game = match.HomeTeam.Series.ElementAt(0).Games.ElementAt(0);
            Assert.Equal(155, game.Pins);
            Assert.Equal("Lennart Axelsson", game.Player);
            Assert.Equal(1, game.Score);
            Assert.Equal(6, match.AwayTeam.Score);
        }

        [Fact]
        public void WhenErrorReturnView()
        {
            // Arrange
            var controller = new MatchController(Session);
            controller.ModelState.AddModelError("key", "error");

            // Act
            var result = controller.Register4x4(new Register4x4MatchViewModel());

            // Assert
            result.AssertViewRendered().ForView(string.Empty);
        }
    }
}