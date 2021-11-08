#nullable enable

namespace Snittlistan.Web.Infrastructure.Indexes
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Raven.Client.Indexes;
    using Snittlistan.Web.Areas.V1.Models;

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
                                            game.Player,
                                            match.Location,
                                            match.Date,
                                            game.Score,
                                            game.Pins,
                                            game.Strikes,
                                            game.Misses,
                                            BitsMatchId = 0,
                                            Type = "4x4",
                                            MatchId = match.Id,
                                            Team = team.Name,
                                            Max = game.Pins,
                                            HasStats = game.Strikes != null ? 1 : 0,
                                            Series = 1
                                        });

            AddMap<Match8x4>(matches => from match in matches
                                        from team in match.Teams
                                        from serie in team.Series
                                        from table in serie.Tables
                                        from game in table.Games
                                        where game.Player != null
                                        select new
                                        {
                                            game.Player,
                                            match.Location,
                                            match.Date,
                                            table.Score,
                                            game.Pins,
                                            game.Strikes,
                                            game.Misses,
                                            match.BitsMatchId,
                                            Type = "8x4",
                                            MatchId = match.Id,
                                            Team = team.Name,
                                            Max = game.Pins,
                                            HasStats = 1,
                                            Series = 1
                                        });

            Reduce = results => from result in results
                                group result by new
                                {
                                    result.Type,
                                    result.Player,
                                    result.MatchId,
                                    result.BitsMatchId,
                                    result.Location,
                                    result.Date,
                                    result.Team
                                }
                                    into games
                                select new Result
                                {
                                    Type = games.Key.Type,
                                    Player = games.Key.Player,
                                    MatchId = games.Key.MatchId,
                                    BitsMatchId = games.Key.BitsMatchId,
                                    Location = games.Key.Location,
                                    Team = games.Key.Team,
                                    Date = games.Key.Date,
                                    Score = games.Sum(g => g.Score),
                                    Pins = games.Sum(g => g.Pins),
                                    Max = games.Max(g => g.Max),
                                    HasStats = games.Sum(x => x.HasStats),
                                    Series = games.Sum(g => g.Series),
                                    Strikes = games.Sum(g => g.Strikes),
                                    Misses = games.Sum(g => g.Misses)
                                };
        }

        /// <summary>
        /// Represents a single game result.
        /// </summary>
        public class Result
        {
            public string Type { get; set; } = null!;

            public string Player { get; set; } = null!;

            public string MatchId { get; set; } = null!;

            public int BitsMatchId { get; set; }

            public string Location { get; set; } = null!;

            public string Team { get; set; } = null!;

            [DataType(DataType.Date)]
            public DateTimeOffset Date { get; set; }

            public double Score { get; set; }

            public double Pins { get; set; }

            public int Max { get; set; }

            public int HasStats { get; set; }

            public int Series { get; set; }

            public double Strikes { get; set; }

            public double Misses { get; set; }
        }
    }
}
