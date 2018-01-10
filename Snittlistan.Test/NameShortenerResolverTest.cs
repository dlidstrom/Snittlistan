using Castle.Windsor;
using NUnit.Framework;
using Snittlistan.Web.Areas.V1.Models;
using Snittlistan.Web.Areas.V1.ViewModels.Match;
using Snittlistan.Web.Infrastructure.AutoMapper;
using Snittlistan.Web.Infrastructure.Installers;

namespace Snittlistan.Test
{
    [TestFixture]
    public class NameShortenerResolverTest
    {
        public NameShortenerResolverTest()
        {
            AutoMapperConfiguration
                .Configure(new WindsorContainer().Install(new AutoMapperInstaller()));
        }

        [Test]
        public void SimpleCase()
        {
            // Arrange
            var game = new Game8x4("Daniel Lidström", 200);

            // Act
            var shortenedName = game.MapTo<Team8x4DetailsViewModel.Game>().Player;

            // Assert
            Assert.That(shortenedName, Is.EqualTo("D. Lidström"));
        }

        [Test]
        public void DoubleName()
        {
            // Arrange
            var game = new Game8x4("Karl-Erik Frick", 200);

            // Act
            var shortenedName = game.MapTo<Team8x4DetailsViewModel.Game>().Player;

            // Assert
            Assert.That(shortenedName, Is.EqualTo("K-E. Frick"));
        }

        [Test]
        public void PlayerNull()
        {
            // Arrange
            var game = new Game8x4(null, 0);

            // Act
            var shortenedName = game.MapTo<Team8x4DetailsViewModel.Game>().Player;

            // Assert
            Assert.That(shortenedName, Is.Empty);
        }
    }
}