using System;
using System.Collections.Generic;
using NUnit.Framework;
using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Areas.V2.Domain.Match;
using Snittlistan.Web.Areas.V2.Domain.Match.Events;
using Snittlistan.Web.DomainEvents;

namespace Snittlistan.Test.Domain
{
    [TestFixture]
    public class MatchResult_RegisterSeries
    {
        private Roster roster;
        private MatchResult matchResult;
        private MatchRegisteredEvent ev;

        [SetUp]
        public void SetUp()
        {
            // Arrange
            roster = new Roster(2012, 11, 1, "H", "A", "L", "A", new DateTime(2012, 2, 3), false)
            {
                Id = "rosters-1",
                Players = new List<string>
                {
                    "p1",
                    "p2",
                    "p3",
                    "p4",
                    "p5",
                    "p6",
                    "p7",
                    "p8"
                }
            };
            matchResult = new MatchResult(roster, 9, 11, 123);

            // Act
            var series = new[]
            {
                new MatchSerie(
                    new List<MatchTable>
                    {
                        new MatchTable(new MatchGame("p1", 169, 0, 0), new MatchGame("p2", 0, 0, 0), 0),
                        new MatchTable(new MatchGame("p3", 0, 0, 0), new MatchGame("p4", 170, 0, 0), 0),
                        new MatchTable(new MatchGame("p5", 0, 0, 0), new MatchGame("p6", 0, 0, 0), 0),
                        new MatchTable(new MatchGame("p7", 200, 0, 0), new MatchGame("p8", 0, 0, 0), 0),
                    }),
                new MatchSerie(
                    new List<MatchTable>
                    {
                        new MatchTable(new MatchGame("p1", 169, 0, 0), new MatchGame("p2", 0, 0, 0), 0),
                        new MatchTable(new MatchGame("p3", 0, 0, 0), new MatchGame("p4", 170, 0, 0), 0),
                        new MatchTable(new MatchGame("p5", 0, 0, 0), new MatchGame("p6", 0, 0, 0), 0),
                        new MatchTable(new MatchGame("p7", 200, 0, 0), new MatchGame("p8", 0, 0, 0), 0),
                    })
            };

            using (DomainEvent.TestWith(e => ev = (MatchRegisteredEvent)e))
            {
                matchResult.RegisterSeries(series);
            }
        }

        [Test]
        public void CanRegisterAllSeries()
        {
            // Assert
            var changes = matchResult.GetUncommittedChanges();
            Assert.That(changes, Has.Length.EqualTo(3));
            Assert.IsAssignableFrom<SerieRegistered>(changes[1]);
        }

        [Test]
        public void RaisesDomainEvent()
        {
            // Assert
            Assert.That(ev, Is.Not.Null);
        }
    }
}