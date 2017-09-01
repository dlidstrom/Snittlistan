using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EventStoreLite;
using Moq;
using NUnit.Framework;
using Snittlistan.Test.ApiControllers;
using Snittlistan.Web.Areas.V2.Commands;
using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Areas.V2.Domain.Match;
using Snittlistan.Web.Areas.V2.Domain.Match.Events;
using Snittlistan.Web.Areas.V2.ReadModels;
using Snittlistan.Web.DomainEvents;
using Snittlistan.Web.Infrastructure;

namespace Snittlistan.Test.Domain
{
    [TestFixture]
    public class MatchResult_MatchCommentary : WebApiIntegrationTest
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
            Assert.That(string.Join(" ", matchCommentaryEvent.BodyText), Is.EqualTo(testCase.ExpectedBodyText));
        }

        private MatchResult Act(TestCase testCase)
        {
            // Arrange
            var bitsClient = new BitsClient();
            var players = new[]
            {
                new Player("Alf Kindblom", "e@d.com", Player.Status.Active, -1, "Affe"),
                new Player("Bengt Solvander", "e@d.com", Player.Status.Active, -1, "Sollan"),
                new Player("Christer Holmström", "e@d.com", Player.Status.Active, -1, "Holmis"),
                new Player("Christer Liedholm", "e@d.com", Player.Status.Active, -1, "Chrille"),
                new Player("Claes Trankärr", "e@d.com", Player.Status.Active, -1, "Traanan"),
                new Player("Daniel Lidström", "e@d.com", Player.Status.Active, -1, "Lidas"),
                new Player("Daniel Solvander", "e@d.com", Player.Status.Active, -1, "Solen"),
                new Player("Hans Norbeck", "e@d.com", Player.Status.Active, -1, "Hasse"),
                new Player("Håkan Gustavsson", "e@d.com", Player.Status.Active, -1, "Håkan"),
                new Player("Karl-Erik Frick", "e@d.com", Player.Status.Active, -1, "Kalle"),
                new Player("Kjell Jansson", "e@d.com", Player.Status.Active, -1, "Jansson"),
                new Player("Kjell Johansson", "e@d.com", Player.Status.Active, -1, "Kjelle"),
                new Player("Lars Magnusson", "e@d.com", Player.Status.Active, -1, "Lasse Magnus"),
                new Player("Lars Norbeck", "e@d.com", Player.Status.Active, -1, "Norpan"),
                new Player("Lars Öberg", "e@d.com", Player.Status.Active, -1, "Öberg"),
                new Player("Lennart Axelsson", "e@d.com", Player.Status.Active, -1, "Laxen"),
                new Player("Magnus Sjöholm", "e@d.com", Player.Status.Active, -1, "Masken"),
                new Player("Markus Norbeck", "e@d.com", Player.Status.Active, -1, "Markus"),
                new Player("Mathias Ernest", "e@d.com", Player.Status.Active, -1, "Ernest"),
                new Player("Matz Classon", "e@d.com", Player.Status.Active, -1, "Classon"),
                new Player("Mikael Axelsson", "e@d.com", Player.Status.Active, -1, "Micke"),
                new Player("Per-Erik Freij", "e@d.com", Player.Status.Active, -1, "Perre"),
                new Player("Peter Engborg", "e@d.com", Player.Status.Active, -1, "Peter E"),
                new Player("Peter Sjöberg", "e@d.com", Player.Status.Active, -1, "Peter S"),
                new Player("Ralph Svensson", "e@d.com", Player.Status.Active, -1, "Raffe"),
                new Player("Stefan Markenfelt", "e@d.com", Player.Status.Active, -1, "Marken"),
                new Player("Stefan Traav", "e@d.com", Player.Status.Active, -1, "Traav"),
                new Player("Thomas Wallgren", "e@d.com", Player.Status.Active, -1, "TW"),
                new Player("Thomas Gurell", "e@d.com", Player.Status.Active, -1, "Gurkan"),
                new Player("Tomas Gustavsson", "e@d.com", Player.Status.Active, -1, "Gusten"),
                new Player("Tony Nordström", "e@d.com", Player.Status.Active, -1, "Tony"),
                new Player("Torbjörn Jensen", "e@d.com", Player.Status.Active, -1, "Tobbe")
            };
            Transact(session =>
            {
                foreach (var player in players)
                {
                    session.Store(player);
                }
            });
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

            // prepare some results
            Transact(session =>
            {
                var playerResults = new Dictionary<string, int[]>(); 
                var nicknameToId = players.ToDictionary(x => x.Nickname);
                playerResults[nicknameToId["Ernest"].Id] = new[] { 205 };
                playerResults[nicknameToId["Lidas"].Id] = new[] { 190 };
                playerResults[nicknameToId["Laxen"].Id] = new[] { 190 };
                playerResults[nicknameToId["Lasse Magnus"].Id] = new[] { 205 };
                playerResults[nicknameToId["Norpan"].Id] = new[] { 190 };
                playerResults[nicknameToId["Traav"].Id] = new[] { 190 };
                foreach (var playerId in playerResults.Keys)
                {
                    for (var bitsMatchId = 0; bitsMatchId < 5; bitsMatchId++)
                    {
                        var resultForPlayer = new ResultForPlayerReadModel(2017, playerId, bitsMatchId, DateTime.Now);
                        foreach (var playerResult in playerResults[playerId])
                        {
                            resultForPlayer.AddGame(1, new MatchGame(playerId, playerResult, 0, 0));
                        }

                        session.Store(resultForPlayer);
                    }
                }
            });

            // Act
            MatchResult matchResult = null;
            using (DomainEvent.Disable())
            {
                Transact(session =>
                {
                    var eventStoreSession = Mock.Of<IEventStoreSession>();
                    Mock.Get(eventStoreSession)
                        .Setup(x => x.Store(It.IsAny<AggregateRoot>()))
                        .Callback((AggregateRoot ar) => matchResult = (MatchResult)ar);
                    command.Execute(session, eventStoreSession);
                });
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