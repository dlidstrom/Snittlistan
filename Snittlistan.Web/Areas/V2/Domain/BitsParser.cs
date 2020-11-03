namespace Snittlistan.Web.Areas.V2.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Infrastructure;
    using Infrastructure.Bits.Contracts;
    using Snittlistan.Web.Areas.V2.ReadModels;

    public class BitsParser
    {
        private readonly Player[] players;

        public BitsParser(Player[] players)
        {
            this.players = players ?? throw new ArgumentNullException(nameof(players));
        }

        public static ParseHeaderResult ParseHeader(HeadInfo headInfo, int clubId)
        {
            string homeTeamName;
            string awayTeamName;
            if (headInfo.MatchHomeClubId == clubId)
            {
                homeTeamName = headInfo.MatchHomeTeamAlias;
                awayTeamName = headInfo.MatchAwayTeamAlias;
            }
            else if (headInfo.MatchAwayClubId == clubId)
            {
                homeTeamName = headInfo.MatchAwayTeamAlias;
                awayTeamName = headInfo.MatchHomeTeamAlias;
            }
            else
            {
                throw new Exception($"Unmatched clubId: {clubId}");
            }

            var result = new ParseHeaderResult(
                homeTeamName,
                awayTeamName,
                headInfo.MatchRoundId,
                headInfo.MatchDate.ToDateTime(headInfo.MatchTime),
                headInfo.MatchHallName,
                OilPatternInformation.Create(
                    headInfo.MatchOilPatternName,
                    headInfo.MatchOilPatternId));
            return result;
        }

        public ParseResult Parse(BitsMatchResult bitsMatchResult, int clubId)
        {
            if (bitsMatchResult.HeadInfo.MatchFinished == false) return null;

            ParseResult parseResult;
            if (bitsMatchResult.HeadInfo.MatchHomeClubId == clubId)
            {
                List<ResultSeriesReadModel.Serie> homeSeries = CreateSeries(
                    bitsMatchResult.HeadResultInfo.HomeHeadDetails,
                    bitsMatchResult.MatchScores,
                    x => GetPlayerId(x).Id,
                    0);
                List<ResultSeriesReadModel.Serie> awaySeries = CreateSeries(
                    bitsMatchResult.HeadResultInfo.HomeHeadDetails,
                    bitsMatchResult.MatchScores,
                    x => x,
                    2);
                parseResult = new ParseResult(
                    bitsMatchResult.HeadResultInfo.MatchHeadHomeTotalRp,
                    bitsMatchResult.HeadResultInfo.MatchHeadAwayTotalRp,
                    bitsMatchResult.HeadInfo.MatchRoundId,
                    homeSeries.ToArray(),
                    awaySeries.ToArray());
            }
            else if (bitsMatchResult.HeadInfo.MatchAwayClubId == clubId)
            {
                List<ResultSeriesReadModel.Serie> homeSeries = CreateSeries(
                    bitsMatchResult.HeadResultInfo.HomeHeadDetails,
                    bitsMatchResult.MatchScores,
                    x => GetPlayerId(x).Id,
                    2);
                List<ResultSeriesReadModel.Serie> awaySeries = CreateSeries(
                    bitsMatchResult.HeadResultInfo.HomeHeadDetails,
                    bitsMatchResult.MatchScores,
                    x => x,
                    0);
                parseResult = new ParseResult(
                    bitsMatchResult.HeadResultInfo.MatchHeadAwayTotalRp,
                    bitsMatchResult.HeadResultInfo.MatchHeadHomeTotalRp,
                    bitsMatchResult.HeadInfo.MatchRoundId,
                    homeSeries.ToArray(),
                    awaySeries.ToArray());
            }
            else
            {
                throw new Exception($"No club matching {clubId}");
            }

            return parseResult;

            List<ResultSeriesReadModel.Serie> CreateSeries(
                HeadDetail[] headDetails,
                MatchScores matchScores,
                Func<string, string> getPlayer,
                int offset)
            {
                var series = new List<ResultSeriesReadModel.Serie>();
                for (int i = 0; i < headDetails.Length; i++)
                {
                    var tables = new List<ResultSeriesReadModel.Table>();
                    for (int j = 0; j < 4; j++)
                    {
                        Score score1 = matchScores.Series[i].Boards[0 + offset].Scores[j];
                        Score score2 = matchScores.Series[i].Boards[1 + offset].Scores[j];
                        var game1 = new ResultSeriesReadModel.Game
                        {
                            Pins = score1.ScoreScore,
                            Player = getPlayer.Invoke(score1.PlayerName)
                        };
                        var game2 = new ResultSeriesReadModel.Game
                        {
                            Pins = score2.ScoreScore,
                            Player = getPlayer.Invoke(score2.PlayerName)
                        };
                        var table = new ResultSeriesReadModel.Table
                        {
                            Score = score1.LaneScore,
                            Game1 = game1,
                            Game2 = game2
                        };
                        tables.Add(table);
                    }

                    var serie = new ResultSeriesReadModel.Serie
                    {
                        Tables = tables
                    };
                    series.Add(serie);
                }

                return series;
            }
        }

        public Parse4Result Parse4(BitsMatchResult bitsMatchResult, int clubId)
        {
            if (bitsMatchResult.HeadInfo.MatchFinished == false) return null;

            Parse4Result parse4Result;
            if (bitsMatchResult.HeadInfo.MatchHomeClubId == clubId)
            {
                var series = new List<ResultSeries4ReadModel.Serie>();
                for (int i = 0; i < bitsMatchResult.HeadResultInfo.HomeHeadDetails.Length; i++)
                {
                    HeadDetail homeHeadDetail = bitsMatchResult.HeadResultInfo.HomeHeadDetails[i];
                    var games = new List<ResultSeries4ReadModel.Game>();
                    foreach (Score boardScore in bitsMatchResult.MatchScores.Series[i].Boards[0].Scores)
                    {
                        var game = new ResultSeries4ReadModel.Game
                        {
                            Player = GetPlayerId(boardScore.PlayerName).Id,
                            Pins = boardScore.ScoreScore,
                            Score = boardScore.LaneScore
                        };

                        games.Add(game);
                    }

                    int score = homeHeadDetail.TeamRp - games.Sum(x => x.Score);
                    var serie = new ResultSeries4ReadModel.Serie
                    {
                        Score = score,
                        Games = games
                    };
                    series.Add(serie);
                }

                parse4Result = new Parse4Result(
                    bitsMatchResult.HeadResultInfo.MatchHeadHomeTotalRp,
                    bitsMatchResult.HeadResultInfo.MatchHeadAwayTotalRp,
                    bitsMatchResult.HeadInfo.MatchRoundId,
                    series.ToArray());
            }
            else if (bitsMatchResult.HeadInfo.MatchAwayClubId == clubId)
            {
                var series = new List<ResultSeries4ReadModel.Serie>();
                for (int i = 0; i < bitsMatchResult.HeadResultInfo.AwayHeadDetails.Length; i++)
                {
                    HeadDetail awayHeadDetail = bitsMatchResult.HeadResultInfo.AwayHeadDetails[i];
                    var games = new List<ResultSeries4ReadModel.Game>();
                    foreach (Score boardScore in bitsMatchResult.MatchScores.Series[i].Boards[1].Scores)
                    {
                        var game = new ResultSeries4ReadModel.Game
                        {
                            Player = GetPlayerId(boardScore.PlayerName).Id,
                            Pins = boardScore.ScoreScore,
                            Score = boardScore.LaneScore
                        };

                        games.Add(game);
                    }

                    int score = awayHeadDetail.TeamRp - games.Sum(x => x.Score);
                    var serie = new ResultSeries4ReadModel.Serie
                    {
                        Score = score,
                        Games = games
                    };
                    series.Add(serie);
                }

                parse4Result = new Parse4Result(
                    bitsMatchResult.HeadResultInfo.MatchHeadAwayTotalRp,
                    bitsMatchResult.HeadResultInfo.MatchHeadHomeTotalRp,
                    bitsMatchResult.HeadInfo.MatchRoundId,
                    series.ToArray());
            }
            else
            {
                throw new Exception($"No clubs matching {clubId}");
            }

            return parse4Result;
        }

        private Player GetPlayerId(string name)
        {
            string[] split = name.Split(' ');
            string lastName = split.Last();
            char initial = name[0];
            IEnumerable<Player> q = from player in players
                                    where player.Name.EndsWith(lastName)
                                    where player.Name.StartsWith(new string(initial, 1))
                                    select player;
            Player p = q.SingleOrDefault();
            if (p == null)
            {
                var ex = new ApplicationException("Player not found");
                ex.Data["name"] = name;
                ex.Data["lastName"] = lastName;
                ex.Data["initial"] = initial;
                ex.Data["players"] = string.Join(",", players.Select(x => x.Name));
                throw ex;
            }

            return p;
        }
    }
}
