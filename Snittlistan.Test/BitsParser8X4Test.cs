#nullable enable

namespace Snittlistan.Test
{
    using System;
    using System.Threading.Tasks;
    using NUnit.Framework;
    using Web.Areas.V2.Domain;
    using Web.Areas.V2.ReadModels;

    [TestFixture]
    public class BitsParser8X4Test
    {
        private readonly Player[] playersTeamA;
        private readonly Player[] playersTeamF;

        public BitsParser8X4Test()
        {
            playersTeamA = new[]
            {
                new Player("Mikael Axelsson", "e@d.com", Player.Status.Active, 0, null, new string[0]) { Id = "player-1" },
                new Player("Christer Liedholm", "e@d.com", Player.Status.Active, 0, null, new string[0]) { Id = "player-2" },
                new Player("Lars Öberg", "e@d.com", Player.Status.Active, 0, null, new string[0]) { Id = "player-3" },
                new Player("Hans Norbeck", "e@d.com", Player.Status.Active, 0, null, new string[0]) { Id = "player-4" },
                new Player("Mathias Ernest", "e@d.com", Player.Status.Active, 0, null, new string[0]) { Id = "player-5" },
                new Player("Torbjörn Jensen", "e@d.com", Player.Status.Active, 0, null, new string[0]) { Id = "player-6" },
                new Player("Alf Kindblom", "e@d.com", Player.Status.Active, 0, null, new string[0]) { Id = "player-7" },
                new Player("Peter Sjöberg", "e@d.com", Player.Status.Active, 0, null, new string[0]) { Id = "player-8" },
                new Player("Lennart Axelsson", "e@d.com", Player.Status.Active, 0, null, new string[0]) { Id = "player-9" },
                new Player("Lars Magnusson", "e@d.com", Player.Status.Active, 0, null, new string[0]) { Id = "player-10" }
            };
            playersTeamF = new[]
            {
                new Player("Kjell Persson", "e@d.com", Player.Status.Active, 0, null, new string[0]) { Id = "player-1" },
                new Player("Lars Öberg", "e@d.com", Player.Status.Active, 0, null, new string[0]) { Id = "player-2" },
                new Player("Tomas Wikbro", "e@d.com", Player.Status.Active, 0, null, new string[0]) { Id = "player-3" },
                new Player("Thomas Wallgren", "e@d.com", Player.Status.Active, 0, null, new string[0]) { Id = "player-4" },
                new Player("Bengt Solvander", "e@d.com", Player.Status.Active, 0, null, new string[0]) { Id = "player-5" },
                new Player("Lars Magnusson", "e@d.com", Player.Status.Active, 0, null, new string[0]) { Id = "player-6" },
                new Player("Kjell Johansson", "e@d.com", Player.Status.Active, 0, null, new string[0]) { Id = "player-7" },
                new Player("Thomas Gurell", "e@d.com", Player.Status.Active, 0, null, new string[0]) { Id = "player-8" },
                new Player("Lars Norbeck", "e@d.com", Player.Status.Active, 0, null, new string[0]) { Id = "player-9" }
            };
        }

        [Test]
        public async Task CanFindName()
        {
            // Arrange
            const int ClubId = 1660;

            // Act
            ParseResult result = new BitsParser(playersTeamA).Parse(await BitsGateway.GetBitsMatchResult(3050651), ClubId);

            // Assert
            Assert.That(result.TeamScore, Is.EqualTo(7));
        }

        [Test]
        public async Task CanParseAlternateHomeTeamName()
        {
            // Arrange
            const int ClubId = 1660;

            // Act
            ParseResult result = new BitsParser(playersTeamA).Parse(await BitsGateway.GetBitsMatchResult(3048746), ClubId);

            // Assert
            Assert.That(result.TeamScore, Is.EqualTo(11));
        }

        [Test]
        public async Task CanParseAlternateAwayTeamName()
        {
            // Arrange
            const int ClubId = 1660;

            // Act
            ParseResult result = new BitsParser(playersTeamA).Parse(await BitsGateway.GetBitsMatchResult(3048747), ClubId);

            // Assert
            Assert.That(result.TeamScore, Is.EqualTo(10));
        }

        [Test]
        public async Task CanParseAlternateAwayTeamNameWithIf()
        {
            // Arrange
            const int ClubId = 1660;

            // Act
            ParseResult result = new BitsParser(playersTeamF).Parse(await BitsGateway.GetBitsMatchResult(3048477), ClubId);

            // Assert
            Assert.That(result.TeamScore, Is.EqualTo(14));
        }

        [Test]
        public async Task CanParseHomeTeam()
        {
            // Arrange
            const int ClubId = 1660;

            // Act
            ParseResult result = new BitsParser(playersTeamA).Parse(await BitsGateway.GetBitsMatchResult(3048746), ClubId);
            Assert.That(result.TeamScore, Is.EqualTo(11));
            Assert.That(result.OpponentScore, Is.EqualTo(9));
            ResultSeriesReadModel.Serie[] series = result.Series;

            // Assert
            Assert.That(series.Length, Is.EqualTo(4));
            ResultSeriesReadModel.Serie serie1 = series[0];
            Assert.That(serie1.Tables.Count, Is.EqualTo(4));
            VerifyTable(serie1.Tables[0], Tuple.Create(1, "player-1", 202, "player-2", 219));
            VerifyTable(serie1.Tables[1], Tuple.Create(0, "player-3", 203, "player-4", 169));
            VerifyTable(serie1.Tables[2], Tuple.Create(1, "player-5", 206, "player-6", 195));
            VerifyTable(serie1.Tables[3], Tuple.Create(0, "player-7", 234, "player-8", 165));

            ResultSeriesReadModel.Serie serie2 = series[1];
            Assert.That(serie2.Tables.Count, Is.EqualTo(4));
            VerifyTable(serie2.Tables[0], Tuple.Create(1, "player-5", 205, "player-6", 212));
            VerifyTable(serie2.Tables[1], Tuple.Create(0, "player-7", 192, "player-8", 192));
            VerifyTable(serie2.Tables[2], Tuple.Create(1, "player-1", 212, "player-2", 237));
            VerifyTable(serie2.Tables[3], Tuple.Create(1, "player-3", 202, "player-4", 199));

            ResultSeriesReadModel.Serie serie3 = series[2];
            Assert.That(serie3.Tables.Count, Is.EqualTo(4));
            VerifyTable(serie3.Tables[0], Tuple.Create(0, "player-7", 206, "player-8", 204));
            VerifyTable(serie3.Tables[1], Tuple.Create(1, "player-5", 215, "player-6", 211));
            VerifyTable(serie3.Tables[2], Tuple.Create(0, "player-3", 184, "player-4", 172));
            VerifyTable(serie3.Tables[3], Tuple.Create(0, "player-1", 175, "player-2", 188));

            ResultSeriesReadModel.Serie serie4 = series[3];
            Assert.That(serie4.Tables.Count, Is.EqualTo(4));
            VerifyTable(serie4.Tables[0], Tuple.Create(0, "player-3", 213, "player-9", 173));
            VerifyTable(serie4.Tables[1], Tuple.Create(1, "player-1", 188, "player-2", 213));
            VerifyTable(serie4.Tables[2], Tuple.Create(1, "player-7", 194, "player-8", 255));
            VerifyTable(serie4.Tables[3], Tuple.Create(1, "player-5", 226, "player-6", 210));
        }

        [Test]
        public async Task CanParseAwayTeam()
        {
            // Arrange
            const int ClubId = 1660;

            // Act
            ParseResult result = new BitsParser(playersTeamA).Parse(await BitsGateway.GetBitsMatchResult(3048747), ClubId);
            Assert.That(result.TeamScore, Is.EqualTo(10));
            Assert.That(result.OpponentScore, Is.EqualTo(9));
            ResultSeriesReadModel.Serie[] series = result.Series;

            // Assert
            Assert.That(series.Length, Is.EqualTo(4));
            ResultSeriesReadModel.Serie serie1 = series[0];
            Assert.That(serie1.Tables.Count, Is.EqualTo(4));
            VerifyTable(serie1.Tables[0], Tuple.Create(1, "player-3", 202, "player-10", 204));
            VerifyTable(serie1.Tables[1], Tuple.Create(0, "player-1", 196, "player-2", 234));
            VerifyTable(serie1.Tables[2], Tuple.Create(1, "player-7", 205, "player-8", 247));
            VerifyTable(serie1.Tables[3], Tuple.Create(1, "player-5", 227, "player-6", 212));

            ResultSeriesReadModel.Serie serie2 = series[1];
            Assert.That(serie2.Tables.Count, Is.EqualTo(4));
            VerifyTable(serie2.Tables[0], Tuple.Create(0, "player-5", 182, "player-6", 213));
            VerifyTable(serie2.Tables[1], Tuple.Create(0, "player-7", 226, "player-8", 211));
            VerifyTable(serie2.Tables[2], Tuple.Create(1, "player-1", 218, "player-2", 269));
            VerifyTable(serie2.Tables[3], Tuple.Create(0, "player-3", 232, "player-10", 162));

            ResultSeriesReadModel.Serie serie3 = series[2];
            Assert.That(serie3.Tables.Count, Is.EqualTo(4));
            VerifyTable(serie3.Tables[0], Tuple.Create(0, "player-1", 200, "player-2", 212));
            VerifyTable(serie3.Tables[1], Tuple.Create(0, "player-3", 202, "player-10", 201));
            VerifyTable(serie3.Tables[2], Tuple.Create(0, "player-5", 202, "player-6", 187));
            VerifyTable(serie3.Tables[3], Tuple.Create(1, "player-7", 232, "player-8", 201));

            ResultSeriesReadModel.Serie serie4 = series[3];
            Assert.That(serie4.Tables.Count, Is.EqualTo(4));
            VerifyTable(serie4.Tables[0], Tuple.Create(1, "player-7", 217, "player-8", 203));
            VerifyTable(serie4.Tables[1], Tuple.Create(1, "player-5", 178, "player-6", 267));
            VerifyTable(serie4.Tables[2], Tuple.Create(0, "player-3", 180, "player-10", 229));
            VerifyTable(serie4.Tables[3], Tuple.Create(0, "player-1", 183, "player-2", 175));
        }

        [Test]
        public async Task CanParseThreeSeries()
        {
            // Arrange
            const int ClubId = 1660;
            Player[] players = new[]
            {
                new Player("Daniel Solvander", "e@d.com", Player.Status.Active, 0, null, new string[0]) { Id = "player-1" },
                new Player("Daniel Lidström", "e@d.com", Player.Status.Active, 0, null, new string[0]) { Id = "player-2" },
                new Player("Thomas Gurell", "e@d.com", Player.Status.Active, 0, null, new string[0]) { Id = "player-3" },
                new Player("Lennart Axelsson", "e@d.com", Player.Status.Active, 0, null, new string[0]) { Id = "player-4" },
                new Player("Håkan Gustavsson", "e@d.com", Player.Status.Active, 0, null, new string[0]) { Id = "player-5" },
                new Player("Kjell Johansson", "e@d.com", Player.Status.Active, 0, null, new string[0]) { Id = "player-6" },
                new Player("Bengt Solvander", "e@d.com", Player.Status.Active, 0, null, new string[0]) { Id = "player-7" },
                new Player("Stefan Traav", "e@d.com", Player.Status.Active, 0, null, new string[0]) { Id = "player-8" },
                new Player("Matz Classon", "e@d.com", Player.Status.Active, 0, null, new string[0]) { Id = "player-9" }
            };

            // Act
            ParseResult result = new BitsParser(players).Parse(await BitsGateway.GetBitsMatchResult(3067035), ClubId);
            Assert.That(result.TeamScore, Is.EqualTo(10));
            Assert.That(result.OpponentScore, Is.EqualTo(5));
            Assert.That(result.Turn, Is.EqualTo(24));
            ResultSeriesReadModel.Serie[] series = result.Series;

            // Assert
            Assert.That(series.Length, Is.EqualTo(3));
            ResultSeriesReadModel.Serie serie1 = series[0];
            Assert.That(serie1.Tables.Count, Is.EqualTo(4));
            VerifyTable(serie1.Tables[0], Tuple.Create(1, "player-1", 200, "player-2", 199));
            VerifyTable(serie1.Tables[1], Tuple.Create(1, "player-3", 160, "player-4", 257));
            VerifyTable(serie1.Tables[2], Tuple.Create(1, "player-5", 214, "player-6", 195));
            VerifyTable(serie1.Tables[3], Tuple.Create(1, "player-7", 175, "player-8", 241));

            ResultSeriesReadModel.Serie serie2 = series[1];
            Assert.That(serie1.Tables.Count, Is.EqualTo(4));
            VerifyTable(serie2.Tables[0], Tuple.Create(1, "player-7", 165, "player-8", 217));
            VerifyTable(serie2.Tables[1], Tuple.Create(0, "player-5", 180, "player-6", 158));
            VerifyTable(serie2.Tables[2], Tuple.Create(0, "player-3", 176, "player-4", 197));
            VerifyTable(serie2.Tables[3], Tuple.Create(1, "player-1", 145, "player-2", 161));

            ResultSeriesReadModel.Serie serie3 = series[2];
            Assert.That(serie1.Tables.Count, Is.EqualTo(4));
            VerifyTable(serie3.Tables[0], Tuple.Create(0, "player-3", 180, "player-4", 222));
            VerifyTable(serie3.Tables[1], Tuple.Create(1, "player-1", 159, "player-2", 277));
            VerifyTable(serie3.Tables[2], Tuple.Create(1, "player-7", 166, "player-8", 234));
            VerifyTable(serie3.Tables[3], Tuple.Create(0, "player-5", 143, "player-6", 171));
        }

        private static void VerifyTable(ResultSeriesReadModel.Table table, Tuple<int, string, int, string, int> expected)
        {
            Assert.That(table.Score, Is.EqualTo(expected.Item1));
            Assert.NotNull(table.Game1);
            Assert.NotNull(table.Game2);
            Assert.That(table.Game1.Player, Is.EqualTo(expected.Item2));
            Assert.That(table.Game1.Pins, Is.EqualTo(expected.Item3));
            Assert.That(table.Game2.Player, Is.EqualTo(expected.Item4));
            Assert.That(table.Game2.Pins, Is.EqualTo(expected.Item5));
        }
    }
}
