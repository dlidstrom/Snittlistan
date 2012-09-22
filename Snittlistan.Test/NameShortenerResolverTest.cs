namespace Snittlistan.Test
{
    using Castle.Windsor;

    using Snittlistan.Web.Infrastructure.AutoMapper;
    using Snittlistan.Web.Infrastructure.Installers;
    using Snittlistan.Web.Models;
    using Snittlistan.Web.ViewModels.Match;

    using Xunit;

    public class NameShortenerResolverTest
    {
        public NameShortenerResolverTest()
        {
            AutoMapperConfiguration
                .Configure(new WindsorContainer().Install(new AutoMapperInstaller()));
        }

        [Fact]
        public void SimpleCase()
        {
            // Arrange
            var game = new Game8x4("Daniel Lidström", 200);

            // Act
            var shortenedName = game.MapTo<Team8x4DetailsViewModel.Game>().Player;

            // Assert
            Assert.Equal("D. Lidström", shortenedName);
        }

        [Fact]
        public void DoubleName()
        {
            // Arrange
            var game = new Game8x4("Karl-Erik Frick", 200);

            // Act
            var shortenedName = game.MapTo<Team8x4DetailsViewModel.Game>().Player;

            // Assert
            Assert.Equal("K-E. Frick", shortenedName);
        }

        [Fact]
        public void PlayerNull()
        {
            // Arrange
            var game = new Game8x4(null, 0);

            // Act
            var shortenedName = game.MapTo<Team8x4DetailsViewModel.Game>().Player;

            // Assert
            Assert.Empty(shortenedName);
        }
    }
}