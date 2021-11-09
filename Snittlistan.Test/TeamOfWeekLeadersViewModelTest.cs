#nullable enable

namespace Snittlistan.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;
    using Snittlistan.Web.Areas.V2.Domain;
    using Snittlistan.Web.Areas.V2.ReadModels;
    using Snittlistan.Web.Areas.V2.ViewModels;

    [TestFixture]
    public class TeamOfWeekLeadersViewModelTest
    {
        private TeamOfWeekViewModel? viewModel;

        [SetUp]
        public void SetUp()
        {
            // Arrange
            Player player1 = new("Daniel", "e@d.com", Player.Status.Active, 0, null, new string[0]) { Id = "9876" };
            Player player2 = new("Tomas", "s@d.com", Player.Status.Active, 0, null, new string[0]) { Id = "8765" };
            TeamOfWeek teamOfWeek1 = new(1234, 2012, "roster-1");
            teamOfWeek1.AddResultForPlayer(player1, 1, 210);
            teamOfWeek1.AddResultForPlayer(player2, 1, 190);
            TeamOfWeek teamOfWeek2 = new(5432, 2012, "roster-2");
            teamOfWeek2.AddResultForPlayer(player1, 0, 220);
            teamOfWeek2.AddResultForPlayer(player2, 1, 180);

            // Act
            viewModel = new TeamOfWeekViewModel(
                2012,
                new[]
                {
                    teamOfWeek1,
                    teamOfWeek2
                },
                new Dictionary<string, Roster>
                {
                    { "roster-1", new Roster(2012, 10, 0, "Fredrikshof IF BK A", null, null, null, DateTime.MinValue, false, OilPatternInformation.Empty) },
                    { "roster-2", new Roster(2012, 10, 0, "Fredrikshof IF BK A", null, null, null, DateTime.MinValue, false, OilPatternInformation.Empty) }
                });
        }

        [Test]
        public void GroupsByTurn()
        {
            // Assert
            List<TeamOfWeekViewModel.Week> weeks = viewModel!.Weeks;
            Assert.That(weeks, Has.Count.EqualTo(1));
            TeamOfWeekViewModel.Week week = weeks[0];
            Assert.That(week.Turn, Is.EqualTo(10));
            PlayerScoreViewModel[] players = week.Players;
            Assert.That(players, Has.Length.EqualTo(2));
            PlayerScoreViewModel playerScore1 = players[0];
            Assert.That(playerScore1.Name, Is.EqualTo("Daniel"));
            Assert.That(playerScore1.Score, Is.EqualTo(0));
            Assert.That(playerScore1.Pins, Is.EqualTo(220));
            Assert.That(playerScore1.Series, Is.EqualTo(1));
            PlayerScoreViewModel playerScore2 = players[1];
            Assert.That(playerScore2.Name, Is.EqualTo("Tomas"));
            Assert.That(playerScore2.Score, Is.EqualTo(1));
            Assert.That(playerScore2.Pins, Is.EqualTo(190));
            Assert.That(playerScore2.Series, Is.EqualTo(1));
            Assert.That(week.Top8, Is.EqualTo(410));
        }

        [Test]
        public void CalculatesLeaders()
        {
            // Assert
            List<TeamOfWeekLeadersViewModel.NameCount> top = viewModel!.Leaders.Top9Total.ToList();
            Assert.That(top, Has.Count.EqualTo(2));
            Assert.That(top[0], Has.Count.EqualTo(1));
            Assert.That(top[1], Has.Count.EqualTo(1));
        }
    }
}
