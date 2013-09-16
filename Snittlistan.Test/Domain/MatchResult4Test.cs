using System;
using System.Collections.Generic;
using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Areas.V2.Domain.Match;
using Snittlistan.Web.Areas.V2.Domain.Match.Events;
using Xunit;

namespace Snittlistan.Test.Domain
{
    public class MatchResult4Test
    {
        private readonly Roster roster;

        public MatchResult4Test()
        {
            roster = new Roster(2012, 11, "H", "L", "A", new DateTime(2012, 2, 3), false)
            {
                Id = "rosters-1"
            };
        }

        [Fact]
        public void CanRegister()
        {
            // Act
            var matchResult = new MatchResult4(roster, 9, 11, 123);

            // Assert
            var uncommittedChanges = matchResult.GetUncommittedChanges();
            Assert.Equal(1, uncommittedChanges.Length);
            var registered = uncommittedChanges[0] as MatchResult4Registered;
            Assert.NotNull(registered);
            Assert.Equal("rosters-1", registered.RosterId);
            Assert.Equal(9, registered.TeamScore);
            Assert.Equal(11, registered.OpponentScore);
            Assert.Equal(123, registered.BitsMatchId);
        }

        [Fact]
        public void CanUpdate()
        {
            // Arrange
            var matchResult = new MatchResult4(roster, 9, 11, 123);
            var newRoster = new Roster(2012, 10, "X", "Y", "Z", new DateTime(2012, 1, 1), false)
            {
                Id = "rosters-2"
            };

            // Act
            matchResult.Update(newRoster, 11, 9, 321);

            // Assert
            var uncommittedChanges = matchResult.GetUncommittedChanges();
            Assert.Equal(3, uncommittedChanges.Length);
            var changed = uncommittedChanges[1] as Roster4Changed;
            Assert.NotNull(changed);
            Assert.Equal("rosters-1", changed.OldId);
            Assert.Equal("rosters-2", changed.NewId);

            var updated = uncommittedChanges[2] as MatchResult4Updated;
            Assert.NotNull(updated);
            Assert.Equal("rosters-2", updated.NewRosterId);
            Assert.Equal(11, updated.NewTeamScore);
            Assert.Equal(9, updated.NewOpponentScore);
            Assert.Equal(321, updated.NewBitsMatchId);
        }

        [Fact]
        public void MustRegisterResultBeforeSeries()
        {
            // Arrange
            Assert.Throws<ArgumentNullException>(() => new MatchResult4(null, 9, 11, 123));
        }

        [Fact]
        public void RosterCannotHaveThreePlayers()
        {
            // Arrange
            var invalidRoster = new Roster(2012, 11, "H", "L", "A", new DateTime(2012, 2, 3), false)
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
                new List<MatchGame4>
                {
                    new MatchGame4("p1", 0, 0), new MatchGame4("p2", 0, 0),
                    new MatchGame4("p3", 0, 0), new MatchGame4("p4", 0, 0)
                });

            // Assert
            var ex = Assert.Throws<MatchException>(() => matchResult.RegisterSerie(matchSerie));
            Assert.Equal("Roster must have 4 or 5 players when registering results", ex.Message);
        }

        [Fact]
        public void RosterCanHaveFourPlayers()
        {
            // Arrange
            var invalidRoster = new Roster(2012, 11, "H", "L", "A", new DateTime(2012, 2, 3), false)
            {
                Id = "rosters-1",
                Players = new List<string>
                {
                    "p1", "p2", "p3", "p4"
                }
            };

            var matchResult = new MatchResult4(invalidRoster, 9, 11, 123);

            // Act
            var matchSerie = new MatchSerie4(
                new List<MatchGame4>
                {
                    new MatchGame4("p1", 0, 0), new MatchGame4("p2", 0, 0),
                    new MatchGame4("p3", 0, 0), new MatchGame4("p4", 0, 0)
                });

            // Assert
            Assert.DoesNotThrow(() => matchResult.RegisterSerie(matchSerie));
            Assert.Equal(typeof(Serie4Registered), matchResult.GetUncommittedChanges()[1].GetType());
        }

        [Fact]
        public void RosterCanHaveFivePlayers()
        {
            // Arrange
            var validRoster = new Roster(2012, 11, "H", "L", "A", new DateTime(2012, 2, 3), false)
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
                new List<MatchGame4>
                {
                    new MatchGame4("p1", 0, 0), new MatchGame4("p2", 0, 0),
                    new MatchGame4("p3", 0, 0), new MatchGame4("p4", 0, 0)
                });

            // Assert
            Assert.DoesNotThrow(() => matchResult.RegisterSerie(matchSerie));
        }

        [Fact]
        public void RosterCannotHaveSixPlayers()
        {
            // Arrange
            var invalidRoster = new Roster(2012, 11, "H", "L", "A", new DateTime(2012, 2, 3), false)
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
                new List<MatchGame4>
                {
                    new MatchGame4("p1", 0, 0), new MatchGame4("p2", 0, 0),
                    new MatchGame4("p3", 0, 0), new MatchGame4("p4", 0, 0)
                });

            // Assert
            var ex = Assert.Throws<MatchException>(() => matchResult.RegisterSerie(matchSerie));
            Assert.Equal("Roster must have 4 or 5 players when registering results", ex.Message);
        }

        [Fact]
        public void CanOnlyRegisterSeriesWithValidPlayer()
        {
            // Arrange
            var validRoster = new Roster(2012, 11, "H", "L", "A", new DateTime(2012, 2, 3), false)
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
                new List<MatchGame4>
                {
                    new MatchGame4("p1", 0, 0), new MatchGame4("p2", 0, 0),
                    new MatchGame4("p3", 0, 0), new MatchGame4("p4", 0, 0)
                });

            // Assert
            Assert.DoesNotThrow(() => matchResult.RegisterSerie(matchSerie));
        }

        [Fact]
        public void CanRegisterWithReserve()
        {
            // Arrange
            var validRoster = new Roster(2012, 11, "H", "L", "A", new DateTime(2012, 2, 3), false)
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
                new List<MatchGame4>
                {
                    new MatchGame4("p1", 0, 0), new MatchGame4("p2", 0, 0),
                    new MatchGame4("p3", 0, 0), new MatchGame4("p4", 0, 0)
                });

            // Assert
            Assert.DoesNotThrow(() => matchResult.RegisterSerie(matchSerie));
        }

        [Fact]
        public void CanNotRegisterInvalidPlayer()
        {
            // Arrange
            var validRoster = new Roster(2012, 11, "H", "L", "A", new DateTime(2012, 2, 3), false)
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
                new List<MatchGame4>
                {
                    new MatchGame4("p1", 0, 0), new MatchGame4("p2", 0, 0),
                    new MatchGame4("invalid-id", 0, 0), new MatchGame4("p4", 0, 0)
                });

            // Assert
            var ex = Assert.Throws<MatchException>(() => matchResult.RegisterSerie(matchSerie));
            Assert.Equal("Can only register players from roster", ex.Message);
        }

        [Fact]
        public void ValidScore()
        {
            Assert.DoesNotThrow(() => new MatchResult4(roster, 0, 0, 0));
            Assert.DoesNotThrow(() => new MatchResult4(roster, 0, 20, 0));
            Assert.DoesNotThrow(() => new MatchResult4(roster, 20, 0, 0));
            Assert.DoesNotThrow(() => new MatchResult4(roster, 10, 10, 0));
        }

        [Fact]
        public void InvalidScore()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new MatchResult4(roster, -1, 0, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => new MatchResult4(roster, 21, 0, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => new MatchResult4(roster, 0, -1, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => new MatchResult4(roster, 0, 21, 0));
            Assert.Throws<ArgumentException>(() => new MatchResult4(roster, 10, 11, 0));
        }

        [Fact]
        public void ValidUpdate()
        {
            var matchResult = new MatchResult4(roster, 0, 0, 0);
            Assert.DoesNotThrow(() => matchResult.Update(roster, 0, 0, 0));
            Assert.DoesNotThrow(() => matchResult.Update(roster, 0, 20, 0));
            Assert.DoesNotThrow(() => matchResult.Update(roster, 20, 0, 0));
            Assert.DoesNotThrow(() => matchResult.Update(roster, 10, 10, 0));
        }

        [Fact]
        public void InvalidUpdate()
        {
            var matchResult = new MatchResult4(roster, 0, 0, 0);
            Assert.Throws<ArgumentNullException>(() => matchResult.Update(null, 0, 0, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => matchResult.Update(roster, -1, 0, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => matchResult.Update(roster, 21, 0, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => matchResult.Update(roster, 0, -1, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => matchResult.Update(roster, 0, 21, 0));
            Assert.Throws<ArgumentException>(() => matchResult.Update(roster, 10, 11, 0));
        }
    }
}