using NUnit.Framework;
using Snittlistan.Web.Areas.V2.Domain.Match;

namespace Snittlistan.Test.Domain;
[TestFixture]
public class MatchGame4Test
{
    [Test]
    public void InvalidPins()
    {
        Assert.Throws<ArgumentException>(() => new MatchGame4("player-1", 0, -1));
        Assert.Throws<ArgumentException>(() => new MatchGame4("player-1", 0, 301));
    }

    [Test]
    public void ValidPins()
    {
        Assert.DoesNotThrow(() => new MatchGame4("player-1", 0, 0));
        Assert.DoesNotThrow(() => new MatchGame4("player-1", 0, 150));
        Assert.DoesNotThrow(() => new MatchGame4("player-1", 0, 300));
    }
}
