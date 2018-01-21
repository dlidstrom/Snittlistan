using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using Castle.MicroKernel.Registration;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using Snittlistan.Queue.Messages;
using Snittlistan.Test.Properties;
using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Areas.V2.ReadModels;
using Snittlistan.Web.Infrastructure;

namespace Snittlistan.Test.ApiControllers
{
    [TestFixture]
    public class Task_Post_RegisterMatch : WebApiIntegrationTest
    {
        private HttpResponseMessage responseMessage;
        private string content;
        private HttpContent httpContent;

        [Test]
        public void ShouldRegisterPendingResult()
        {
            // Assert
            Assert.That(httpContent, Is.Not.Null);
            Assert.That(responseMessage.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public void ShouldReturnInfoAboutRegisteredMatches()
        {
            var expected = new[]
            {
                new
                {
                    date = "2013-04-27T00:00:00",
                    season = 2012,
                    turn = 1,
                    bitsMatchId = 3048746,
                    team = "Fredrikshof IF",
                    location = "Bowl-O-Rama",
                    opponent = "Högdalen BK"
                }
            };
            Assert.That(content, Is.EqualTo(JsonConvert.SerializeObject(expected)));
        }

        [Test]
        public void ShouldStoreMatchResult()
        {
            Transact(session =>
                {
                    var resultReadModel = session.Load<ResultHeaderReadModel>("ResultHeader-3048746");
                    Assert.That(resultReadModel, Is.Not.Null);
                });
        }

        protected override void Act()
        {
            // Act
            var request = new TaskRequest(new MessageEnvelope(new RegisterMatchesMessage(), new Uri("http://temp.uri/")));
            responseMessage = Client.PostAsJsonAsync("http://temp.uri/api/task", request).Result;
            responseMessage.EnsureSuccessStatusCode();

            httpContent = responseMessage.Content;
            content = httpContent.ReadAsStringAsync().Result;
        }

        protected override void OnSetUp(Castle.Windsor.IWindsorContainer container)
        {
            // Arrange
            Transact(session =>
            {
                var players = new[]
                {
                    new Player("Christer Liedholm", "e@d.com", Player.Status.Active, 0, null),
                    new Player("Mathias Ernest", "e@d.com", Player.Status.Active, 0, null),
                    new Player("Torbjörn Jensen", "e@d.com", Player.Status.Active, 0, null),
                    new Player("Alf Kindblom", "e@d.com", Player.Status.Active, 0, null),
                    new Player("Peter Sjöberg", "e@d.com", Player.Status.Active, 0, null),
                    new Player("Lars Öberg", "e@d.com", Player.Status.Active, 0, null),
                    new Player("Mikael Axelsson", "e@d.com", Player.Status.Active, 0, null),
                    new Player("Hans Norbeck", "e@d.com", Player.Status.Active, 0, null),
                    new Player("Lennart Axelsson", "e@d.com", Player.Status.Active, 0, null)
                };

                foreach (var player in players)
                {
                    session.Store(player);
                }

                var roster = new Roster(
                    2012,
                    1,
                    3048746,
                    "Fredrikshof IF",
                    "A",
                    "Bowl-O-Rama",
                    "Högdalen BK",
                    new DateTime(2013, 4, 27),
                    false,
                    OilPatternInformation.Empty)
                {
                    Players = players.Select(x => x.Id).ToList()
                };
                session.Store(roster);
            });

            var bitsClient = Mock.Of<IBitsClient>(x => x.DownloadMatchResult(3048746) == Resources.Id3048746);
            container.Register(Component.For<IBitsClient>().Instance(bitsClient));
        }
    }
}