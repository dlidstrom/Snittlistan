namespace Snittlistan.Test
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using Snittlistan.Web.Areas.V1.Controllers;
    using Snittlistan.Web.Areas.V1.Models;
    using Snittlistan.Web.Controllers;
    using Snittlistan.Web.Infrastructure.Indexes;
    using Snittlistan.Web.Models;

    using Xunit;

    public class MatchController_Index : DbTest
    {
        [Fact]
        public void ShouldViewIndex()
        {
            // Arrange
            var controller = new MatchController(Session);

            // Act
            var result = controller.Index();

            // Assert
            result.AssertViewRendered().ForView(string.Empty);
        }

        [Fact]
        public void ShouldListMatches()
        {
            // Arrange
            var now = DateTime.Now;
            Session.Store(new Match8x4("P1", now, 1, new Team8x4("Home", 1), new Team8x4("Away", 2)));
            Session.Store(new Match8x4("P2", now.AddDays(1), 2, new Team8x4("Home2", 3), new Team8x4("Away2", 4)));
            Session.Store(new Match4x4("P3", now.AddDays(2), new Team4x4("Home3", 6), new Team4x4("Away3", 14)));
            Session.SaveChanges();

            // Act
            var controller = new MatchController(Session);
            var result = controller.Index().Model as IEnumerable<Match_ByDate.Result>;

            // Assert
            Assert.NotNull(result);
            Debug.Assert(result != null, "result != null");
            var matches = result.ToArray();
            Assert.Equal(3, matches.Length);
            Assert.Equal("P3", matches[0].Location);
            Assert.NotNull(matches[0].HomeTeamName);
            Assert.Equal("Home3", matches[0].HomeTeamName);
            Assert.Equal(6, matches[0].HomeTeamScore);
            Assert.NotNull(matches[0].AwayTeamName);
            Assert.Equal("Away3", matches[0].AwayTeamName);
            Assert.Equal(14, matches[0].AwayTeamScore);
            Assert.Equal("4x4", matches[0].Type);
            Assert.Equal("P2", matches[1].Location);
            Assert.NotNull(matches[1].HomeTeamName);
            Assert.Equal("Home2", matches[1].HomeTeamName);
            Assert.Equal(3, matches[1].HomeTeamScore);
            Assert.NotNull(matches[1].AwayTeamName);
            Assert.Equal("Away2", matches[1].AwayTeamName);
            Assert.Equal(4, matches[1].AwayTeamScore);
            Assert.Equal("8x4", matches[1].Type);
            Assert.Equal("P1", matches[2].Location);
            Assert.NotNull(matches[2].HomeTeamName);
            Assert.Equal("Home", matches[2].HomeTeamName);
            Assert.Equal(1, matches[2].HomeTeamScore);
            Assert.NotNull(matches[2].AwayTeamName);
            Assert.Equal("Away", matches[2].AwayTeamName);
            Assert.Equal(2, matches[2].AwayTeamScore);
            Assert.Equal("8x4", matches[2].Type);
        }
    }
}