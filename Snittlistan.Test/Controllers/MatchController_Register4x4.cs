using System;
using System.Linq;
using NUnit.Framework;
using Snittlistan.Web.Areas.V1.Controllers;
using Snittlistan.Web.Areas.V1.Models;
using Snittlistan.Web.Areas.V1.ViewModels.Match;

namespace Snittlistan.Test.Controllers
{
    [TestFixture]
    public class MatchController_Register4x4 : DbTest
    {
        [Test]
        public void ViewIsCreate()
        {
            // Arrange
            var controller = new MatchController { DocumentSession = Session };

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
            Assert.That(match.Location, Is.EqualTo("Somewhere"));
            Assert.That(match.Date, Is.EqualTo(now));
            Assert.That(match.HomeTeam.Name, Is.EqualTo("HomeTeam"));
            Assert.That(match.HomeTeam.Score, Is.EqualTo(13));
            var game = match.HomeTeam.Series.ElementAt(0).Games.ElementAt(0);
            Assert.That(game.Pins, Is.EqualTo(155));
            Assert.That(game.Player, Is.EqualTo("Lennart Axelsson"));
            Assert.That(game.Score, Is.EqualTo(1));
            Assert.That(match.AwayTeam.Score, Is.EqualTo(6));
        }

        [Test]
        public void WhenErrorReturnView()
        {
            // Arrange
            var controller = new MatchController { DocumentSession = Session };
            controller.ModelState.AddModelError("key", "error");

            // Act
            var result = controller.Register4x4(new Register4x4MatchViewModel());

            // Assert
            result.AssertViewRendered().ForView(string.Empty);
        }
    }
}