using System;
using Snittlistan.Test.Properties;
using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Areas.V2.ReadModels;
using Xunit;

namespace Snittlistan.Test
{
    public class BitsParser4X4Test
    {
        private readonly Player[] homePlayers;
        private readonly Player[] awayPlayers;

        public BitsParser4X4Test()
        {
            homePlayers = new[]
            {
                new Player("Markus Norbeck", "e@d.com", Player.Status.Active, 0) { Id = "player-1" },
                new Player("Lars Norbeck", "e@d.com", Player.Status.Active, 0) { Id = "player-2" },
                new Player("Daniel Solvander", "e@d.com", Player.Status.Active, 0) { Id = "player-3" },
                new Player("Matz Classon", "e@d.com", Player.Status.Active, 0) { Id = "player-4" }
            };
            awayPlayers = new[]
            {
                new Player("Tobias Wiklund", "e@d.com", Player.Status.Active, 0) { Id = "player-5" },
                new Player("Wiktor Svensson", "e@d.com", Player.Status.Active, 0) { Id = "player-6" },
                new Player("Moa Nilsson", "e@d.com", Player.Status.Active, 0) { Id = "player-7" },
                new Player("Rebecka Wahlström", "e@d.com", Player.Status.Active, 0) { Id = "player-8" },
                new Player("Hanna Engevall", "e@d.com", Player.Status.Active, 0) { Id = "player-9" }
            };
        }

        [Fact]
        public void CanParseHomeTeam()
        {
            // Arrange
            const string Team = "Fredrikshof B";

            // Act
            var result = new BitsParser(homePlayers).Parse4(Resources.Id3060835, Team);
            Assert.Equal(18, result.TeamScore);
            Assert.Equal(1, result.OpponentScore);
            var series = result.Series;

            // Assert
            Assert.Equal(4, series.Length);
            var serie1 = series[0];
            Assert.Equal(4, serie1.Games.Count);
            VerifyGame(serie1.Games[0], Tuple.Create(0, "player-1", 138));
            VerifyGame(serie1.Games[1], Tuple.Create(1, "player-2", 178));
            VerifyGame(serie1.Games[2], Tuple.Create(1, "player-3", 183));
            VerifyGame(serie1.Games[3], Tuple.Create(1, "player-4", 131));

            var serie2 = series[1];
            Assert.Equal(4, serie2.Games.Count);
            VerifyGame(serie2.Games[0], Tuple.Create(1, "player-3", 152));
            VerifyGame(serie2.Games[1], Tuple.Create(1, "player-4", 189));
            VerifyGame(serie2.Games[2], Tuple.Create(1, "player-1", 205));
            VerifyGame(serie2.Games[3], Tuple.Create(1, "player-2", 136));

            var serie3 = series[2];
            Assert.Equal(4, serie3.Games.Count);
            VerifyGame(serie3.Games[0], Tuple.Create(1, "player-4", 223));
            VerifyGame(serie3.Games[1], Tuple.Create(1, "player-3", 251));
            VerifyGame(serie3.Games[2], Tuple.Create(1, "player-2", 158));
            VerifyGame(serie3.Games[3], Tuple.Create(1, "player-1", 149));

            var serie4 = series[3];
            Assert.Equal(4, serie4.Games.Count);
            VerifyGame(serie4.Games[0], Tuple.Create(1, "player-2", 179));
            VerifyGame(serie4.Games[1], Tuple.Create(1, "player-1", 181));
            VerifyGame(serie4.Games[2], Tuple.Create(1, "player-4", 183));
            VerifyGame(serie4.Games[3], Tuple.Create(0, "player-3", 167));
        }

        [Fact]
        public void CanParseAwayTeam()
        {
            // Arrange
            const string Team = "Trippel XXX";

            // Act
            var result = new BitsParser(awayPlayers).Parse4(Resources.Id3060835, Team);
            Assert.Equal(1, result.TeamScore);
            Assert.Equal(18, result.OpponentScore);
            var series = result.Series;

            // Assert
            Assert.Equal(4, series.Length);
            var serie1 = series[0];
            Assert.Equal(4, serie1.Games.Count);
            VerifyGame(serie1.Games[0], Tuple.Create(1, "player-5", 160));
            VerifyGame(serie1.Games[1], Tuple.Create(0, "player-6", 117));
            VerifyGame(serie1.Games[2], Tuple.Create(0, "player-7", 139));
            VerifyGame(serie1.Games[3], Tuple.Create(0, "player-8", 83));

            var serie2 = series[1];
            Assert.Equal(4, serie2.Games.Count);
            VerifyGame(serie2.Games[0], Tuple.Create(0, "player-8", 122));
            VerifyGame(serie2.Games[1], Tuple.Create(0, "player-7", 156));
            VerifyGame(serie2.Games[2], Tuple.Create(0, "player-6", 101));
            VerifyGame(serie2.Games[3], Tuple.Create(0, "player-5", 133));

            var serie3 = series[2];
            Assert.Equal(4, serie3.Games.Count);
            VerifyGame(serie3.Games[0], Tuple.Create(0, "player-6", 148));
            VerifyGame(serie3.Games[1], Tuple.Create(0, "player-5", 142));
            VerifyGame(serie3.Games[2], Tuple.Create(0, "player-9", 118));
            VerifyGame(serie3.Games[3], Tuple.Create(0, "player-7", 136));

            var serie4 = series[3];
            Assert.Equal(4, serie4.Games.Count);
            VerifyGame(serie4.Games[0], Tuple.Create(0, "player-7", 134));
            VerifyGame(serie4.Games[1], Tuple.Create(0, "player-9", 119));
            VerifyGame(serie4.Games[2], Tuple.Create(0, "player-5", 122));
            VerifyGame(serie4.Games[3], Tuple.Create(0, "player-6", 167));
        }

        private static void VerifyGame(ResultSeries4ReadModel.Game game, Tuple<int, string, int> expected)
        {
            Assert.Equal(game.Score, expected.Item1);
            Assert.Equal(game.Player, expected.Item2);
            Assert.Equal(game.Pins, expected.Item3);
        }
    }
}