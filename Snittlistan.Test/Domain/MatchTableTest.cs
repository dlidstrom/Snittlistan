using System;
using Snittlistan.Web.Areas.V2.Domain;
using Xunit;

namespace Snittlistan.Test.Domain
{
    public class MatchTableTest
    {
        [Fact]
        public void MustHaveDifferentPlayers()
        {
            // Arrange
            var game1 = new MatchGame("p1", 0, 0, 0);
            var game2 = new MatchGame("p1", 0, 0, 0);

            // Act & Assert
            var ex = Assert.Throws<MatchException>(() => new MatchTable(game1, game2, 0));
            Assert.Equal("Table must have different players", ex.Message);
        }

        [Fact]
        public void ValidScore()
        {
            // Arrange
            var game1 = new MatchGame("p1", 0, 0, 0);
            var game2 = new MatchGame("p2", 0, 0, 0);

            // Act & Assert
            Assert.DoesNotThrow(() => new MatchTable(game1, game2, 0));
            Assert.DoesNotThrow(() => new MatchTable(game1, game2, 1));
        }

        [Fact]
        public void InvalidScore()
        {
            // Arrange
            var game1 = new MatchGame("p1", 0, 0, 0);
            var game2 = new MatchGame("p2", 0, 0, 0);

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new MatchTable(game1, game2, -1));
            Assert.Throws<ArgumentOutOfRangeException>(() => new MatchTable(game1, game2, 2));
        }
    }
}
