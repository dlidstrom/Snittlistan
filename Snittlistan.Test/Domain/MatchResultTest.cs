using EventStoreLite;
using NUnit.Framework;
using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Areas.V2.Domain.Match;
using Snittlistan.Web.Areas.V2.Domain.Match.Events;

#nullable enable

namespace Snittlistan.Test.Domain;
[TestFixture]
public class MatchResultTest
{
    private readonly Roster roster;

    public MatchResultTest()
    {
        roster = new Roster(2012, 11, 1, "H", "A", "L", "A", new DateTime(2012, 2, 3), false, OilPatternInformation.Empty)
        {
            Id = "rosters-1"
        };
    }

    [Test]
    public void CanRegister()
    {
        // Act
        MatchResult matchResult = new(roster, 9, 11, 123);

        // Assert
        IDomainEvent[] uncommittedChanges = matchResult.GetUncommittedChanges();
        Assert.That(uncommittedChanges.Length, Is.EqualTo(1));
        MatchResultRegistered? registered = uncommittedChanges[0] as MatchResultRegistered;
        Assert.NotNull(registered);
        Assert.That(registered!.RosterId, Is.EqualTo("rosters-1"));
        Assert.That(registered.TeamScore, Is.EqualTo(9));
        Assert.That(registered.OpponentScore, Is.EqualTo(11));
        Assert.That(registered.BitsMatchId, Is.EqualTo(123));
    }

    [Test]
    public void RosterCannotHaveSevenPlayers()
    {
        // Arrange
        Roster invalidRoster = new(2012, 11, 1, "H", "A", "L", "A", new DateTime(2012, 2, 3), false, OilPatternInformation.Empty)
        {
            Id = "rosters-1",
            Players = new List<string>
                          {
                              "p1", "p2", "p3", "p4", "p5", "p6", "p7"
                          }
        };

        MatchResult matchResult = new(invalidRoster, 9, 11, 123);

        // Act
        MatchTable[] matchTables = new[]
            {
                    new MatchTable(1, new MatchGame("p1", 0, 0, 0), new MatchGame("p2", 0, 0, 0), 0),
                    new MatchTable(2, new MatchGame("p3", 0, 0, 0), new MatchGame("p4", 0, 0, 0), 0),
                    new MatchTable(3, new MatchGame("p5", 0, 0, 0), new MatchGame("p6", 0, 0, 0), 0),
                    new MatchTable(4, new MatchGame("p7", 0, 0, 0), new MatchGame("p8", 0, 0, 0), 0)
                };

        // Assert
        MatchException? ex = Assert.Throws<MatchException>(() => matchResult.RegisterSerie(matchTables));
        Assert.That(ex?.Message, Is.EqualTo("Roster must have 8, 9, or 10 players when registering results"));
    }

    [Test]
    public void RosterCanHaveEightPlayers()
    {
        // Arrange
        Roster validRoster = new(2012, 11, 1, "H", "A", "L", "A", new DateTime(2012, 2, 3), false, OilPatternInformation.Empty)
        {
            Id = "rosters-1",
            Players = new List<string>
                          {
                              "p1", "p2", "p3", "p4", "p5", "p6", "p7", "p8"
                          }
        };

        MatchResult matchResult = new(validRoster, 9, 11, 123);

        // Act
        MatchTable[] matchTables =
            new[]
            {
                    new MatchTable(1, new MatchGame("p1", 0, 0, 0), new MatchGame("p2", 0, 0, 0), 0),
                    new MatchTable(2, new MatchGame("p3", 0, 0, 0), new MatchGame("p4", 0, 0, 0), 0),
                    new MatchTable(3, new MatchGame("p5", 0, 0, 0), new MatchGame("p6", 0, 0, 0), 0),
                    new MatchTable(4, new MatchGame("p7", 0, 0, 0), new MatchGame("p8", 0, 0, 0), 0),
            };

        // Assert
        Assert.DoesNotThrow(() => matchResult.RegisterSerie(matchTables));
        SerieRegistered? serieRegistered = matchResult.GetUncommittedChanges()[1] as SerieRegistered;
        Assert.NotNull(serieRegistered);
        Assert.That(serieRegistered!.MatchSerie.SerieNumber, Is.EqualTo(1));
    }

    [Test]
    public void RosterCanHaveNinePlayers()
    {
        // Arrange
        Roster validRoster = new(2012, 11, 1, "H", "A", "L", "A", new DateTime(2012, 2, 3), false, OilPatternInformation.Empty)
        {
            Id = "rosters-1",
            Players = new List<string>
                          {
                              "p1", "p2", "p3", "p4", "p5", "p6", "p7", "p8", "p9"
                          }
        };

        MatchResult matchResult = new(validRoster, 9, 11, 123);

        // Act
        MatchTable[] matchTables =
            new[]
            {
                    new MatchTable(1, new MatchGame("p1", 0, 0, 0), new MatchGame("p2", 0, 0, 0), 0),
                    new MatchTable(2, new MatchGame("p3", 0, 0, 0), new MatchGame("p4", 0, 0, 0), 0),
                    new MatchTable(3, new MatchGame("p5", 0, 0, 0), new MatchGame("p6", 0, 0, 0), 0),
                    new MatchTable(4, new MatchGame("p7", 0, 0, 0), new MatchGame("p8", 0, 0, 0), 0)
            };

        // Assert
        Assert.DoesNotThrow(() => matchResult.RegisterSerie(matchTables));
    }

    [Test]
    public void RosterCanHaveTenPlayers()
    {
        // Arrange
        Roster validRoster = new(2012, 11, 1, "H", "A", "L", "A", new DateTime(2012, 2, 3), false, OilPatternInformation.Empty)
        {
            Id = "rosters-1",
            Players = new List<string>
                          {
                              "p1", "p2", "p3", "p4", "p5", "p6", "p7", "p8", "p9", "p10"
                          }
        };

        MatchResult matchResult = new(validRoster, 9, 11, 123);

        // Act
        MatchTable[] matchTables =
            new[]
            {
                    new MatchTable(1, new MatchGame("p1", 0, 0, 0), new MatchGame("p2", 0, 0, 0), 0),
                    new MatchTable(2, new MatchGame("p3", 0, 0, 0), new MatchGame("p4", 0, 0, 0), 0),
                    new MatchTable(3, new MatchGame("p5", 0, 0, 0), new MatchGame("p6", 0, 0, 0), 0),
                    new MatchTable(4, new MatchGame("p7", 0, 0, 0), new MatchGame("p8", 0, 0, 0), 0)
            };

        // Assert
        Assert.DoesNotThrow(() => matchResult.RegisterSerie(matchTables));
    }

    [Test]
    public void RosterCannotHaveElevenPlayers()
    {
        // Arrange
        Roster invalidRoster = new(2012, 11, 1, "H", "A", "L", "A", new DateTime(2012, 2, 3), false, OilPatternInformation.Empty)
        {
            Id = "rosters-1",
            Players = new List<string>
                          {
                              "p1", "p2", "p3", "p4", "p5", "p6", "p7", "p8", "p9", "p10", "p11"
                          }
        };

        MatchResult matchResult = new(invalidRoster, 9, 11, 123);

        // Act
        MatchTable[] matchTables = new[]
            {
                    new MatchTable(1, new MatchGame("p1", 0, 0, 0), new MatchGame("p2", 0, 0, 0), 0),
                    new MatchTable(2, new MatchGame("p3", 0, 0, 0), new MatchGame("p4", 0, 0, 0), 0),
                    new MatchTable(3, new MatchGame("p5", 0, 0, 0), new MatchGame("p6", 0, 0, 0), 0),
                    new MatchTable(4, new MatchGame("p7", 0, 0, 0), new MatchGame("p8", 0, 0, 0), 0)
                };

        // Assert
        MatchException? ex = Assert.Throws<MatchException>(() => matchResult.RegisterSerie(matchTables));
        Assert.That(ex!.Message, Is.EqualTo("Roster must have 8, 9, or 10 players when registering results"));
    }

    [Test]
    public void CanOnlyRegisterSeriesWithValidPlayer()
    {
        // Arrange
        Roster validRoster = new(2012, 11, 1, "H", "A", "L", "A", new DateTime(2012, 2, 3), false, OilPatternInformation.Empty)
        {
            Id = "rosters-1",
            Players = new List<string>
                          {
                              "p1", "p2", "p3", "p4", "p5", "p6", "p7", "p8"
                          }
        };
        MatchResult matchResult = new(validRoster, 9, 11, 123);

        // Act
        MatchTable[] matchTables = new[]
            {
                    new MatchTable(1, new MatchGame("p1", 0, 0, 0), new MatchGame("p2", 0, 0, 0), 0),
                    new MatchTable(2, new MatchGame("p3", 0, 0, 0), new MatchGame("p4", 0, 0, 0), 0),
                    new MatchTable(3, new MatchGame("p5", 0, 0, 0), new MatchGame("p6", 0, 0, 0), 0),
                    new MatchTable(4, new MatchGame("p7", 0, 0, 0), new MatchGame("p8", 0, 0, 0), 0)
                };

        // Assert
        Assert.DoesNotThrow(() => matchResult.RegisterSerie(matchTables));
    }

    [Test]
    public void CanRegisterWithReserve1()
    {
        // Arrange
        Roster validRoster = new(2012, 11, 1, "H", "A", "L", "A", new DateTime(2012, 2, 3), false, OilPatternInformation.Empty)
        {
            Id = "rosters-1",
            Players = new List<string>
                          {
                              "p1", "p2", "p3", "p4", "p5", "p6", "p7", "p8", "p9"
                          }
        };
        MatchResult matchResult = new(validRoster, 9, 11, 123);

        // Act
        MatchTable[] matchTables = new[]
            {
                    new MatchTable(1, new MatchGame("p1", 0, 0, 0), new MatchGame("p2", 0, 0, 0), 0),
                    new MatchTable(2, new MatchGame("p3", 0, 0, 0), new MatchGame("p4", 0, 0, 0), 0),
                    new MatchTable(3, new MatchGame("p5", 0, 0, 0), new MatchGame("p6", 0, 0, 0), 0),
                    new MatchTable(4, new MatchGame("p7", 0, 0, 0), new MatchGame("p8", 0, 0, 0), 0)
                };

        // Assert
        Assert.DoesNotThrow(() => matchResult.RegisterSerie(matchTables));
    }

    [Test]
    public void CanRegisterWithReserve1AndReserve2()
    {
        // Arrange
        Roster validRoster = new(2012, 11, 1, "H", "A", "L", "A", new DateTime(2012, 2, 3), false, OilPatternInformation.Empty)
        {
            Id = "rosters-1",
            Players = new List<string>
                          {
                              "p1", "p2", "p3", "p4", "p5", "p6", "p7", "p8", "p9", "p10"
                          }
        };
        MatchResult matchResult = new(validRoster, 9, 11, 123);

        // Act
        MatchTable[] matchTables = new[]
            {
                    new MatchTable(1, new MatchGame("p1", 0, 0, 0), new MatchGame("p2", 0, 0, 0), 0),
                    new MatchTable(2, new MatchGame("p3", 0, 0, 0), new MatchGame("p4", 0, 0, 0), 0),
                    new MatchTable(3, new MatchGame("p5", 0, 0, 0), new MatchGame("p6", 0, 0, 0), 0),
                    new MatchTable(4, new MatchGame("p7", 0, 0, 0), new MatchGame("p8", 0, 0, 0), 0)
                };

        // Assert
        Assert.DoesNotThrow(() => matchResult.RegisterSerie(matchTables));
    }

    [Test]
    public void CanNotRegisterInvalidPlayer()
    {
        // Arrange
        Roster validRoster = new(2012, 11, 1, "H", "A", "L", "A", new DateTime(2012, 2, 3), false, OilPatternInformation.Empty)
        {
            Id = "rosters-1",
            Players = new List<string>
                          {
                              "p1", "p2", "p3", "p4", "p5", "p6", "p7", "p8"
                          }
        };
        MatchResult matchResult = new(validRoster, 9, 11, 123);

        // Act
        MatchTable[] matchTables = new[]
            {
                    new MatchTable(1, new MatchGame("p1", 0, 0, 0), new MatchGame("p2", 0, 0, 0), 0),
                    new MatchTable(2, new MatchGame("invalid-id", 0, 0, 0), new MatchGame("p4", 0, 0, 0), 0),
                    new MatchTable(3, new MatchGame("p5", 0, 0, 0), new MatchGame("p6", 0, 0, 0), 0),
                    new MatchTable(4, new MatchGame("p7", 0, 0, 0), new MatchGame("p8", 0, 0, 0), 0)
                };

        // Assert
        MatchException? ex = Assert.Throws<MatchException>(() => matchResult.RegisterSerie(matchTables));
        Assert.That(ex!.Message, Is.EqualTo("Can only register players from roster"));
    }

    [Test]
    public void ValidScore()
    {
        Assert.DoesNotThrow(() => new MatchResult(roster, 0, 0, 0));
        Assert.DoesNotThrow(() => new MatchResult(roster, 0, 20, 0));
        Assert.DoesNotThrow(() => new MatchResult(roster, 20, 0, 0));
        Assert.DoesNotThrow(() => new MatchResult(roster, 10, 10, 0));
    }

    [Test]
    public void InvalidScore()
    {
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => new MatchResult(roster, -1, 0, 0));
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => new MatchResult(roster, 21, 0, 0));
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => new MatchResult(roster, 0, -1, 0));
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => new MatchResult(roster, 0, 21, 0));
        _ = Assert.Throws<ArgumentException>(() => new MatchResult(roster, 10, 11, 0));
    }

    [Test]
    public void MedalFor4Score()
    {
        // Arrange
        Roster validRoster = new(2012, 11, 1, "H", "A", "L", "A", new DateTime(2012, 2, 3), false, OilPatternInformation.Empty)
        {
            Id = "rosters-1",
            Players = new List<string>
                          {
                              "p1", "p2", "p3", "p4", "p5", "p6", "p7", "p8"
                          }
        };

        MatchResult matchResult = new(validRoster, 9, 11, 123);

        // Act
        for (int i = 0; i < 4; i++)
        {
            MatchTable[] matchTables = new[]
                {
                        new MatchTable(1, new MatchGame("p1", 0, 0, 0), new MatchGame("p2", 0, 0, 0), 1),
                        new MatchTable(2, new MatchGame("p3", 0, 0, 0), new MatchGame("p4", 0, 0, 0), 0),
                        new MatchTable(3, new MatchGame("p5", 0, 0, 0), new MatchGame("p6", 0, 0, 0), 0),
                        new MatchTable(4, new MatchGame("p7", 0, 0, 0), new MatchGame("p8", 0, 0, 0), 0),
                    };
            matchResult.RegisterSerie(matchTables);
        }

        // Assert
        IDomainEvent[] changes = matchResult.GetUncommittedChanges();
        Assert.That(changes.Length, Is.EqualTo(7));
        AwardedMedal? medal1 = changes[5] as AwardedMedal;
        AwardedMedal? medal2 = changes[6] as AwardedMedal;
        Assert.NotNull(medal1);
        Assert.NotNull(medal2);
        Assert.That(medal1!.Player, Is.EqualTo("p1"));
        Assert.That(medal1.MedalType, Is.EqualTo(MedalType.TotalScore));
        Assert.That(medal1.Value, Is.EqualTo(4));
        Assert.That(medal2!.Player, Is.EqualTo("p2"));
        Assert.That(medal2.MedalType, Is.EqualTo(MedalType.TotalScore));
        Assert.That(medal2.Value, Is.EqualTo(4));
    }

    [Test]
    public void MedalFor270OrMore()
    {
        // Arrange
        Roster validRoster = new(2012, 11, 1, "H", "A", "L", "A", new DateTime(2012, 2, 3), false, OilPatternInformation.Empty)
        {
            Id = "rosters-1",
            Players = new List<string>
                          {
                              "p1", "p2", "p3", "p4", "p5", "p6", "p7", "p8"
                          }
        };

        MatchResult matchResult = new(validRoster, 9, 11, 123);

        // Act
        MatchTable[] matchTables = new[]
            {
                    new MatchTable(1, new MatchGame("p1", 269, 0, 0), new MatchGame("p2", 0, 0, 0), 0),
                    new MatchTable(2, new MatchGame("p3", 0, 0, 0), new MatchGame("p4", 270, 0, 0), 0),
                    new MatchTable(3, new MatchGame("p5", 0, 0, 0), new MatchGame("p6", 0, 0, 0), 0),
                    new MatchTable(4, new MatchGame("p7", 300, 0, 0), new MatchGame("p8", 0, 0, 0), 0),
                };
        matchResult.RegisterSerie(matchTables);

        // Assert
        IDomainEvent[] changes = matchResult.GetUncommittedChanges();
        Assert.That(changes.Length, Is.EqualTo(4));
        AwardedMedal? medal1 = changes[2] as AwardedMedal;
        AwardedMedal? medal2 = changes[3] as AwardedMedal;
        Assert.NotNull(medal1);
        Assert.NotNull(medal2);
        Assert.That(medal1!.Player, Is.EqualTo("p4"));
        Assert.That(medal1.MedalType, Is.EqualTo(MedalType.PinsInSerie));
        Assert.That(medal1.Value, Is.EqualTo(270));
        Assert.That(medal2!.Player, Is.EqualTo("p7"));
        Assert.That(medal2.MedalType, Is.EqualTo(MedalType.PinsInSerie));
        Assert.That(medal2.Value, Is.EqualTo(300));
    }

    [Test]
    public void OnlyAwardMedalsOnce()
    {
        // Arrange
        Roster validRoster = new(2012, 11, 1, "H", "A", "L", "A", new DateTime(2012, 2, 3), false, OilPatternInformation.Empty)
        {
            Id = "rosters-1",
            Players = new List<string>
                          {
                              "p1", "p2", "p3", "p4", "p5", "p6", "p7", "p8"
                          }
        };

        MatchResult matchResult = new(validRoster, 9, 11, 123);

        // Act
        matchResult.RegisterSerie(new[]
        {
                new MatchTable(1, new MatchGame("p1", 269, 0, 0), new MatchGame("p2", 0, 0, 0), 0),
                new MatchTable(2, new MatchGame("p3", 0, 0, 0), new MatchGame("p4", 270, 0, 0), 0),
                new MatchTable(3, new MatchGame("p5", 0, 0, 0), new MatchGame("p6", 0, 0, 0), 0),
                new MatchTable(4, new MatchGame("p7", 300, 0, 0), new MatchGame("p8", 0, 0, 0), 0),
            });
        matchResult.RegisterSerie(new[]
        {
                new MatchTable(1, new MatchGame("p1", 169, 0, 0), new MatchGame("p2", 0, 0, 0), 0),
                new MatchTable(2, new MatchGame("p3", 0, 0, 0), new MatchGame("p4", 170, 0, 0), 0),
                new MatchTable(3, new MatchGame("p5", 0, 0, 0), new MatchGame("p6", 0, 0, 0), 0),
                new MatchTable(4, new MatchGame("p7", 200, 0, 0), new MatchGame("p8", 0, 0, 0), 0),
            });

        // Assert
        IDomainEvent[] changes = matchResult.GetUncommittedChanges();
        Assert.That(changes.Length, Is.EqualTo(5));
        AwardedMedal? medal1 = changes[2] as AwardedMedal;
        AwardedMedal? medal2 = changes[3] as AwardedMedal;
        Assert.NotNull(medal1);
        Assert.NotNull(medal2);
        Assert.That(medal1!.Player, Is.EqualTo("p4"));
        Assert.That(medal1.MedalType, Is.EqualTo(MedalType.PinsInSerie));
        Assert.That(medal1.Value, Is.EqualTo(270));
        Assert.That(medal2!.Player, Is.EqualTo("p7"));
        Assert.That(medal2.MedalType, Is.EqualTo(MedalType.PinsInSerie));
        Assert.That(medal2.Value, Is.EqualTo(300));
    }

    [Test]
    public void CannotAwardMedalsTwice()
    {
        // Arrange
        Roster validRoster = new(2012, 11, 1, "H", "A", "L", "A", new DateTime(2012, 2, 3), false, OilPatternInformation.Empty)
        {
            Id = "rosters-1",
            Players = new List<string>
                {
                    "p1", "p2", "p3", "p4", "p5", "p6", "p7", "p8"
                }
        };

        MatchResult matchResult = new(validRoster, 9, 11, 123);

        // Act
        matchResult.AwardMedals();

        // Assert
        _ = Assert.Throws<ApplicationException>(() => matchResult.AwardMedals());
    }

    [Test]
    public void CanClearMedals()
    {
        // Arrange
        Roster validRoster = new(2012, 11, 1, "H", "A", "L", "A", new DateTime(2012, 2, 3), false, OilPatternInformation.Empty)
        {
            Id = "rosters-1",
            Players = new List<string>
                          {
                              "p1", "p2", "p3", "p4", "p5", "p6", "p7", "p8"
                          }
        };

        MatchResult matchResult = new(validRoster, 9, 11, 123);

        // Act
        matchResult.ClearMedals();

        // Assert
        IDomainEvent[] changes = matchResult.GetUncommittedChanges();
        Assert.That(changes.Length, Is.EqualTo(2));
        IDomainEvent domainEvent = changes[1];
        Assert.IsAssignableFrom<ClearMedals>(domainEvent);
        Assert.That(123, Is.EqualTo(((ClearMedals)domainEvent).BitsMatchId));
    }

    [Test]
    public void CanClearAndReAwardMedals()
    {
        // Arrange
        Roster validRoster = new(2012, 11, 1, "H", "A", "L", "A", new DateTime(2012, 2, 3), false, OilPatternInformation.Empty)
        {
            Id = "rosters-1",
            Players = new List<string>
                          {
                              "p1", "p2", "p3", "p4", "p5", "p6", "p7", "p8"
                          }
        };

        MatchResult matchResult = new(validRoster, 9, 11, 123);
        matchResult.AwardMedals();

        // Act
        matchResult.ClearMedals();

        // Assert
        Assert.DoesNotThrow(matchResult.AwardMedals);
    }
}
