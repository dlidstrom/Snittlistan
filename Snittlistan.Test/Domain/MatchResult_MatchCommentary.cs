using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EventStoreLite;
using Moq;
using NUnit.Framework;
using Raven.Client;
using Snittlistan.Web.Areas.V2.Commands;
using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Areas.V2.Domain.Match;
using Snittlistan.Web.Areas.V2.Domain.Match.Events;
using Snittlistan.Web.DomainEvents;
using Snittlistan.Web.Infrastructure;

namespace Snittlistan.Test.Domain
{
    [TestFixture]
    public class MatchResult_MatchCommentary
    {
        [TestCaseSource("BitsMatchIdAndCommentaries")]
        public void MatchCommentarySummaryText(TestCase testCase)
        {
            // Act
            var matchResult = Act(testCase);

            // Assert
            var changes = matchResult.GetUncommittedChanges();
            var matchCommentaryEvent = (MatchCommentaryEvent)changes.Single(x => x is MatchCommentaryEvent);
            Assert.That(matchCommentaryEvent.SummaryText, Is.EqualTo(testCase.ExpectedSummaryText));
        }

        [TestCaseSource("BitsMatchIdAndCommentaries")]
        public void MatchCommentaryBodyText(TestCase testCase)
        {
            // Act
            var matchResult = Act(testCase);

            // Assert
            var changes = matchResult.GetUncommittedChanges();
            var matchCommentaryEvent = (MatchCommentaryEvent)changes.Single(x => x is MatchCommentaryEvent);
            Assert.That(matchCommentaryEvent.BodyText, Is.EqualTo(testCase.ExpectedBodyText));
        }

        private static MatchResult Act(TestCase testCase)
        {
            // Arrange
            var bitsClient = new BitsClient();
            var players = new[]
            {
                new Player("Alf Kindblom", "e@d.com", Player.Status.Active, -1, null)
                {
                    Id = "players-31"
                },
                new Player("Bengt Solvander", "e@d.com", Player.Status.Active, -1, null)
                {
                    Id = "players-1"
                },
                new Player("Christer Holmström", "e@d.com", Player.Status.Active, -1, null)
                {
                    Id = "players-10"
                },
                new Player("Christer Liedholm", "e@d.com", Player.Status.Active, -1, null)
                {
                    Id = "players-19"
                },
                new Player("Claes Trankärr", "e@d.com", Player.Status.Active, -1, null)
                {
                    Id = "players-29"
                },
                new Player("Daniel Lidström", "e@d.com", Player.Status.Active, -1, "Lidas")
                {
                    Id = "players-2"
                },
                new Player("Daniel Solvander", "e@d.com", Player.Status.Active, -1, null)
                {
                    Id = "players-3"
                },
                new Player("Hans Norbeck", "e@d.com", Player.Status.Active, -1, null)
                {
                    Id = "players-30"
                },
                new Player("Håkan Gustavsson", "e@d.com", Player.Status.Active, -1, null)
                {
                    Id = "players-4"
                },
                new Player("Karl-Erik Frick", "e@d.com", Player.Status.Active, -1, null)
                {
                    Id = "players-11"
                },
                new Player("Kjell Jansson", "e@d.com", Player.Status.Active, -1, null)
                {
                    Id = "players-25"
                },
                new Player("Kjell Johansson", "e@d.com", Player.Status.Active, -1, null)
                {
                    Id = "players-5"
                },
                new Player("Lars Magnusson", "e@d.com", Player.Status.Active, -1, "Lasse Magnus")
                {
                    Id = "players-17"
                },
                new Player("Lars Norbeck", "e@d.com", Player.Status.Active, -1, null)
                {
                    Id = "players-13"
                },
                new Player("Lars Öberg", "e@d.com", Player.Status.Active, -1, null)
                {
                    Id = "players-14"
                },
                new Player("Lennart Axelsson", "e@d.com", Player.Status.Active, -1, "Laxen")
                {
                    Id = "players-6"
                },
                new Player("Magnus Sjöholm", "e@d.com", Player.Status.Active, -1, null)
                {
                    Id = "players-18"
                },
                new Player("Markus Norbeck", "e@d.com", Player.Status.Active, -1, null)
                {
                    Id = "players-27"
                },
                new Player("Mathias Ernest", "e@d.com", Player.Status.Active, -1, "Ernest")
                {
                    Id = "players-15"
                },
                new Player("Matz Classon", "e@d.com", Player.Status.Active, -1, null)
                {
                    Id = "players-7"
                },
                new Player("Mikael Axelsson", "e@d.com", Player.Status.Active, -1, null)
                {
                    Id = "players-20"
                },
                new Player("Per-Erik Freij", "e@d.com", Player.Status.Active, -1, null)
                {
                    Id = "players-23"
                },
                new Player("Peter Engborg", "e@d.com", Player.Status.Active, -1, null)
                {
                    Id = "players-24"
                },
                new Player("Peter Sjöberg", "e@d.com", Player.Status.Active, -1, null)
                {
                    Id = "players-32"
                },
                new Player("Ralph Svensson", "e@d.com", Player.Status.Active, -1, null)
                {
                    Id = "players-22"
                },
                new Player("Stefan Markenfelt", "e@d.com", Player.Status.Active, -1, null)
                {
                    Id = "players-28"
                },
                new Player("Stefan Traav", "e@d.com", Player.Status.Active, -1, "Traav")
                {
                    Id = "players-8"
                },
                new Player("Thomas Wallgren", "e@d.com", Player.Status.Active, -1, null)
                {
                    Id = "players-12"
                },
                new Player("Thomas Gurell", "e@d.com", Player.Status.Active, -1, null)
                {
                    Id = "players-9"
                },
                new Player("Tomas Gustavsson", "e@d.com", Player.Status.Active, -1, null)
                {
                    Id = "players-21"
                },
                new Player("Tony Nordström", "e@d.com", Player.Status.Active, -1, null)
                {
                    Id = "players-26"
                },
                new Player("Torbjörn Jensen", "e@d.com", Player.Status.Active, -1, null)
                {
                    Id = "players-16"
                }
            };
            var bitsParser = new BitsParser(players);

            string content;
            var directoryName = Directory.GetCurrentDirectory();
            var path = Path.Combine(
                directoryName,
                string.Format("BitsMatch-{0}.html", testCase.BitsMatchId));
            try
            {
                content = File.ReadAllText(path);
            }
            catch (Exception)
            {
                content = bitsClient.DownloadMatchResult(testCase.BitsMatchId);
                File.WriteAllText(path, content);
            }

            var parseResult = bitsParser.Parse(content, "Fredrikshof");
            var rosterPlayerIds = new HashSet<string>(
                parseResult.Series.SelectMany(x => x.Tables.SelectMany(y => new[] { y.Game1.Player, y.Game2.Player })));
            var roster = new Roster(2017, 1, testCase.BitsMatchId, "Fredrikshof", "A", string.Empty, string.Empty, DateTime.Now, false)
            {
                Id = "rosters-1",
                Players = rosterPlayerIds.ToList()
            };
            var command = new RegisterMatchCommand(roster, parseResult);

            // Act
            MatchResult matchResult = null;
            using (DomainEvent.Disable())
            {
                var session = Mock.Of<IDocumentSession>();
                Mock.Get(session)
                    .Setup(x => x.Load<Player>(It.IsAny<List<string>>()))
                    .Returns(players);
                var eventStoreSession = Mock.Of<IEventStoreSession>();
                Mock.Get(eventStoreSession)
                    .Setup(x => x.Store(It.IsAny<AggregateRoot>()))
                    .Callback((AggregateRoot ar) => matchResult = (MatchResult)ar);
                command.Execute(session, eventStoreSession);
            }
            return matchResult;
        }

        private static IEnumerable<TestCase> BitsMatchIdAndCommentaries
        {
            get
            {
                yield return new TestCase(3050553, "10-10", null, null);
                yield return new TestCase(3067035, "10-10", null, null);
                yield return new TestCase(3122544, "10-10", null, null);
                yield return new TestCase(3128352, "10-10", null, null);
                yield return new TestCase(3128387, "10-10", null, null);
                yield return new TestCase(3119116, "20-0", null, null);
                yield return new TestCase(3119211, "19-1", null, null);
                yield return new TestCase(3083803, "13-7", null, null);
                yield return new TestCase(3105692, "16-4", null, null);
                yield return new TestCase(3119140, "17-3", null, null);
                yield return new TestCase(3119150, "6-13", null, null);
                yield return new TestCase(3119219, "11-8", null, null);
            }
        }

        public class TestCase
        {
            public TestCase(
                int bitsMatchId,
                string result,
                string expectedSummaryText,
                string expectedBodyText)
            {
                BitsMatchId = bitsMatchId;
                Result = result;
                ExpectedSummaryText = expectedSummaryText;
                ExpectedBodyText = expectedBodyText;
            }

            public int BitsMatchId { get; private set; }

            public string Result { get; private set; }

            public string ExpectedSummaryText { get; private set; }

            public string ExpectedBodyText { get; private set; }

            public override string ToString()
            {
                return string.Format("{0}, {1}", BitsMatchId, Result);
            }
        }
    }
}