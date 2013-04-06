using System;
using Snittlistan.Web.Areas.V2.Domain.Match;
using Xunit;

namespace Snittlistan.Test.Domain
{
    public class MatchGameTest
    {
        [Fact]
        public void InvalidPins()
        {
            Assert.Throws<ArgumentException>(() => new MatchGame("player-1", -1, 0, 0));
            Assert.Throws<ArgumentException>(() => new MatchGame("player-1", 301, 0, 0));
        }

        [Fact]
        public void ValidPins()
        {
            Assert.DoesNotThrow(() => new MatchGame("player-1", 0, 0, 0));
            Assert.DoesNotThrow(() => new MatchGame("player-1", 150, 0, 0));
            Assert.DoesNotThrow(() => new MatchGame("player-1", 300, 0, 0));
        }

        [Fact]
        public void InvalidStrikes()
        {
            Assert.Throws<ArgumentException>(() => new MatchGame("player-1", 0, -1, 0));
            Assert.Throws<ArgumentException>(() => new MatchGame("player-1", 0, 13, 0));
        }

        [Fact]
        public void ValidStrikes()
        {
            Assert.DoesNotThrow(() => new MatchGame("player-1", 0, 0, 0));
            Assert.DoesNotThrow(() => new MatchGame("player-1", 0, 6, 0));
            Assert.DoesNotThrow(() => new MatchGame("player-1", 0, 12, 0));
        }

        [Fact]
        public void InvalidSpares()
        {
            Assert.Throws<ArgumentException>(() => new MatchGame("player-1", 0, 0, -1));
            Assert.Throws<ArgumentException>(() => new MatchGame("player-1", 0, 0, 11));
            Assert.Throws<ArgumentException>(() => new MatchGame("player-1", 0, 10, 3));
        }

        [Fact]
        public void ValidSpares()
        {
            Assert.DoesNotThrow(() => new MatchGame("player-1", 0, 0, 0));
            Assert.DoesNotThrow(() => new MatchGame("player-1", 0, 0, 10));
            Assert.DoesNotThrow(() => new MatchGame("player-1", 0, 7, 5));
            Assert.DoesNotThrow(() => new MatchGame("player-1", 0, 10, 0));
            Assert.DoesNotThrow(() => new MatchGame("player-1", 0, 10, 1));
            Assert.DoesNotThrow(() => new MatchGame("player-1", 0, 10, 2));
        }
    }
}
