using System;
using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Areas.V2.Domain.Match;
using Snittlistan.Web.Areas.V2.Domain.Match.Events;
using Xunit;

namespace Snittlistan.Test.Domain
{
    public class MatchResultTest
    {
        private readonly Roster roster;

        public MatchResultTest()
        {
            this.roster = new Roster(2012, 11, "H", "L", "A", new DateTime(2012, 2, 3))
                         {
                             Id = "rosters-1"
                         };
        }

        [Fact]
        public void CanRegister()
        {
            // Act
            var matchResult = new MatchResult(roster, 9, 11, 123);

            // Assert
            var uncommittedChanges = matchResult.GetUncommittedChanges();
            Assert.Equal(1, uncommittedChanges.Length);
            var registered = uncommittedChanges[0] as MatchResultRegistered;
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
            var matchResult = new MatchResult(roster, 9, 11, 123);
            var newRoster = new Roster(2012, 10, "X", "Y", "Z", new DateTime(2012, 1, 1))
                                {
                                    Id = "rosters-2"
                                };

            // Act
            matchResult.Update(newRoster, 11, 9, 321);

            // Assert
            var uncommittedChanges = matchResult.GetUncommittedChanges();
            Assert.Equal(3, uncommittedChanges.Length);
            var changed = uncommittedChanges[1] as RosterChanged;
            Assert.NotNull(changed);
            Assert.Equal("rosters-1", changed.OldId);
            Assert.Equal("rosters-2", changed.NewId);

            var updated = uncommittedChanges[2] as MatchResultUpdated;
            Assert.NotNull(updated);
            Assert.Equal("rosters-2", updated.NewRosterId);
            Assert.Equal(11, updated.NewTeamScore);
            Assert.Equal(9, updated.NewOpponentScore);
            Assert.Equal(321, updated.NewBitsMatchId);
        }
    }
}
