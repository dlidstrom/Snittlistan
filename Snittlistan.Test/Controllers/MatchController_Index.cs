using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;
using Snittlistan.Web.Areas.V1.Controllers;
using Snittlistan.Web.Areas.V1.Models;
using Snittlistan.Web.Infrastructure.Indexes;

namespace Snittlistan.Test.Controllers
{
    [TestFixture]
    public class MatchController_Index : DbTest
    {
        [Test]
        public void ShouldViewIndex()
        {
            // Arrange
            var controller = new MatchController { DocumentSession = Session };

            // Act
            var result = controller.Index();

            // Assert
            result.AssertViewRendered().ForView(string.Empty);
        }

        [Test]
        public void ShouldListMatches()
        {
            // Arrange
            var now = DateTime.Now;
            Session.Store(new Match8x4("P1", now, 1, new Team8x4("Home", 1), new Team8x4("Away", 2)));
            Session.Store(new Match8x4("P2", now.AddDays(1), 2, new Team8x4("Home2", 3), new Team8x4("Away2", 4)));
            Session.Store(new Match4x4("P3", now.AddDays(2), new Team4x4("Home3", 6), new Team4x4("Away3", 14)));
            Session.SaveChanges();

            // Act
            var controller = new MatchController { DocumentSession = Session };
            var result = controller.Index().Model as IEnumerable<Match_ByDate.Result>;

            // Assert
            Assert.NotNull(result);
            Debug.Assert(result != null, "result != null");
            var matches = result.ToArray();
            Assert.That(matches.Length, Is.EqualTo(3));
            Assert.That(matches[0].Location, Is.EqualTo("P3"));
            Assert.NotNull(matches[0].HomeTeamName);
            Assert.That(matches[0].HomeTeamName, Is.EqualTo("Home3"));
            Assert.That(matches[0].HomeTeamScore, Is.EqualTo(6));
            Assert.NotNull(matches[0].AwayTeamName);
            Assert.That(matches[0].AwayTeamName, Is.EqualTo("Away3"));
            Assert.That(matches[0].AwayTeamScore, Is.EqualTo(14));
            Assert.That(matches[0].Type, Is.EqualTo("4x4"));
            Assert.That(matches[1].Location, Is.EqualTo("P2"));
            Assert.NotNull(matches[1].HomeTeamName);
            Assert.That(matches[1].HomeTeamName, Is.EqualTo("Home2"));
            Assert.That(matches[1].HomeTeamScore, Is.EqualTo(3));
            Assert.NotNull(matches[1].AwayTeamName);
            Assert.That(matches[1].AwayTeamName, Is.EqualTo("Away2"));
            Assert.That(matches[1].AwayTeamScore, Is.EqualTo(4));
            Assert.That(matches[1].Type, Is.EqualTo("8x4"));
            Assert.That(matches[2].Location, Is.EqualTo("P1"));
            Assert.NotNull(matches[2].HomeTeamName);
            Assert.That(matches[2].HomeTeamName, Is.EqualTo("Home"));
            Assert.That(matches[2].HomeTeamScore, Is.EqualTo(1));
            Assert.NotNull(matches[2].AwayTeamName);
            Assert.That(matches[2].AwayTeamName, Is.EqualTo("Away"));
            Assert.That(matches[2].AwayTeamScore, Is.EqualTo(2));
            Assert.That(matches[2].Type, Is.EqualTo("8x4"));
        }
    }
}