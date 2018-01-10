using System;
using System.Linq;
using NUnit.Framework;
using Snittlistan.Web.Areas.V1.Controllers;
using Snittlistan.Web.Areas.V1.Models;
using Snittlistan.Web.Areas.V1.ViewModels.Match;

namespace Snittlistan.Test.Controllers
{
    [TestFixture]
    public class MatchController_Register8x4 : DbTest
    {
        [Test]
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
            Assert.That(match.Location, Is.EqualTo("Somewhere"));
            Assert.That(match.BitsMatchId, Is.EqualTo(1));
            Assert.That(match.Date, Is.EqualTo(now));
            Assert.That(match.HomeTeam.Name, Is.EqualTo("HomeTeam"));
            Assert.That(match.HomeTeam.Score, Is.EqualTo(13));
            Assert.That(match.AwayTeam.Score, Is.EqualTo(6));
        }

        [Test]
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

        [Test]
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