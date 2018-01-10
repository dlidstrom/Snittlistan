using System;
using NUnit.Framework;
using Snittlistan.Web.Areas.V2.Domain.Match;

namespace Snittlistan.Test.Domain
{
    [TestFixture]
    public class MatchTableTest
    {
        [Test]
        public void MustHaveDifferentPlayers()
        {
            // Arrange
            var game1 = new MatchGame("p1", 0, 0, 0);
            var game2 = new MatchGame("p1", 0, 0, 0);

            // Act & Assert
            var ex = Assert.Throws<MatchException>(() => new MatchTable(1, game1, game2, 0));
            Assert.That(ex.Message, Is.EqualTo("Table must have different players"));
        }

        [Test]
        public void ValidScore()
        {
            // Arrange
            var game1 = new MatchGame("p1", 0, 0, 0);
            var game2 = new MatchGame("p2", 0, 0, 0);

            // Act & Assert
            Assert.DoesNotThrow(() => new MatchTable(1, game1, game2, 0));
            Assert.DoesNotThrow(() => new MatchTable(2, game1, game2, 1));
        }

        [Test]
        public void InvalidScore()
        {
            // Arrange
            var game1 = new MatchGame("p1", 0, 0, 0);
            var game2 = new MatchGame("p2", 0, 0, 0);

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new MatchTable(1, game1, game2, -1));
            Assert.Throws<ArgumentOutOfRangeException>(() => new MatchTable(2, game1, game2, 2));
        }
    }
}
