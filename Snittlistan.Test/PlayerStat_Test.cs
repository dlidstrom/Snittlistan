namespace Snittlistan.Test
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using NUnit.Framework;
    using Snittlistan.Web.Areas.V1.Models;
    using Snittlistan.Web.Infrastructure.Indexes;

    [TestFixture]
    public class PlayerStat_Test : DbTest
    {
        [Test]
        public void VerifyIndex8x4()
        {
            // Arrange
            Session.Store(DbSeed.Create8x4Match());
            Session.SaveChanges();

            // Act
            Matches_PlayerStats.Result stats = Session.Query<Matches_PlayerStats.Result, Matches_PlayerStats>()
                .Customize(c => c.WaitForNonStaleResultsAsOfNow())
                .SingleOrDefault(s => s.Player == "Mikael Axelsson");

            // Assert
            Assert.NotNull(stats);
            Debug.Assert(stats != null, "stats != null");
            Assert.That(stats.Series, Is.EqualTo(4.0));
            Assert.That(stats.Score, Is.EqualTo(3.0));
            Assert.That(stats.Pins, Is.EqualTo(845.0));
            Assert.That(stats.BestGame, Is.EqualTo(223));
            Assert.That(stats.Strikes, Is.EqualTo(5.0));
            Assert.That(stats.Misses, Is.EqualTo(2.0));
            Assert.That(stats.OnePinMisses, Is.EqualTo(1.0));
            Assert.That(stats.Splits, Is.EqualTo(2.0));
            Assert.That(stats.AveragePins, Is.EqualTo(211.25));
            Assert.That(stats.AverageStrikes, Is.EqualTo(5.0));
            Assert.That(stats.AverageMisses, Is.EqualTo(2.0));
            Assert.That(stats.AverageOnePinMisses, Is.EqualTo(1.0));
            Assert.That(stats.AverageSplits, Is.EqualTo(2.0));
            Assert.That(stats.CoveredAll, Is.EqualTo(1));
        }

        [Test]
        public void VerifyIndex4x4()
        {
            // Arrange
            Session.Store(DbSeed.Create4x4Match());
            Session.SaveChanges();

            // Act
            Matches_PlayerStats.Result stats = Session.Query<Matches_PlayerStats.Result, Matches_PlayerStats>()
                .Customize(c => c.WaitForNonStaleResultsAsOfNow())
                .SingleOrDefault(s => s.Player == "Lars Norbeck");

            // Assert
            Assert.NotNull(stats);
            Debug.Assert(stats != null, "stats != null");
            Assert.That(stats.Series, Is.EqualTo(4.0));
            Assert.That(stats.Score, Is.EqualTo(4.0));
            Assert.That(stats.Pins, Is.EqualTo(717.0));
            Assert.That(stats.BestGame, Is.EqualTo(231));
            Assert.That(stats.AveragePins, Is.EqualTo(179.25));
        }

        [Test]
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
                                    new Game4x4("Tomas Vikbro", 160, 0),
                                    new Game4x4("Markus Norbeck", 154, 0),
                                    new Game4x4("Lars Norbeck", 169, 1),
                                    new Game4x4("Matz Classon", 140, 0),
                                }),
                            new Serie4x4(new List<Game4x4>
                                {
                                    new Game4x4("Tomas Vikbro", 141, 0),
                                    new Game4x4("Markus Norbeck", 114, 0),
                                    new Game4x4("Lars Norbeck", 163, 1),
                                    new Game4x4("Matz Classon", 127, 0),
                                }),
                            new Serie4x4(new List<Game4x4>
                                {
                                    new Game4x4("Tomas Vikbro", 128, 1),
                                    new Game4x4("Markus Norbeck", 165, 0),
                                    new Game4x4("Lars Norbeck", 231, 1),
                                    new Game4x4("Matz Classon", 165, 0),
                                }),
                            new Serie4x4(new List<Game4x4>
                                {
                                    new Game4x4("Tomas Vikbro", 132, 0) { Strikes = 5, Misses = 3, OnePinMisses = 1, CoveredAll = true, Splits = 2 },
                                    new Game4x4("Markus Norbeck", 165, 0),
                                    new Game4x4("Lars Norbeck", 154, 1),
                                    new Game4x4("Matz Classon", 162, 1),
                                })
                        }),
                awayTeam: new Team4x4("Librex", 14)));

            Session.SaveChanges();

            // Act
            Matches_PlayerStats.Result stats = Session.Query<Matches_PlayerStats.Result, Matches_PlayerStats>()
                .Customize(c => c.WaitForNonStaleResultsAsOfNow())
                .ToList()
                .SingleOrDefault(s => s.Player == "Tomas Vikbro");

            // Assert
            Assert.NotNull(stats);
            Debug.Assert(stats != null, "stats != null");
            Assert.That(stats.Series, Is.EqualTo(8.0));
            Assert.That(stats.Score, Is.EqualTo(2.0));
            Assert.That(stats.Pins, Is.EqualTo(1122.0));
            Assert.That(stats.BestGame, Is.EqualTo(160));
            Assert.That(stats.AveragePins, Is.EqualTo(140.25));
            Assert.That(stats.AverageMisses, Is.EqualTo(3.0));
            Assert.That(stats.AverageStrikes, Is.EqualTo(5.0));
            Assert.That(stats.AverageOnePinMisses, Is.EqualTo(1.0));
            Assert.That(stats.AverageSplits, Is.EqualTo(2.0));
            Assert.That(stats.CoveredAll, Is.EqualTo(1));
        }
    }
}