namespace Snittlistan.Test.ApiControllers
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using Moq;
    using Newtonsoft.Json;
    using NUnit.Framework;
    using Snittlistan.Queue.Messages;
    using Snittlistan.Web.Areas.V2.Domain;
    using Snittlistan.Web.Areas.V2.ReadModels;
    using Web.Infrastructure.Bits;
    using Web.Infrastructure.Bits.Contracts;
    using Web.Models;

    [TestFixture]
    public class Task_Post_RegisterMatch : WebApiIntegrationTest
    {
        private HttpResponseMessage responseMessage;
        private string rosterId;

        [Test]
        public void ShouldRegisterPendingResult()
        {
            // Assert
            Assert.That(responseMessage.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public void ShouldStoreMatchResult()
        {
            Transact(session =>
                {
                    ResultHeaderReadModel resultReadModel = session.Load<ResultHeaderReadModel>("ResultHeader-3048746");
                    Assert.That(resultReadModel, Is.Not.Null);
                });
        }

        protected override async Task Act()
        {
            // Act
            var request = new TaskRequest(new MessageEnvelope(new RegisterMatchTask(rosterId), new Uri("http://temp.uri/")));
            responseMessage = await Client.PostAsJsonAsync("http://temp.uri/api/task", request);
            responseMessage.EnsureSuccessStatusCode();
        }

        protected override Task OnSetUp(IWindsorContainer container)
        {
            // Arrange
            Transact(session =>
            {
                session.Store(new WebsiteConfig(new[] { new WebsiteConfig.TeamNameAndLevel("FIF", "A") }, false, 1660, 2012));
                Player[] players = new[]
                {
                    new Player("Christer Liedholm", "e@d.com", Player.Status.Active, 0, null, new string[0], new PlayerItem { LicNbr = "M131061CHR01" }),
                    new Player("Mathias Ernest", "e@d.com", Player.Status.Active, 0, null, new string[0], new PlayerItem { LicNbr = "M131061CHR01" }),
                    new Player("Torbjörn Jensen", "e@d.com", Player.Status.Active, 0, null, new string[0], new PlayerItem { LicNbr = "M131061CHR01" }),
                    new Player("Alf Kindblom", "e@d.com", Player.Status.Active, 0, null, new string[0], new PlayerItem { LicNbr = "M131061CHR01" }),
                    new Player("Peter Sjöberg", "e@d.com", Player.Status.Active, 0, null, new string[0], new PlayerItem { LicNbr = "M131061CHR01" }),
                    new Player("Lars Öberg", "e@d.com", Player.Status.Active, 0, null, new string[0], new PlayerItem { LicNbr = "M131061CHR01" }),
                    new Player("Mikael Axelsson", "e@d.com", Player.Status.Active, 0, null, new string[0], new PlayerItem { LicNbr = "M131061CHR01" }),
                    new Player("Hans Norbeck", "e@d.com", Player.Status.Active, 0, null, new string[0], new PlayerItem { LicNbr = "M131061CHR01" }),
                    new Player("Lennart Axelsson", "e@d.com", Player.Status.Active, 0, null, new string[0], new PlayerItem { LicNbr = "M131061CHR01" })
                };

                foreach (Player player in players)
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
                rosterId = roster.Id;
            });

            IBitsClient bitsClient = Mock.Of<IBitsClient>();
            Mock.Get(bitsClient)
                .Setup(x => x.GetMatchResults(3048746))
                .Returns(BitsGateway.GetMatchResults(3048746));
            Mock.Get(bitsClient)
                .Setup(x => x.GetMatchScores(3048746))
                .Returns(BitsGateway.GetMatchScores(3048746));
            Mock.Get(bitsClient)
                .Setup(x => x.GetHeadInfo(3048746))
                .Returns(BitsGateway.GetHeadInfo(3048746));
            Mock.Get(bitsClient)
                .Setup(x => x.GetHeadResultInfo(3048746))
                .Returns(BitsGateway.GetHeadResultInfo(3048746));
            container.Register(Component.For<IBitsClient>().Instance(bitsClient));
            return Task.CompletedTask;
        }
    }
}
