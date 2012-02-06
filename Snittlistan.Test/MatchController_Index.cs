namespace Snittlistan.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MvcContrib.TestHelper;
    using Snittlistan.Controllers;
    using Snittlistan.Infrastructure.Indexes;
    using Snittlistan.Models;
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
            WaitForNonStaleResults<Match8x4>();
            WaitForNonStaleResults<Match4x4>();

            // Act
            var controller = new MatchController(Session);
            var result = controller.Index().Model as IEnumerable<Match_ByDate.Result>;

            // Assert
            result.ShouldNotBeNull("Expected model as IEnumerable<Match_ByDate.Result>");
            var matches = result.ToArray();
            matches.Length.ShouldBe(3);
            matches[0].Location.ShouldBe("P3");
            matches[0].HomeTeamName.ShouldNotBeNull("Missing home team name");
            matches[0].HomeTeamName.ShouldBe("Home3");
            matches[0].HomeTeamScore.ShouldBe(6);
            matches[0].AwayTeamName.ShouldNotBeNull("Missing away team name");
            matches[0].AwayTeamName.ShouldBe("Away3");
            matches[0].AwayTeamScore.ShouldBe(14);
            matches[1].Location.ShouldBe("P2");
            matches[1].HomeTeamName.ShouldNotBeNull("Missing home team name");
            matches[1].HomeTeamName.ShouldBe("Home2");
            matches[1].HomeTeamScore.ShouldBe(3);
            matches[1].AwayTeamName.ShouldNotBeNull("Missing away team name");
            matches[1].AwayTeamName.ShouldBe("Away2");
            matches[1].AwayTeamScore.ShouldBe(4);
            matches[2].Location.ShouldBe("P1");
            matches[2].HomeTeamName.ShouldNotBeNull("Missing home team name");
            matches[2].HomeTeamName.ShouldBe("Home");
            matches[2].HomeTeamScore.ShouldBe(1);
            matches[2].AwayTeamName.ShouldNotBeNull("Missing away team name");
            matches[2].AwayTeamName.ShouldBe("Away");
            matches[2].AwayTeamScore.ShouldBe(2);
        }
    }
}