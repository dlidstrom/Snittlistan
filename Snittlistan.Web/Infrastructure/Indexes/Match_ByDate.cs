namespace Snittlistan.Web.Infrastructure.Indexes
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using Raven.Abstractions.Indexing;
    using Raven.Client.Indexes;

    using Snittlistan.Web.Areas.V1.Models;

    public class Match_ByDate : AbstractMultiMapIndexCreationTask<Match_ByDate.Result>
    {
        public Match_ByDate()
        {
            this.AddMap<Match4x4>(matches => from match in matches
                                             select new
                                             {
                                                 match.Id,
                                                 Type = "4x4",
                                                 match.Date,
                                                 match.Location,
                                                 HomeTeamName = match.Teams.ElementAt(0).Name,
                                                 HomeTeamScore = match.Teams.ElementAt(0).Score,
                                                 AwayTeamName = match.Teams.ElementAt(1).Name,
                                                 AwayTeamScore = match.Teams.ElementAt(1).Score
                                             });

            this.AddMap<Match8x4>(matches => from match in matches
                                             select new
                                             {
                                                 match.Id,
                                                 Type = "8x4",
                                                 match.Date,
                                                 match.Location,
                                                 HomeTeamName = match.Teams.ElementAt(0).Name,
                                                 HomeTeamScore = match.Teams.ElementAt(0).Score,
                                                 AwayTeamName = match.Teams.ElementAt(1).Name,
                                                 AwayTeamScore = match.Teams.ElementAt(1).Score
                                             });

            this.Store(x => x.Type, FieldStorage.Yes);
            this.Store(x => x.HomeTeamName, FieldStorage.Yes);
            this.Store(x => x.HomeTeamScore, FieldStorage.Yes);
            this.Store(x => x.AwayTeamName, FieldStorage.Yes);
            this.Store(x => x.AwayTeamScore, FieldStorage.Yes);
        }

        public class Result
        {
            public int Id { get; set; }

            public string Type { get; set; }

            [Display(Name = "Datum"), DataType(DataType.Date)]
            public DateTimeOffset Date { get; set; }

            public string HomeTeamName { get; set; }
            public int HomeTeamScore { get; set; }

            public string AwayTeamName { get; set; }
            public int AwayTeamScore { get; set; }

            public string Location { get; set; }
        }
    }
}