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
                new Player("Jennie Bäck", "e@d.com", Player.Status.Active) { Id = "player-1" },
                new Player("Gunvor Magnusson", "e@d.com", Player.Status.Active) { Id = "player-2" },
                new Player("Elsy Lundin", "e@d.com", Player.Status.Active) { Id = "player-3" },
                new Player("Kristina Wikström", "e@d.com", Player.Status.Active) { Id = "player-4" },
                new Player("Anita Enström", "e@d.com", Player.Status.Active) { Id = "player-5" }
            };
            awayPlayers = new[]
            {
                new Player("Stefan Markenfelt", "e@d.com", Player.Status.Active) { Id = "player-1" },
                new Player("Markus Norbeck", "e@d.com", Player.Status.Active) { Id = "player-2" },
                new Player("Daniel Solvander", "e@d.com", Player.Status.Active) { Id = "player-3" },
                new Player("Per-Erik Freij", "e@d.com", Player.Status.Active) { Id = "player-4" }
            };
        }

        [Fact]
        public void CanParseAwayTeam()
        {
            // Arrange
            const string Team = "Fredrikshof B";

            // Act
            var result = new BitsParser(awayPlayers).Parse4(Resources.Id3060803, Team);
            Assert.Equal(8, result.TeamScore);
            Assert.Equal(12, result.OpponentScore);
            var series = result.Series;

            // Assert
            Assert.Equal(4, series.Length);
            var serie1 = series[0];
            Assert.Equal(4, serie1.Games.Count);
            VerifyGame(serie1.Games[0], Tuple.Create(0, "player-1", 157));
            VerifyGame(serie1.Games[1], Tuple.Create(0, "player-2", 140));
            VerifyGame(serie1.Games[2], Tuple.Create(1, "player-3", 167));
            VerifyGame(serie1.Games[3], Tuple.Create(0, "player-4", 137));

            var serie2 = series[1];
            Assert.Equal(4, serie2.Games.Count);
            VerifyGame(serie2.Games[0], Tuple.Create(0, "player-4", 134));
            VerifyGame(serie2.Games[1], Tuple.Create(0, "player-3", 124));
            VerifyGame(serie2.Games[2], Tuple.Create(0, "player-2", 132));
            VerifyGame(serie2.Games[3], Tuple.Create(1, "player-1", 160));

            var serie3 = series[2];
            Assert.Equal(4, serie3.Games.Count);
            VerifyGame(serie3.Games[0], Tuple.Create(1, "player-2", 161));
            VerifyGame(serie3.Games[1], Tuple.Create(1, "player-1", 172));
            VerifyGame(serie3.Games[2], Tuple.Create(0, "player-4", 145));
            VerifyGame(serie3.Games[3], Tuple.Create(0, "player-3", 110));

            var serie4 = series[3];
            Assert.Equal(4, serie4.Games.Count);
            VerifyGame(serie4.Games[0], Tuple.Create(1, "player-3", 148));
            VerifyGame(serie4.Games[1], Tuple.Create(0, "player-4", 158));
            VerifyGame(serie4.Games[2], Tuple.Create(1, "player-1", 164));
            VerifyGame(serie4.Games[3], Tuple.Create(0, "player-2", 144));
        }

        [Fact]
        public void CanParseHomeTeam()
        {
            // Arrange
            const string Team = "BK Ringen";

            // Act
            var result = new BitsParser(homePlayers).Parse4(Resources.Id3060803, Team);
            Assert.Equal(12, result.TeamScore);
            Assert.Equal(8, result.OpponentScore);
            var series = result.Series;

            // Assert
            Assert.Equal(4, series.Length);
            var serie1 = series[0];
            Assert.Equal(4, serie1.Games.Count);
            VerifyGame(serie1.Games[0], Tuple.Create(1, "player-1", 201));
            VerifyGame(serie1.Games[1], Tuple.Create(1, "player-2", 166));
            VerifyGame(serie1.Games[2], Tuple.Create(0, "player-3", 160));
            VerifyGame(serie1.Games[3], Tuple.Create(1, "player-4", 169));

            var serie2 = series[1];
            Assert.Equal(4, serie2.Games.Count);
            VerifyGame(serie2.Games[0], Tuple.Create(1, "player-5", 167));
            VerifyGame(serie2.Games[1], Tuple.Create(1, "player-4", 192));
            VerifyGame(serie2.Games[2], Tuple.Create(1, "player-1", 145));
            VerifyGame(serie2.Games[3], Tuple.Create(0, "player-2", 147));

            var serie3 = series[2];
            Assert.Equal(4, serie3.Games.Count);
            VerifyGame(serie3.Games[0], Tuple.Create(0, "player-4", 138));
            VerifyGame(serie3.Games[1], Tuple.Create(0, "player-5", 162));
            VerifyGame(serie3.Games[2], Tuple.Create(1, "player-2", 147));
            VerifyGame(serie3.Games[3], Tuple.Create(1, "player-3", 128));

            var serie4 = series[3];
            Assert.Equal(4, serie4.Games.Count);
            VerifyGame(serie4.Games[0], Tuple.Create(0, "player-2", 143));
            VerifyGame(serie4.Games[1], Tuple.Create(1, "player-1", 184));
            VerifyGame(serie4.Games[2], Tuple.Create(0, "player-4", 129));
            VerifyGame(serie4.Games[3], Tuple.Create(1, "player-5", 151));
        }

        private static void VerifyGame(ResultSeries4ReadModel.Game game, Tuple<int, string, int> expected)
        {
            Assert.Equal(game.Score, expected.Item1);
            Assert.Equal(game.Player, expected.Item2);
            Assert.Equal(game.Pins, expected.Item3);
        }
    }
}