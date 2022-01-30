using NUnit.Framework;
using Snittlistan.Web.Areas.V2.Domain.Match;

#nullable enable

namespace Snittlistan.Test.Domain;
[TestFixture]
public class MatchTableTest
{
    [Test]
    public void MustHaveDifferentPlayers()
    {
        // Arrange
        MatchGame game1 = new("p1", 0, 0, 0);
        MatchGame game2 = new("p1", 0, 0, 0);

        // Act & Assert
        MatchException? ex = Assert.Throws<MatchException>(() => new MatchTable(1, game1, game2, 0));
        Assert.That(ex?.Message, Is.EqualTo("Table must have different players"));
    }

    [Test]
    public void ValidScore()
    {
        // Arrange
        MatchGame game1 = new("p1", 0, 0, 0);
        MatchGame game2 = new("p2", 0, 0, 0);

        // Act & Assert
        Assert.DoesNotThrow(() => new MatchTable(1, game1, game2, 0));
        Assert.DoesNotThrow(() => new MatchTable(2, game1, game2, 1));
    }

    [Test]
    public void InvalidScore()
    {
        // Arrange
        MatchGame game1 = new("p1", 0, 0, 0);
        MatchGame game2 = new("p2", 0, 0, 0);

        // Act & Assert
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => new MatchTable(1, game1, game2, -1));
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => new MatchTable(2, game1, game2, 2));
    }
}
