namespace Snittlistan.Infrastructure.Indexes
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Raven.Client.Indexes;
    using Snittlistan.Models;

    public class Player_ByMatch : AbstractMultiMapIndexCreationTask<Player_ByMatch.Result>
    {
        public Player_ByMatch()
        {
            AddMap<Match4x4>(matches => from match in matches
                                        from team in match.Teams
                                        from serie in team.Series
                                        from game in serie.Games
                                        where game.Player != null
                                        select new
                                        {
                                            Type = "4x4",
                                            Player = game.Player,
                                            MatchId = match.Id,
                                            BitsMatchId = 0,
                                            Location = match.Location,
                                            Team = team.Name,
                                            Date = match.Date,
                                            Score = game.Score,
                                            Pins = game.Pins,
                                            Max = game.Pins,
                                            Series = 1,
                                            Strikes = game.Strikes,
                                            Misses = game.Misses
                                        });

            AddMap<Match8x4>(matches => from match in matches
                                        from team in match.Teams
                                        from serie in team.Series
                                        from table in serie.Tables
                                        from game in table.Games
                                        where game.Player != null
                                        select new
                                        {
                                            Type = "8x4",
                                            Player = game.Player,
                                            MatchId = match.Id,
                                            BitsMatchId = match.BitsMatchId,
                                            Location = match.Location,
                                            Team = team.Name,
                                            Date = match.Date,
                                            Score = table.Score,
                                            Pins = game.Pins,
                                            Max = game.Pins,
                                            Series = 1,
                                            Strikes = game.Strikes,
                                            Misses = game.Misses
                                        });

            Reduce = results => from result in results
                                group result by new { result.Type, result.Player, result.MatchId, result.BitsMatchId, result.Location, result.Date, result.Team } into games
                                select new
                                {
                                    Type = games.Key.Type,
                                    Player = games.Key.Player,
                                    MatchId = games.Key.MatchId,
                                    BitsMatchId = (int)games.Key.BitsMatchId,
                                    Location = games.Key.Location,
                                    Team = games.Key.Team,
                                    Date = games.Key.Date,
                                    Score = games.Sum(g => g.Score),
                                    Pins = games.Sum(g => g.Pins),
                                    Max = games.Max(g => g.Max),
                                    Series = games.Sum(g => g.Series),
                                    Strikes = games.Sum(g => g.Strikes),
                                    Misses = games.Sum(g => g.Misses)
                                };
        }

        public class Result
        {
            public string Type { get; set; }
            public string Player { get; set; }
            public string MatchId { get; set; }
            public int BitsMatchId { get; set; }
            public string Location { get; set; }
            public string Team { get; set; }

            [DataType(DataType.Date)]
            public DateTimeOffset Date { get; set; }
            public double Score { get; set; }
            public double Pins { get; set; }
            public int Max { get; set; }
            public int Series { get; set; }
            public double Strikes { get; set; }
            public double Misses { get; set; }
        }
    }
}