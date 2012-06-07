namespace Snittlistan.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Controllers;
    using Infrastructure.Indexes;
    using Models;
    using MvcContrib.TestHelper;
    using ViewModels;
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
            Session.Query<Pins_Last20.Result, Pins_Last20>()
                .Customize(c => c.WaitForNonStaleResultsAsOfNow())
                .ToList();
            var controller = new HomeController(Session);

            // Act
            var result = controller.Player("Tomas Gustavsson").Model as PlayerMatchesViewModel;

            // Assert
            result.ShouldNotBeNull("Expected PlayerMatchesViewModel");
            result.AverageStrikes.ShouldBe(4.5);
            result.AverageMisses.ShouldBe(2.5);
            result.Last20.GamesWithStats.ShouldBe(2);
            result.Last20.Max.ShouldBe(160);
            result.Last20.AverageStrikes.ShouldBe(4.5);
            result.Last20.AverageMisses.ShouldBe(2.5);
            result.Last20.AverageOnePinMisses.ShouldBe(1.5);
            result.Last20.AverageSplits.ShouldBe(2.5);
            result.Last20.Pins.ShouldBe(561.0 / 4);
            result.Last20.Score.ShouldBe(0.25);
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
            Session.Query<Pins_Last20.Result, Pins_Last20>()
                .Customize(x => x.WaitForNonStaleResultsAsOfNow())
                .ToList();
            var controller = new HomeController(Session);

            // Act
            var result = controller.Player("Mikael Axelsson").Model as PlayerMatchesViewModel;

            // Assert
            result.ShouldNotBeNull("Expected PlayerMatchesViewModel");
            result.AverageStrikes.ShouldBe(5.0);
            result.AverageMisses.ShouldBe(2.0);
            result.Last20.GamesWithStats.ShouldBe(1);
            result.Last20.Max.ShouldBe(202);
            result.Last20.AverageStrikes.ShouldBe(5.0);
            result.Last20.AverageMisses.ShouldBe(2.0);
            result.Last20.AverageOnePinMisses.ShouldBe(1.0);
            result.Last20.AverageSplits.ShouldBe(2.0);
            result.Last20.Pins.ShouldBe(845.0 / 4);
            result.Last20.Score.ShouldBe(0.25);
        }
    }
}
