using NUnit.Framework;

namespace Snittlistan.Test;
[TestFixture]
public class BitsClientTest
{
    [Test]
    public async Task ParsesTeam()
    {
        // Act
        Web.Infrastructure.Bits.Contracts.TeamResult[] team = await BitsGateway.GetTeam(51538, 2019);

        // Assert
        Assert.That(team, Has.Length.EqualTo(3));
        Assert.That(team[0].TeamId, Is.EqualTo(185185));
        Assert.That(team[0].TeamName, Is.EqualTo("Fredrikshof IF BK"));
        Assert.That(team[0].TeamAlias, Is.EqualTo("Fredrikshof IF BK A"));
        Assert.That(team[1].TeamId, Is.EqualTo(185186));
        Assert.That(team[1].TeamName, Is.EqualTo("Fredrikshof IF BK B"));
        Assert.That(team[1].TeamAlias, Is.EqualTo("Fredrikshof IF BK B"));
        Assert.That(team[2].TeamId, Is.EqualTo(185187));
        Assert.That(team[2].TeamName, Is.EqualTo("Fredrikshof IF BK F"));
        Assert.That(team[2].TeamAlias, Is.EqualTo("Fredrikshof IF BK F"));
    }

    [Test]
    public async Task ParsesDivisions()
    {
        // Act
        Web.Infrastructure.Bits.Contracts.DivisionResult[] divisions = await BitsGateway.GetDivisions(185185, 2019);

        // Assert
        Assert.That(divisions, Has.Length.EqualTo(1));
        Assert.That(divisions[0].DivisionId, Is.EqualTo(8));
        Assert.That(divisions[0].DivisionName, Is.EqualTo("Div 1 Södra Svealand"));
    }

    [Test]
    public async Task ParsesMatchRounds()
    {
        // Act
        Web.Infrastructure.Bits.Contracts.MatchRound[] matches = await BitsGateway.GetMatchRounds(185567, 684, 2019);

        // Assert
        Assert.That(matches, Has.Length.EqualTo(15));
        Assert.That(matches[0].MatchRoundId, Is.EqualTo(2));
    }
}
