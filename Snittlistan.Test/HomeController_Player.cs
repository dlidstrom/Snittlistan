namespace Snittlistan.Test
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using Snittlistan.Web.Areas.V1.Controllers;
    using Snittlistan.Web.Areas.V1.Models;
    using Snittlistan.Web.Areas.V1.ViewModels;
    using Snittlistan.Web.Infrastructure.Indexes;

    using Xunit;

    public class HomeController_Player : DbTest
    {
        [Fact]
        public void Handles4x4Correctly()
        {
            // Arrange
            Session.Store(new Match4x4(
                location: "Bowl-O-Rama",
                date: new DateTime(2012, 01, 28),
                homeTeam: new Team4x4(
                    "Fredrikshof C",
                    6,
                    new List<Serie4x4>
                        {
                            new Serie4x4(new List<Game4x4>
                                {
                                    new Game4x4("Tomas Gustavsson", 160, 0),
                                    new Game4x4("Markus Norbeck", 154, 0),
                                    new Game4x4("Lars Norbeck", 169, 1),
                                    new Game4x4("Matz Classon", 140, 0),
                                }),
                            new Serie4x4(new List<Game4x4>
                                {
                                    new Game4x4("Tomas Gustavsson", 141, 0),
                                    new Game4x4("Markus Norbeck", 114, 0),
                                    new Game4x4("Lars Norbeck", 163, 1),
                                    new Game4x4("Matz Classon", 127, 0),
                                }),
                            new Serie4x4(new List<Game4x4>
                                {
                                    new Game4x4("Tomas Gustavsson", 128, 1) { Strikes = 4, Misses = 2, OnePinMisses = 2, CoveredAll = true, Splits = 3 },
                                    new Game4x4("Markus Norbeck", 165, 0),
                                    new Game4x4("Lars Norbeck", 231, 1),
                                    new Game4x4("Matz Classon", 165, 0),
                                }),
                            new Serie4x4(new List<Game4x4>
                                {
                                    new Game4x4("Tomas Gustavsson", 132, 0) { Strikes = 5, Misses = 3, OnePinMisses = 1, CoveredAll = true, Splits = 2 },
                                    new Game4x4("Markus Norbeck", 165, 0),
                                    new Game4x4("Lars Norbeck", 154, 1),
                                    new Game4x4("Matz Classon", 162, 1),
                                })
                        }),
                awayTeam: new Team4x4("Librex", 14)));

            Session.SaveChanges();

            // wait for map/reduce indexing to do its work
            Session.Query<Player_ByMatch.Result, Player_ByMatch>()
                .Customize(c => c.WaitForNonStaleResultsAsOfNow())
                .ToList();
            Session.Query<Matches_PlayerStats.Result, Matches_PlayerStats>()
                .Customize(c => c.WaitForNonStaleResultsAsOfNow())
                .ToList();
            var controller = new HomeController(Session);

            // Act
            var result = controller.Player("Tomas Gustavsson").Model as PlayerMatchesViewModel;

            // Assert
            Assert.NotNull(result);
            Debug.Assert(result != null, "result != null");
            Assert.Equal(4.5, result.Results.AverageStrikes);
            Assert.Equal(2.5, result.Results.AverageMisses);
            Assert.Equal(2, result.Results.GamesWithStats);
            Assert.Equal(160, result.Results.BestGame);
            Assert.Equal(4.5, result.Results.AverageStrikes);
            Assert.Equal(2.5, result.Results.AverageMisses);
            Assert.Equal(1.5, result.Results.AverageOnePinMisses);
            Assert.Equal(2.5, result.Results.AverageSplits);
            Assert.Equal(561.0 / 4, result.Results.AveragePins);
            Assert.Equal(0.25, result.Results.AverageScore);
        }

        [Fact]
        public void Handles8x4Correctly()
        {
            // Arrange
            Session.Store(DbSeed.Create8x4Match());

            Session.SaveChanges();
            Session.Query<Player_ByMatch.Result, Player_ByMatch>()
                .Customize(x => x.WaitForNonStaleResultsAsOfNow())
                .ToList();
            Session.Query<Matches_PlayerStats.Result, Matches_PlayerStats>()
                .Customize(x => x.WaitForNonStaleResultsAsOfNow())
                .ToList();
            var controller = new HomeController(Session);

            // Act
            var result = controller.Player("Mikael Axelsson").Model as PlayerMatchesViewModel;

            // Assert
            Assert.NotNull(result);
            Debug.Assert(result != null, "result != null");
            Assert.Equal(5.0, result.Results.AverageStrikes);
            Assert.Equal(2.0, result.Results.AverageMisses);
            Assert.Equal(1, result.Results.GamesWithStats);
            Assert.Equal(223, result.Results.BestGame);
            Assert.Equal(5.0, result.Results.AverageStrikes);
            Assert.Equal(2.0, result.Results.AverageMisses);
            Assert.Equal(1.0, result.Results.AverageOnePinMisses);
            Assert.Equal(2.0, result.Results.AverageSplits);
            Assert.Equal(845.0 / 4, result.Results.AveragePins);
            Assert.Equal(0.75, result.Results.AverageScore);
        }
    }
}
