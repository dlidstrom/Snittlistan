namespace Snittlistan.Infrastructure.Indexes
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Raven.Abstractions.Indexing;
    using Raven.Client.Indexes;
    using Snittlistan.Models;

    public class Match_ByDate : AbstractMultiMapIndexCreationTask<Match_ByDate.Result>
    {
        public Match_ByDate()
        {
            AddMap<Match4x4>(matches => from match in matches
                                        select new
                                        {
                                            Id = match.Id,
                                            Date = match.Date,
                                            Location = match.Location,
                                            HomeTeamName = match.Teams.ElementAt(0).Name,
                                            HomeTeamScore = match.Teams.ElementAt(0).Score,
                                            AwayTeamName = match.Teams.ElementAt(1).Name,
                                            AwayTeamScore = match.Teams.ElementAt(1).Score
                                        });

            AddMap<Match8x4>(matches => from match in matches
                                        select new
                                        {
                                            Id = match.Id,
                                            Date = match.Date,
                                            Location = match.Location,
                                            HomeTeamName = match.Teams.ElementAt(0).Name,
                                            HomeTeamScore = match.Teams.ElementAt(0).Score,
                                            AwayTeamName = match.Teams.ElementAt(1).Name,
                                            AwayTeamScore = match.Teams.ElementAt(1).Score
                                        });

            Store(x => x.HomeTeamName, FieldStorage.Yes);
            Store(x => x.HomeTeamScore, FieldStorage.Yes);
            Store(x => x.AwayTeamName, FieldStorage.Yes);
            Store(x => x.AwayTeamScore, FieldStorage.Yes);
        }

        public class Result
        {
            public int Id { get; set; }

            [Display(Name = "Datum"), DataType(DataType.Date)]
            public DateTime Date { get; set; }

            public string HomeTeamName { get; set; }
            public int HomeTeamScore { get; set; }

            public string AwayTeamName { get; set; }
            public int AwayTeamScore { get; set; }

            public string Location { get; set; }
        }
    }
}