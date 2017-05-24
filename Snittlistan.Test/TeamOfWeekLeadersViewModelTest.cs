using System.Linq;
using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Areas.V2.ReadModels;
using Snittlistan.Web.Areas.V2.ViewModels;
using Xunit;

namespace Snittlistan.Test
{
    public class TeamOfWeekLeadersViewModelTest
    {
        private readonly TeamOfWeekViewModel viewModel;

        public TeamOfWeekLeadersViewModelTest()
        {
            // Arrange
            var player1 = new Player("Daniel", "e@d.com", Player.Status.Active, 0) { Id = "9876" };
            var player2 = new Player("Tomas", "s@d.com", Player.Status.Active, 0) { Id = "8765" };
            var teamOfWeek1 = new TeamOfWeek(1234, 2012, 10, "Team A", "A");
            teamOfWeek1.AddResultForPlayer(player1, 1, 210);
            teamOfWeek1.AddResultForPlayer(player2, 1, 190);
            var teamOfWeek2 = new TeamOfWeek(5432, 2012, 10, "Team B", "B");
            teamOfWeek2.AddResultForPlayer(player1, 0, 220);
            teamOfWeek2.AddResultForPlayer(player2, 1, 180);

            // Act
            viewModel = new TeamOfWeekViewModel(
                2012,
                new[]
                {
                    teamOfWeek1,
                    teamOfWeek2
                });
        }

        [Fact]
        public void GroupsByTurn()
        {
            // Assert
            var weeks = viewModel.Weeks;
            Assert.Equal(1, weeks.Count);
            var week = weeks[0];
            Assert.Equal(10, week.Turn);
            var players = week.Players;
            Assert.Equal(2, players.Count);
            var playerScore1 = players[0];
            Assert.Equal("Daniel", playerScore1.Name);
            Assert.Equal(0, playerScore1.Score);
            Assert.Equal(220, playerScore1.Pins);
            Assert.Equal(1, playerScore1.Series);
            var playerScore2 = players[1];
            Assert.Equal("Tomas", playerScore2.Name);
            Assert.Equal(1, playerScore2.Score);
            Assert.Equal(190, playerScore2.Pins);
            Assert.Equal(1, playerScore2.Series);
            Assert.Equal(410, week.Top8);
        }

        [Fact]
        public void CalculatesLeaders()
        {
            // Assert
            var top = viewModel.Leaders.Top9Total.ToList();
            Assert.Equal(2, top.Count);
            Assert.Equal(1, top[0].Count);
            Assert.Equal(1, top[1].Count);
        }
    }
}