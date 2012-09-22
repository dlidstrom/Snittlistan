namespace Snittlistan.Test
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using Snittlistan.Web.Infrastructure.Indexes;
    using Snittlistan.Web.Models;

    using Xunit;

    public class PlayerStat_Test : DbTest
    {
        [Fact]
        public void VerifyIndex8x4()
        {
            // Arrange
            Session.Store(DbSeed.Create8x4Match());
            Session.SaveChanges();

            // Act
            var stats = Session.Query<Matches_PlayerStats.Result, Matches_PlayerStats>()
                .Customize(c => c.WaitForNonStaleResultsAsOfNow())
                .SingleOrDefault(s => s.Player == "Mikael Axelsson");

            // Assert
            Assert.NotNull(stats);
            Debug.Assert(stats != null, "stats != null");
            Assert.Equal(4.0, stats.Series);
            Assert.Equal(3.0, stats.Score);
            Assert.Equal(845.0, stats.Pins);
            Assert.Equal(223, stats.BestGame);
            Assert.Equal(5.0, stats.Strikes);
            Assert.Equal(2.0, stats.Misses);
            Assert.Equal(1.0, stats.OnePinMisses);
            Assert.Equal(2.0, stats.Splits);
            Assert.Equal(211.25, stats.AveragePins);
            Assert.Equal(5.0, stats.AverageStrikes);
            Assert.Equal(2.0, stats.AverageMisses);
            Assert.Equal(1.0, stats.AverageOnePinMisses);
            Assert.Equal(2.0, stats.AverageSplits);
            Assert.Equal(1, stats.CoveredAll);
        }

        [Fact]
        public void VerifyIndex4x4()
        {
            // Arrange
            Session.Store(DbSeed.Create4x4Match());
            Session.SaveChanges();

            // Act
            var stats = Session.Query<Matches_PlayerStats.Result, Matches_PlayerStats>()
                .Customize(c => c.WaitForNonStaleResultsAsOfNow())
                .SingleOrDefault(s => s.Player == "Lars Norbeck");

            // Assert
            Assert.NotNull(stats);
            Debug.Assert(stats != null, "stats != null");
            Assert.Equal(4.0, stats.Series);
            Assert.Equal(4.0, stats.Score);
            Assert.Equal(717.0, stats.Pins);
            Assert.Equal(231, stats.BestGame);
            Assert.Equal(179.25, stats.AveragePins);
        }

        [Fact]
        public void OnlyCountSeriesWithStats()
        {
            // Arrange
            Session.Store(DbSeed.Create4x4Match());

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
                                new Game4x4("Tomas Gustavsson", 128, 1),
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

            // Act
            var stats = Session.Query<Matches_PlayerStats.Result, Matches_PlayerStats>()
                .Customize(c => c.WaitForNonStaleResultsAsOfNow())
                .ToList()
                .SingleOrDefault(s => s.Player == "Tomas Gustavsson");

            // Assert
            Assert.NotNull(stats);
            Debug.Assert(stats != null, "stats != null");
            Assert.Equal(8.0, stats.Series);
            Assert.Equal(2.0, stats.Score);
            Assert.Equal(1122.0, stats.Pins);
            Assert.Equal(160, stats.BestGame);
            Assert.Equal(140.25, stats.AveragePins);
            Assert.Equal(3.0, stats.AverageMisses);
            Assert.Equal(5.0, stats.AverageStrikes);
            Assert.Equal(1.0, stats.AverageOnePinMisses);
            Assert.Equal(2.0, stats.AverageSplits);
            Assert.Equal(1, stats.CoveredAll);
        }
    }
}