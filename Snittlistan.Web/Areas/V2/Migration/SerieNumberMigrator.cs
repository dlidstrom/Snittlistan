using System;
using System.Collections.Generic;
using EventStoreLite;
using Snittlistan.Web.Areas.V2.Domain.Match.Events;

namespace Snittlistan.Web.Areas.V2.Migration
{
    public class SerieNumberMigrator : IEventMigratorWithResults
    {
        private readonly Dictionary<string, int> currentSerieNumberForMatch = new Dictionary<string, int>();
        private readonly HashSet<string> numberOfMatchesMigrated = new HashSet<string>();
        private int numberOfSeriesMigrated;

        public IEnumerable<IDomainEvent> Migrate(IDomainEvent @event, string aggregateId)
        {
            var matchResultRegistered = @event as MatchResultRegistered;
            if (matchResultRegistered != null)
            {
                currentSerieNumberForMatch[aggregateId] = 1;
                return new[] { @event };
            }

            var serieRegistered = @event as SerieRegistered;
            if (serieRegistered != null)
            {
                serieRegistered.MatchSerie.SerieNumber = currentSerieNumberForMatch[aggregateId]++;
                serieRegistered.MatchSerie.Table1.TableNumber = 1;
                serieRegistered.MatchSerie.Table2.TableNumber = 2;
                serieRegistered.MatchSerie.Table3.TableNumber = 3;
                serieRegistered.MatchSerie.Table4.TableNumber = 4;
                numberOfSeriesMigrated++;
                numberOfMatchesMigrated.Add(aggregateId);
                return new[] { serieRegistered };
            }

            return new[] { @event };
        }

        public DateTime DefinedOn()
        {
            return new DateTime(2017, 5, 22, 10, 36, 00);
        }

        public string GetResults()
        {
            var s = string.Format(
                "<div class=\"row\"><div class=\"span12\"><h1>SerieNumberMigrator</h1><p>Migrated {0} matches and {1} series</p></div></div>",
                numberOfMatchesMigrated.Count,
                numberOfSeriesMigrated);
            return s;
        }
    }
}