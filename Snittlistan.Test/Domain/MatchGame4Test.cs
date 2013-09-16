using System;
using Snittlistan.Web.Areas.V2.Domain.Match;
using Xunit;

namespace Snittlistan.Test.Domain
{
    public class MatchGame4Test
    {
        [Fact]
        public void InvalidPins()
        {
            Assert.Throws<ArgumentException>(() => new MatchGame4("player-1", 0, -1));
            Assert.Throws<ArgumentException>(() => new MatchGame4("player-1", 0, 301));
        }

        [Fact]
        public void ValidPins()
        {
            Assert.DoesNotThrow(() => new MatchGame4("player-1", 0, 0));
            Assert.DoesNotThrow(() => new MatchGame4("player-1", 0, 150));
            Assert.DoesNotThrow(() => new MatchGame4("player-1", 0, 300));
        }
    }
}