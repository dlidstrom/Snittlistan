namespace Snittlistan.Test
{
    using Castle.Windsor;
    using Infrastructure.AutoMapper;
    using Infrastructure.Installers;
    using Models;
    using MvcContrib.TestHelper;
    using ViewModels.Match;
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
            shortenedName.ShouldBe("D. Lidström");
        }

        [Fact]
        public void DoubleName()
        {
            // Arrange
            var game = new Game8x4("Karl-Erik Frick", 200);

            // Act
            var shortenedName = game.MapTo<Team8x4DetailsViewModel.Game>().Player;

            // Assert
            shortenedName.ShouldBe("K-E. Frick");
        }

        [Fact]
        public void PlayerNull()
        {
            // Arrange
            var game = new Game8x4(null, 0);

            // Act
            var shortenedName = game.MapTo<Team8x4DetailsViewModel.Game>().Player;

            // Assert
            shortenedName.ShouldBe(string.Empty);
        }
    }
}