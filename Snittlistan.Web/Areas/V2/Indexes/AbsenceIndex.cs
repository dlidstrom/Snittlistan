namespace Snittlistan.Web.Areas.V2.Indexes
{
    using System;
    using System.Linq;

    using Raven.Abstractions.Indexing;
    using Raven.Client.Indexes;

    using Snittlistan.Web.Areas.V2.Models;

    public class AbsenceIndex : AbstractIndexCreationTask<Absence, AbsenceIndex.Result>
    {
        public AbsenceIndex()
        {
            this.Map = absences => from absence in absences
                                   select new
                                   {
                                       absence.Id,
                                       absence.From,
                                       absence.To,
                                       PlayerName = this.LoadDocument<Player>(absence.Player).Name
                                   };

            this.Store(x => x.PlayerName, FieldStorage.Yes);
        }

        public class Result
        {
            public string Id { get; set; }

            public DateTime From { get; set; }

            public DateTime To { get; set; }

            public string PlayerName { get; set; }
        }
    }
}