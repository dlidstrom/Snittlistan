using System;
using System.Web;
using NUnit.Framework;
using Snittlistan.Web.Areas.V1.Controllers;
using Snittlistan.Web.Areas.V1.Models;
using Snittlistan.Web.Areas.V1.ViewModels.Match;

namespace Snittlistan.Test.Controllers
{
    [TestFixture]
    public class MatchController_EditDetails8x4 : DbTest
    {
        [Test]
        public void CanEditDetails()
        {
            // Arrange
            var then = DateTime.Now.AddDays(-1);
            var originalMatch = new Match8x4("Place", then, 1, new Team8x4("Home", 13), new Team8x4("Away", 6));
            Session.Store(originalMatch);
            Session.SaveChanges();

            // Act
            var controller = new MatchController { DocumentSession = Session };
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
            Assert.That(match.Location, Is.EqualTo("NewPlace"));
            Assert.That(match.Date, Is.EqualTo(now));
            Assert.That(match.BitsMatchId, Is.EqualTo(2));
        }

        [Test]
        public void CannotEditNonExistingMatch()
        {
            var controller = new MatchController { DocumentSession = Session };
            try
            {
                controller.EditDetails8x4(1);
                Assert.False(true, "Should throw");
            }
            catch (HttpException ex)
            {
                Assert.That(ex.GetHttpCode(), Is.EqualTo(404));
            }
        }

        [Test]
        public void CannotPostNonExistingMatch()
        {
            var controller = new MatchController { DocumentSession = Session };
            try
            {
                controller.EditDetails8x4(new Match8x4ViewModel.MatchDetails { Id = 1 });
                Assert.False(true, "Should throw");
            }
            catch (HttpException ex)
            {
                Assert.That(ex.GetHttpCode(), Is.EqualTo(404));
            }
        }

        [Test]
        public void CorrectView()
        {
            // Arrange
            var match = DbSeed.Create8x4Match();
            Session.Store(match);

            // Act
            var controller = new MatchController { DocumentSession = Session };
            var result = controller.EditDetails8x4(match.Id);

            // Assert
            result.AssertViewRendered().ForView(string.Empty);
        }

        [Test]
        public void WhenErrorReturnView()
        {
            // Arrange
            var controller = new MatchController { DocumentSession = Session };
            controller.ModelState.AddModelError("key", "error");

            // Act
            var result = controller.EditDetails8x4(null);

            // Assert
            result.AssertViewRendered().ForView(string.Empty);
        }
    }
}