using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using NUnit.Framework;
using Snittlistan.Web.Areas.V1.Controllers;
using Snittlistan.Web.Areas.V1.Models;
using Snittlistan.Web.Areas.V1.ViewModels;
using Snittlistan.Web.Infrastructure.Indexes;

namespace Snittlistan.Test.Controllers
{
    [TestFixture]
    public class HomeController_Player : DbTest
    {
        [Test]
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
                                    new Game4x4("Tomas Vikbro", 128, 1) { Strikes = 4, Misses = 2, OnePinMisses = 2, CoveredAll = true, Splits = 3 },
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

            // wait for map/reduce indexing to do its work
            Session.Query<Player_ByMatch.Result, Player_ByMatch>()
                .Customize(c => c.WaitForNonStaleResultsAsOfNow())
                .ToList();
            Session.Query<Matches_PlayerStats.Result, Matches_PlayerStats>()
                .Customize(c => c.WaitForNonStaleResultsAsOfNow())
                .ToList();
            var controller = new HomeController { DocumentSession = Session };

            // Act
            var viewResult = controller.Player("Tomas Vikbro") as ViewResult;
            Assert.NotNull(viewResult);
            var result = viewResult.Model as PlayerMatchesViewModel;

            // Assert
            Assert.NotNull(result);
            Debug.Assert(result != null, "result != null");
            Assert.That(result.Results.AverageStrikes, Is.EqualTo(4.5));
            Assert.That(result.Results.AverageMisses, Is.EqualTo(2.5));
            Assert.That(result.Results.GamesWithStats, Is.EqualTo(2));
            Assert.That(result.Results.BestGame, Is.EqualTo(160));
            Assert.That(result.Results.AverageStrikes, Is.EqualTo(4.5));
            Assert.That(result.Results.AverageMisses, Is.EqualTo(2.5));
            Assert.That(result.Results.AverageOnePinMisses, Is.EqualTo(1.5));
            Assert.That(result.Results.AverageSplits, Is.EqualTo(2.5));
            Assert.That(result.Results.AveragePins, Is.EqualTo(561.0 / 4));
            Assert.That(result.Results.AverageScore, Is.EqualTo(0.25));
        }

        [Test]
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
            var controller = new HomeController { DocumentSession = Session };

            // Act
            var viewResult = controller.Player("Mikael Axelsson") as ViewResult;
            Assert.NotNull(viewResult);
            var result = viewResult.Model as PlayerMatchesViewModel;

            // Assert
            Assert.NotNull(result);
            Debug.Assert(result != null, "result != null");
            Assert.That(result.Results.AverageStrikes, Is.EqualTo(5.0));
            Assert.That(result.Results.AverageMisses, Is.EqualTo(2.0));
            Assert.That(result.Results.GamesWithStats, Is.EqualTo(1));
            Assert.That(result.Results.BestGame, Is.EqualTo(223));
            Assert.That(result.Results.AverageStrikes, Is.EqualTo(5.0));
            Assert.That(result.Results.AverageMisses, Is.EqualTo(2.0));
            Assert.That(result.Results.AverageOnePinMisses, Is.EqualTo(1.0));
            Assert.That(result.Results.AverageSplits, Is.EqualTo(2.0));
            Assert.That(result.Results.AveragePins, Is.EqualTo(845.0 / 4));
            Assert.That(result.Results.AverageScore, Is.EqualTo(0.75));
        }
    }
}