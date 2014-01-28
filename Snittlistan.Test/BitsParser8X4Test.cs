using System;
using Snittlistan.Test.Properties;
using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Areas.V2.ReadModels;
using Xunit;

namespace Snittlistan.Test
{
    public class BitsParser8X4Test
    {
        private readonly Player[] playersTeamA;
        private readonly Player[] playersTeamF;

        public BitsParser8X4Test()
        {
            playersTeamA = new[]
            {
                new Player("Mikael Axelsson", "e@d.com", Player.Status.Active) { Id = "player-1" },
                new Player("Christer Liedholm", "e@d.com", Player.Status.Active) { Id = "player-2" },
                new Player("Lars Öberg", "e@d.com", Player.Status.Active) { Id = "player-3" },
                new Player("Hans Norbeck", "e@d.com", Player.Status.Active) { Id = "player-4" },
                new Player("Mathias Ernest", "e@d.com", Player.Status.Active) { Id = "player-5" },
                new Player("Torbjörn Jensen", "e@d.com", Player.Status.Active) { Id = "player-6" },
                new Player("Alf Kindblom", "e@d.com", Player.Status.Active) { Id = "player-7" },
                new Player("Peter Sjöberg", "e@d.com", Player.Status.Active) { Id = "player-8" },
                new Player("Lennart Axelsson", "e@d.com", Player.Status.Active) { Id = "player-9" },
                new Player("Lars Magnusson", "e@d.com", Player.Status.Active) { Id = "player-10" }
            };
            playersTeamF = new[]
            {
                new Player("Kjell Persson", "e@d.com", Player.Status.Active) { Id = "player-1" },
                new Player("Lars Öberg", "e@d.com", Player.Status.Active) { Id = "player-2" },
                new Player("Tomas Gustavsson", "e@d.com", Player.Status.Active) { Id = "player-3" },
                new Player("Thomas Wallgren", "e@d.com", Player.Status.Active) { Id = "player-4" },
                new Player("Bengt Solvander", "e@d.com", Player.Status.Active) { Id = "player-5" },
                new Player("Lars Magnusson", "e@d.com", Player.Status.Active) { Id = "player-6" },
                new Player("Kjell Johansson", "e@d.com", Player.Status.Active) { Id = "player-7" },
                new Player("Thomas Gurell", "e@d.com", Player.Status.Active) { Id = "player-8" },
                new Player("Lars Norbeck", "e@d.com", Player.Status.Active) { Id = "player-9" }
            };
        }

        [Fact]
        public void CanFindName()
        {
            // Arrange
            const string Team = "Fredrikshof A";

            // Act
            var result = new BitsParser(playersTeamA).Parse(Resources.Id3050651, Team);

            // Assert
            Assert.Equal(7, result.TeamScore);
        }

        [Fact]
        public void CanParseAlternateHomeTeamName()
        {
            // Arrange
            const string Team = "Fredrikshof A";

            // Act
            var result = new BitsParser(playersTeamA).Parse(Resources.Id3048746, Team);

            // Assert
            Assert.Equal(11, result.TeamScore);
        }

        [Fact(Skip = "Not implemented yet")]
        public void CanParseAlternateHomeTeamNameWithIf()
        {
            // Arrange
            const string Team = "Fredrikshof IF A";

            // Act
            var result = new BitsParser(playersTeamA).Parse(Resources.Id3048746, Team);

            // Assert
            Assert.Equal(11, result.TeamScore);
            Assert.False(true, "Update test");
        }

        [Fact]
        public void CanParseAlternateAwayTeamName()
        {
            // Arrange
            const string Team = "Fredrikshof A";

            // Act
            var result = new BitsParser(playersTeamA).Parse(Resources.Id3048747, Team);

            // Assert
            Assert.Equal(10, result.TeamScore);
        }

        [Fact]
        public void CanParseAlternateAwayTeamNameWithIf()
        {
            // Arrange
            const string Team = "Fredrikshof F";

            // Act
            var result = new BitsParser(playersTeamF).Parse(Resources.Id3048477, Team);

            // Assert
            Assert.Equal(14, result.TeamScore);
        }

        [Fact]
        public void CanParseHomeTeam()
        {
            // Arrange
            const string Team = "Fredrikshof IF";

            // Act
            var result = new BitsParser(playersTeamA).Parse(Resources.Id3048746, Team);
            Assert.Equal(11, result.TeamScore);
            Assert.Equal(9, result.OpponentScore);
            var series = result.Series;

            // Assert
            Assert.Equal(4, series.Length);
            var serie1 = series[0];
            Assert.Equal(4, serie1.Tables.Count);
            VerifyTable(serie1.Tables[0], Tuple.Create(1, "player-1", 202, "player-2", 219));
            VerifyTable(serie1.Tables[1], Tuple.Create(0, "player-3", 203, "player-4", 169));
            VerifyTable(serie1.Tables[2], Tuple.Create(1, "player-5", 206, "player-6", 195));
            VerifyTable(serie1.Tables[3], Tuple.Create(0, "player-7", 234, "player-8", 165));

            var serie2 = series[1];
            Assert.Equal(4, serie2.Tables.Count);
            VerifyTable(serie2.Tables[0], Tuple.Create(1, "player-5", 205, "player-6", 212));
            VerifyTable(serie2.Tables[1], Tuple.Create(0, "player-7", 192, "player-8", 192));
            VerifyTable(serie2.Tables[2], Tuple.Create(1, "player-1", 212, "player-2", 237));
            VerifyTable(serie2.Tables[3], Tuple.Create(1, "player-3", 202, "player-4", 199));

            var serie3 = series[2];
            Assert.Equal(4, serie3.Tables.Count);
            VerifyTable(serie3.Tables[0], Tuple.Create(0, "player-7", 206, "player-8", 204));
            VerifyTable(serie3.Tables[1], Tuple.Create(1, "player-5", 215, "player-6", 211));
            VerifyTable(serie3.Tables[2], Tuple.Create(0, "player-3", 184, "player-4", 172));
            VerifyTable(serie3.Tables[3], Tuple.Create(0, "player-1", 175, "player-2", 188));

            var serie4 = series[3];
            Assert.Equal(4, serie4.Tables.Count);
            VerifyTable(serie4.Tables[0], Tuple.Create(0, "player-3", 213, "player-9", 173));
            VerifyTable(serie4.Tables[1], Tuple.Create(1, "player-1", 188, "player-2", 213));
            VerifyTable(serie4.Tables[2], Tuple.Create(1, "player-7", 194, "player-8", 255));
            VerifyTable(serie4.Tables[3], Tuple.Create(1, "player-5", 226, "player-6", 210));
        }

        [Fact]
        public void CanParseAwayTeam()
        {
            // Arrange
            const string Team = "Fredrikshof IF";

            // Act
            var result = new BitsParser(playersTeamA).Parse(Resources.Id3048747, Team);
            Assert.Equal(10, result.TeamScore);
            Assert.Equal(9, result.OpponentScore);
            var series = result.Series;

            // Assert
            Assert.Equal(4, series.Length);
            var serie1 = series[0];
            Assert.Equal(4, serie1.Tables.Count);
            VerifyTable(serie1.Tables[0], Tuple.Create(1, "player-3", 202, "player-10", 204));
            VerifyTable(serie1.Tables[1], Tuple.Create(0, "player-1", 196, "player-2", 234));
            VerifyTable(serie1.Tables[2], Tuple.Create(1, "player-7", 205, "player-8", 247));
            VerifyTable(serie1.Tables[3], Tuple.Create(1, "player-5", 227, "player-6", 212));

            var serie2 = series[1];
            Assert.Equal(4, serie2.Tables.Count);
            VerifyTable(serie2.Tables[0], Tuple.Create(0, "player-5", 182, "player-6", 213));
            VerifyTable(serie2.Tables[1], Tuple.Create(0, "player-7", 226, "player-8", 211));
            VerifyTable(serie2.Tables[2], Tuple.Create(1, "player-1", 218, "player-2", 269));
            VerifyTable(serie2.Tables[3], Tuple.Create(0, "player-3", 232, "player-10", 162));

            var serie3 = series[2];
            Assert.Equal(4, serie3.Tables.Count);
            VerifyTable(serie3.Tables[0], Tuple.Create(0, "player-1", 200, "player-2", 212));
            VerifyTable(serie3.Tables[1], Tuple.Create(0, "player-3", 202, "player-10", 201));
            VerifyTable(serie3.Tables[2], Tuple.Create(0, "player-5", 202, "player-6", 187));
            VerifyTable(serie3.Tables[3], Tuple.Create(1, "player-7", 232, "player-8", 201));

            var serie4 = series[3];
            Assert.Equal(4, serie4.Tables.Count);
            VerifyTable(serie4.Tables[0], Tuple.Create(1, "player-7", 217, "player-8", 203));
            VerifyTable(serie4.Tables[1], Tuple.Create(1, "player-5", 178, "player-6", 267));
            VerifyTable(serie4.Tables[2], Tuple.Create(0, "player-3", 180, "player-10", 229));
            VerifyTable(serie4.Tables[3], Tuple.Create(0, "player-1", 183, "player-2", 175));
        }

        private static void VerifyTable(ResultSeriesReadModel.Table table, Tuple<int, string, int, string, int> expected)
        {
            Assert.Equal(expected.Item1, table.Score);
            Assert.NotNull(table.Game1);
            Assert.NotNull(table.Game2);
            Assert.Equal(expected.Item2, table.Game1.Player);
            Assert.Equal(expected.Item3, table.Game1.Pins);
            Assert.Equal(expected.Item4, table.Game2.Player);
            Assert.Equal(expected.Item5, table.Game2.Pins);
        }
    }
}