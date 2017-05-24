using System;
using System.Collections.Generic;
using EventStoreLite;
using Snittlistan.Web.Areas.V2.Domain.Match.Events;

namespace Snittlistan.Web.Areas.V2.Migration
{
    public class RosterIdMigrator : IEventMigratorWithResults
    {
        private readonly Dictionary<string, string> aggregateIdToRosterId
            = new Dictionary<string, string>();

        private int numberOfSeriesMigrated;

        public IEnumerable<IDomainEvent> Migrate(IDomainEvent @event, string aggregateId)
        {
            var matchResultRegistered = @event as MatchResultRegistered;
            if (matchResultRegistered != null)
            {
                aggregateIdToRosterId[aggregateId] = matchResultRegistered.RosterId;
                return new[] { @event };
            }

            var serieRegistered = @event as SerieRegistered;
            if (serieRegistered != null)
            {
                serieRegistered.RosterId = aggregateIdToRosterId[aggregateId];
                numberOfSeriesMigrated++;
                return new[] { serieRegistered };
            }

            return new[] { @event };
        }

        public DateTime DefinedOn()
        {
            return new DateTime(2017, 05, 19, 10, 50, 00);
        }

        public string GetResults()
        {
            var s = string.Format(
                "<div class=\"row\"><div class=\"span12\"><h1>RosterIdMigrator</h1><p>Migrated {0} series</p></div></div>",
                numberOfSeriesMigrated);
            return s;
        }
    }
}