namespace Snittlistan.Test.Domain
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;
    using Snittlistan.Web.Areas.V2.Domain;
    using Snittlistan.Web.Areas.V2.Domain.Match;
    using Snittlistan.Web.Areas.V2.Domain.Match.Events;

    [TestFixture]
    public class MatchResult4Test
    {
        private readonly Roster roster;

        public MatchResult4Test()
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
            var matchResult = new MatchResult4(roster, 9, 11, 123);

            // Assert
            EventStoreLite.IDomainEvent[] uncommittedChanges = matchResult.GetUncommittedChanges();
            Assert.That(uncommittedChanges.Length, Is.EqualTo(1));
            var registered = uncommittedChanges[0] as MatchResult4Registered;
            Assert.NotNull(registered);
            Assert.That(registered.RosterId, Is.EqualTo("rosters-1"));
            Assert.That(registered.TeamScore, Is.EqualTo(9));
            Assert.That(registered.OpponentScore, Is.EqualTo(11));
            Assert.That(registered.BitsMatchId, Is.EqualTo(123));
        }

        [Test]
        public void MustRegisterResultBeforeSeries()
        {
            // Arrange
            Assert.Throws<ArgumentNullException>(() => new MatchResult4(null, 9, 11, 123));
        }

        [Test]
        public void RosterCannotHaveThreePlayers()
        {
            // Arrange
            var invalidRoster = new Roster(2012, 11, 1, "H", "A", "L", "A", new DateTime(2012, 2, 3), false, OilPatternInformation.Empty)
            {
                Id = "rosters-1",
                Players = new List<string>
                {
                    "p1", "p2", "p3"
                }
            };

            var matchResult = new MatchResult4(invalidRoster, 9, 11, 123);

            // Act
            var matchSerie = new MatchSerie4(
                1,
                new List<MatchGame4>
                {
                    new MatchGame4("p1", 0, 0), new MatchGame4("p2", 0, 0),
                    new MatchGame4("p3", 0, 0), new MatchGame4("p4", 0, 0)
                });

            // Assert
            MatchException ex = Assert.Throws<MatchException>(() => matchResult.RegisterSerie(matchSerie));
            Assert.That(ex.Message, Is.EqualTo("Roster must have 4 or 5 players when registering results"));
        }

        [Test]
        public void RosterCanHaveFourPlayers()
        {
            // Arrange
            var validRoster = new Roster(2012, 11, 1, "H", "A", "L", "A", new DateTime(2012, 2, 3), false, OilPatternInformation.Empty)
            {
                Id = "rosters-1",
                Players = new List<string>
                {
                    "p1", "p2", "p3", "p4"
                }
            };

            var matchResult = new MatchResult4(validRoster, 9, 11, 123);

            // Act
            var matchSerie = new MatchSerie4(
                1,
                new List<MatchGame4>
                {
                    new MatchGame4("p1", 0, 0), new MatchGame4("p2", 0, 0),
                    new MatchGame4("p3", 0, 0), new MatchGame4("p4", 0, 0)
                });

            // Assert
            Assert.DoesNotThrow(() => matchResult.RegisterSerie(matchSerie));
            Assert.That(matchResult.GetUncommittedChanges()[1].GetType(), Is.EqualTo(typeof(Serie4Registered)));
        }

        [Test]
        public void RosterCanHaveFivePlayers()
        {
            // Arrange
            var validRoster = new Roster(2012, 11, 1, "H", "A", "L", "A", new DateTime(2012, 2, 3), false, OilPatternInformation.Empty)
            {
                Id = "rosters-1",
                Players = new List<string>
                {
                    "p1", "p2", "p3", "p4", "p5"
                }
            };

            var matchResult = new MatchResult4(validRoster, 9, 11, 123);

            // Act
            var matchSerie = new MatchSerie4(
                1,
                new List<MatchGame4>
                {
                    new MatchGame4("p1", 0, 0), new MatchGame4("p2", 0, 0),
                    new MatchGame4("p3", 0, 0), new MatchGame4("p4", 0, 0)
                });

            // Assert
            Assert.DoesNotThrow(() => matchResult.RegisterSerie(matchSerie));
        }

        [Test]
        public void RosterCannotHaveSixPlayers()
        {
            // Arrange
            var invalidRoster = new Roster(2012, 11, 1, "H", "A", "L", "A", new DateTime(2012, 2, 3), false, OilPatternInformation.Empty)
            {
                Id = "rosters-1",
                Players = new List<string>
                {
                    "p1", "p2", "p3", "p4", "p5", "p6"
                }
            };

            var matchResult = new MatchResult4(invalidRoster, 9, 11, 123);

            // Act
            var matchSerie = new MatchSerie4(
                1,
                new List<MatchGame4>
                {
                    new MatchGame4("p1", 0, 0), new MatchGame4("p2", 0, 0),
                    new MatchGame4("p3", 0, 0), new MatchGame4("p4", 0, 0)
                });

            // Assert
            MatchException ex = Assert.Throws<MatchException>(() => matchResult.RegisterSerie(matchSerie));
            Assert.That(ex.Message, Is.EqualTo("Roster must have 4 or 5 players when registering results"));
        }

        [Test]
        public void CanOnlyRegisterSeriesWithValidPlayer()
        {
            // Arrange
            var validRoster = new Roster(2012, 11, 1, "H", "A", "L", "A", new DateTime(2012, 2, 3), false, OilPatternInformation.Empty)
            {
                Id = "rosters-1",
                Players = new List<string>
                {
                    "p1", "p2", "p3", "p4"
                }
            };
            var matchResult = new MatchResult4(validRoster, 9, 11, 123);

            // Act
            var matchSerie = new MatchSerie4(
                1,
                new List<MatchGame4>
                {
                    new MatchGame4("p1", 0, 0), new MatchGame4("p2", 0, 0),
                    new MatchGame4("p3", 0, 0), new MatchGame4("p4", 0, 0)
                });

            // Assert
            Assert.DoesNotThrow(() => matchResult.RegisterSerie(matchSerie));
        }

        [Test]
        public void CanRegisterWithReserve()
        {
            // Arrange
            var validRoster = new Roster(2012, 11, 1, "H", "A", "L", "A", new DateTime(2012, 2, 3), false, OilPatternInformation.Empty)
            {
                Id = "rosters-1",
                Players = new List<string>
                {
                    "p1", "p2", "p3", "p4", "p5"
                }
            };
            var matchResult = new MatchResult4(validRoster, 9, 11, 123);

            // Act
            var matchSerie = new MatchSerie4(
                1,
                new List<MatchGame4>
                {
                    new MatchGame4("p1", 0, 0), new MatchGame4("p2", 0, 0),
                    new MatchGame4("p3", 0, 0), new MatchGame4("p4", 0, 0)
                });

            // Assert
            Assert.DoesNotThrow(() => matchResult.RegisterSerie(matchSerie));
        }

        [Test]
        public void CanNotRegisterInvalidPlayer()
        {
            // Arrange
            var validRoster = new Roster(2012, 11, 1, "H", "A", "L", "A", new DateTime(2012, 2, 3), false, OilPatternInformation.Empty)
            {
                Id = "rosters-1",
                Players = new List<string>
                {
                    "p1", "p2", "p3", "p4"
                }
            };
            var matchResult = new MatchResult4(validRoster, 9, 11, 123);

            // Act
            var matchSerie = new MatchSerie4(
                1,
                new List<MatchGame4>
                {
                    new MatchGame4("p1", 0, 0), new MatchGame4("p2", 0, 0),
                    new MatchGame4("invalid-id", 0, 0), new MatchGame4("p4", 0, 0)
                });

            // Assert
            MatchException ex = Assert.Throws<MatchException>(() => matchResult.RegisterSerie(matchSerie));
            Assert.That(ex.Message, Is.EqualTo("Can only register players from roster"));
        }

        [Test]
        public void ValidScore()
        {
            Assert.DoesNotThrow(() => new MatchResult4(roster, 0, 0, 0));
            Assert.DoesNotThrow(() => new MatchResult4(roster, 0, 20, 0));
            Assert.DoesNotThrow(() => new MatchResult4(roster, 20, 0, 0));
            Assert.DoesNotThrow(() => new MatchResult4(roster, 10, 10, 0));
        }

        [Test]
        public void InvalidScore()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new MatchResult4(roster, -1, 0, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => new MatchResult4(roster, 21, 0, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => new MatchResult4(roster, 0, -1, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => new MatchResult4(roster, 0, 21, 0));
            Assert.Throws<ArgumentException>(() => new MatchResult4(roster, 10, 11, 0));
        }

        [Test]
        public void MedalFor4Score()
        {
            // Arrange
            var validRoster = new Roster(2012, 11, 1, "H", "A", "L", "A", new DateTime(2012, 2, 3), false, OilPatternInformation.Empty)
            {
                Id = "rosters-1",
                Players = new List<string>
                {
                    "p1", "p2", "p3", "p4"
                }
            };

            var matchResult = new MatchResult4(validRoster, 9, 11, 123);

            // Act
            for (int i = 0; i < 4; i++)
            {
                var matchSerie = new MatchSerie4(
                    i + 1,
                    new List<MatchGame4>
                    {
                        new MatchGame4("p1", 0, 0),
                        new MatchGame4("p2", 1, 0),
                        new MatchGame4("p3", 0, 0),
                        new MatchGame4("p4", 0, 0)
                    });
                matchResult.RegisterSerie(matchSerie);
            }

            // Assert
            EventStoreLite.IDomainEvent[] changes = matchResult.GetUncommittedChanges();
            Assert.That(changes.Length, Is.EqualTo(6));
            var medal = changes[5] as AwardedMedal;
            Assert.NotNull(medal);
            Assert.That(medal.Player, Is.EqualTo("p2"));
            Assert.That(medal.MedalType, Is.EqualTo(MedalType.TotalScore));
            Assert.That(medal.Value, Is.EqualTo(4));
        }

        [Test]
        public void MedalFor270OrMore()
        {
            // Arrange
            var validRoster = new Roster(2012, 11, 1, "H", "A", "L", "A", new DateTime(2012, 2, 3), false, OilPatternInformation.Empty)
            {
                Id = "rosters-1",
                Players = new List<string>
                {
                    "p1", "p2", "p3", "p4"
                }
            };

            var matchResult = new MatchResult4(validRoster, 9, 11, 123);

            // Act
            var matchSerie = new MatchSerie4(
                1,
                new List<MatchGame4>
                {
                    new MatchGame4("p1", 0, 269), new MatchGame4("p2", 0, 270),
                    new MatchGame4("p3", 0, 0), new MatchGame4("p4", 0, 300)
                });
            matchResult.RegisterSerie(matchSerie);

            // Assert
            EventStoreLite.IDomainEvent[] changes = matchResult.GetUncommittedChanges();
            Assert.That(changes.Length, Is.EqualTo(4));
            var medal1 = changes[2] as AwardedMedal;
            var medal2 = changes[3] as AwardedMedal;
            Assert.NotNull(medal1);
            Assert.NotNull(medal2);
            Assert.That(medal1.Player, Is.EqualTo("p2"));
            Assert.That(medal1.MedalType, Is.EqualTo(MedalType.PinsInSerie));
            Assert.That(medal1.Value, Is.EqualTo(270));
            Assert.That(medal2.Player, Is.EqualTo("p4"));
            Assert.That(medal2.MedalType, Is.EqualTo(MedalType.PinsInSerie));
            Assert.That(medal2.Value, Is.EqualTo(300));
        }

        [Test]
        public void CannotAwardMedalsTwice()
        {
            // Arrange
            var validRoster = new Roster(2012, 11, 1, "H", "A", "L", "A", new DateTime(2012, 2, 3), false, OilPatternInformation.Empty)
            {
                Id = "rosters-1",
                Players = new List<string>
                {
                    "p1", "p2", "p3", "p4"
                }
            };

            var matchResult = new MatchResult4(validRoster, 9, 11, 123);

            // Act
            matchResult.AwardMedals();

            // Assert
            Assert.Throws<ApplicationException>(() => matchResult.AwardMedals());
        }

        [Test]
        public void OnlyAwardMedalsOnce()
        {
            // Arrange
            var validRoster = new Roster(2012, 11, 1, "H", "A", "L", "A", new DateTime(2012, 2, 3), false, OilPatternInformation.Empty)
            {
                Id = "rosters-1",
                Players = new List<string>
                {
                    "p1", "p2", "p3", "p4"
                }
            };

            var matchResult = new MatchResult4(validRoster, 9, 11, 123);

            // Act
            matchResult.RegisterSerie(new MatchSerie4(
                1,
                new List<MatchGame4>
                {
                    new MatchGame4("p1", 0, 269),
                    new MatchGame4("p2", 0, 270),
                    new MatchGame4("p3", 0, 0),
                    new MatchGame4("p4", 0, 300)
                }));
            matchResult.RegisterSerie(new MatchSerie4(
                2,
                new List<MatchGame4>
                {
                    new MatchGame4("p1", 0, 169),
                    new MatchGame4("p2", 0, 170),
                    new MatchGame4("p3", 0, 0),
                    new MatchGame4("p4", 0, 200)
                }));

            // Assert
            EventStoreLite.IDomainEvent[] changes = matchResult.GetUncommittedChanges();
            Assert.That(changes.Length, Is.EqualTo(5));
            var medal1 = changes[2] as AwardedMedal;
            var medal2 = changes[3] as AwardedMedal;
            Assert.NotNull(medal1);
            Assert.NotNull(medal2);
            Assert.That(medal1.Player, Is.EqualTo("p2"));
            Assert.That(medal1.MedalType, Is.EqualTo(MedalType.PinsInSerie));
            Assert.That(medal1.Value, Is.EqualTo(270));
            Assert.That(medal2.Player, Is.EqualTo("p4"));
            Assert.That(medal2.MedalType, Is.EqualTo(MedalType.PinsInSerie));
            Assert.That(medal2.Value, Is.EqualTo(300));
        }

        [Test]
        public void CanClearMedals()
        {
            // Arrange
            var validRoster = new Roster(2012, 11, 1, "H", "A", "L", "A", new DateTime(2012, 2, 3), false, OilPatternInformation.Empty)
            {
                Id = "rosters-1",
                Players = new List<string>
                {
                    "p1", "p2", "p3", "p4"
                }
            };

            var matchResult = new MatchResult4(validRoster, 9, 11, 123);

            // Act
            matchResult.ClearMedals();

            // Assert
            EventStoreLite.IDomainEvent[] changes = matchResult.GetUncommittedChanges();
            Assert.That(changes.Length, Is.EqualTo(2));
            EventStoreLite.IDomainEvent domainEvent = changes[1];
            Assert.IsAssignableFrom<ClearMedals>(domainEvent);
            Assert.That(123, Is.EqualTo(((ClearMedals)domainEvent).BitsMatchId));
        }

        [Test]
        public void CanClearAndReAwardMedals()
        {
            // Arrange
            var validRoster = new Roster(2012, 11, 1, "H", "A", "L", "A", new DateTime(2012, 2, 3), false, OilPatternInformation.Empty)
            {
                Id = "rosters-1",
                Players = new List<string>
                {
                    "p1", "p2", "p3", "p4"
                }
            };

            var matchResult = new MatchResult4(validRoster, 9, 11, 123);
            matchResult.AwardMedals();

            // Act
            matchResult.ClearMedals();

            // Assert
            Assert.DoesNotThrow(matchResult.AwardMedals);
        }
    }
}