#nullable enable

using System.Net;
using System.Net.Http;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Moq;
using NUnit.Framework;
using Snittlistan.Queue;
using Snittlistan.Queue.Messages;
using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Areas.V2.ReadModels;
using Snittlistan.Web.Infrastructure.Bits;
using Snittlistan.Web.Infrastructure.Bits.Contracts;
using Snittlistan.Web.Infrastructure.Database;
using Snittlistan.Web.Models;

namespace Snittlistan.Test.ApiControllers;

[TestFixture]
public class Task_Post_RegisterMatch : WebApiIntegrationTest
{
    private RegisterPendingMatchTask? task;
    private MessageEnvelope? envelope;
    private HttpResponseMessage? responseMessage;
    private string? rosterId;

    [Test]
    public void ShouldRegisterPendingResult()
    {
        // Assert
        Assert.That(responseMessage!.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task ShouldStoreMatchResult()
    {
        await Transact(session =>
            {
                ResultHeaderReadModel resultReadModel = session.Load<ResultHeaderReadModel>("ResultHeader-3048746");
                Assert.That(resultReadModel, Is.Not.Null);
                return Task.CompletedTask;
            });
    }

    protected override async Task Act()
    {
        // Act
        TaskRequest request = new(envelope!);
        responseMessage = await Client.PostAsJsonAsync("http://temp.uri/api/task", request);
        if (responseMessage.IsSuccessStatusCode == false)
        {
            throw new Exception(await responseMessage.Content.ReadAsStringAsync());
        }
    }

    protected override async Task OnSetUp(IWindsorContainer container)
    {
        // Arrange
        await Transact(session =>
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

            Roster roster = new(
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
            return Task.CompletedTask;
        });

        IBitsClient bitsClient = Mock.Of<IBitsClient>();
        _ = Mock.Get(bitsClient)
            .Setup(x => x.GetMatchResults(3048746))
            .Returns(BitsGateway.GetMatchResults(3048746));
        _ = Mock.Get(bitsClient)
            .Setup(x => x.GetMatchScores(3048746))
            .Returns(BitsGateway.GetMatchScores(3048746));
        _ = Mock.Get(bitsClient)
            .Setup(x => x.GetHeadInfo(3048746))
            .Returns(BitsGateway.GetHeadInfo(3048746));
        _ = Mock.Get(bitsClient)
            .Setup(x => x.GetHeadResultInfo(3048746))
            .Returns(BitsGateway.GetHeadResultInfo(3048746));
        _ = container.Register(Component.For<IBitsClient>().Instance(bitsClient));

        task = new(rosterId!, 123);
        PublishedTask publishedTask = Databases.Snittlistan.PublishedTasks.Add(
            PublishedTask.CreateImmediate(
                task,
                task.BusinessKey.ToJson(),
                0,
                1,
                Guid.NewGuid(),
                null,
                "test"));
        envelope = new(
            task,
            publishedTask.TenantId,
            "",
            publishedTask.CorrelationId,
            null,
            publishedTask.MessageId);
    }
}
