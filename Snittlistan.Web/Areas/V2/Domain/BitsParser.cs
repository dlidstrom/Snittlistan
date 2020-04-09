using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using HtmlAgilityPack;
using Snittlistan.Web.Areas.V2.ReadModels;

namespace Snittlistan.Web.Areas.V2.Domain
{
    using Infrastructure;
    using Infrastructure.Bits.Contracts;

    public class BitsParser
    {
        private readonly Player[] players;

        public BitsParser(Player[] players)
        {
            this.players = players ?? throw new ArgumentNullException(nameof(players));
        }

        public static ParseHeaderResult ParseHeader(HeadInfo headInfo, int clubId)
        {
            var yearPart = headInfo.MatchDate.Substring(0, 4);
            var monthPart = headInfo.MatchDate.Substring(5, 2);
            var dayPart = headInfo.MatchDate.Substring(8, 2);
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
                new DateTime(int.Parse(yearPart), int.Parse(monthPart), int.Parse(dayPart)).AddHours(headInfo.MatchTime / 100).AddMinutes(headInfo.MatchTime % 100),
                headInfo.MatchHallName,
                new OilPatternInformation(
                    headInfo.MatchOilPatternName,
                    headInfo.MatchOilPatternId != 0 ? $"https://bits.swebowl.se/MiscDisplay/Oilpattern/{headInfo.MatchOilPatternId}" : string.Empty));
            return result;
        }

        public static ParseStandingsResult ParseStandings(string content)
        {
            var document = new HtmlDocument();
            document.LoadHtml(content);
            var documentNode = document.DocumentNode;
            var directLinkNode = documentNode.SelectSingleNode("//span[@id=\"MainContentPlaceHolder_Standings1_LabelDirectLink\"]");

            // table
            var currentRow = 0;
            var standings = new List<ParseStandingsResult.StandingsItem>();
            var currentGroup = (string)null;
            while (true)
            {
                var trNode = documentNode.SelectSingleNode(
                    $"//tr[@id=\"MainContentPlaceHolder_Standings1_ListViewBossStandings_TabelRowDivider_{currentRow}\"]");
                if (trNode == null) break;

                var nameNode = trNode.SelectSingleNode($"//span[@id=\"MainContentPlaceHolder_Standings1_ListViewBossStandings_LblStandingsTeamName_{currentRow}\"]");
                if (nameNode.HasClass("GroupText"))
                {
                    currentGroup = nameNode.InnerText;
                }
                else
                {
                    var matchesNode = trNode.SelectSingleNode($"//span[@id=\"MainContentPlaceHolder_Standings1_ListViewBossStandings_LblStandingsMatches_{currentRow}\"]");
                    var winNode = trNode.SelectSingleNode($"//span[@id=\"MainContentPlaceHolder_Standings1_ListViewBossStandings_LblStandingsWin_{currentRow}\"]");
                    var drawNode = trNode.SelectSingleNode($"//span[@id=\"MainContentPlaceHolder_Standings1_ListViewBossStandings_LblStandingsDraw_{currentRow}\"]");
                    var lossNode = trNode.SelectSingleNode($"//span[@id=\"MainContentPlaceHolder_Standings1_ListViewBossStandings_LblStandingsLoss_{currentRow}\"]");
                    var totalNode = trNode.SelectSingleNode($"//span[@id=\"MainContentPlaceHolder_Standings1_ListViewBossStandings_LblStandingsTotal_{currentRow}\"]");
                    var diffNode = trNode.SelectSingleNode($"//span[@id=\"MainContentPlaceHolder_Standings1_ListViewBossStandings_LblStandingsDiff_{currentRow}\"]");
                    var pointsNode = trNode.SelectSingleNode($"//span[@id=\"MainContentPlaceHolder_Standings1_ListViewBossStandings_LblStandingsPoints_{currentRow}\"]");
                    var dividerSolid = trNode.HasClass("DividerSolid");
                    var matches = int.Parse(matchesNode.InnerText);
                    var win = int.Parse(winNode.InnerText);
                    var draw = int.Parse(drawNode.InnerText);
                    var loss = int.Parse(lossNode.InnerText);
                    var diff = int.Parse(diffNode.InnerText);
                    var points = int.Parse(pointsNode.InnerText);
                    standings.Add(new ParseStandingsResult.StandingsItem
                    {
                        Group = currentGroup,
                        Name = nameNode.InnerText.Trim(),
                        Matches = matches,
                        Win = win,
                        Draw = draw,
                        Loss = loss,
                        Total = totalNode.InnerText,
                        Diff = diff,
                        Points = points,
                        DividerSolid = dividerSolid
                    });
                }

                currentRow++;
            }

            var result = new ParseStandingsResult(
                directLinkNode.InnerText,
                standings.ToArray());
            return result;
        }

        public static ParseMatchSchemeResult ParseMatchScheme(string content)
        {
            //
            var document = new HtmlDocument();
            document.LoadHtml(content);
            var documentNode = document.DocumentNode;
            var tableNode = documentNode.SelectSingleNode("//div[@id=\"MainContentPlaceHolder_MatchScheme1_PanelStandings\"]/table");

            var currentRow = 0;
            var matches = new List<ParseMatchSchemeResult.MatchItem>();
            while (true)
            {
                var matchFactNode = tableNode.SelectSingleNode($"//a[@id=\"MainContentPlaceHolder_MatchScheme1_ListViewMatchScheme_HyperLinkMatchFakta_{currentRow}\"]");
                if (matchFactNode == null) break;
                var matchDayNode = tableNode.SelectSingleNode($"//span[@id=\"MainContentPlaceHolder_MatchScheme1_ListViewMatchScheme_LblMatchDayFormatted_{currentRow}\"]");
                var isTurnRow = matchDayNode.InnerText.IndexOf("Omgång", StringComparison.Ordinal) >= 0;
                if (isTurnRow == false)
                {
                    var turnNode = tableNode.SelectSingleNode($"//input[@id=\"MainContentPlaceHolder_MatchScheme1_ListViewMatchScheme_HiddenRoundFormatted_{currentRow}\"]");
                    var matchTimeNode = tableNode.SelectSingleNode($"//span[@id=\"MainContentPlaceHolder_MatchScheme1_ListViewMatchScheme_LblMatchTimeFormatted_{currentRow}\"]");
                    var matchDateNode = tableNode.SelectSingleNode($"//input[@id=\"MainContentPlaceHolder_MatchScheme1_ListViewMatchScheme_HiddenFieldMatchDate_{currentRow}\"]");
                    var matchIdNode = tableNode.SelectSingleNode($"//input[@id=\"MainContentPlaceHolder_MatchScheme1_ListViewMatchScheme_HiddenFieldMatchId_{currentRow}\"]");
                    var teamsNode = tableNode.SelectSingleNode($"//a[@id=\"MainContentPlaceHolder_MatchScheme1_ListViewMatchScheme_HyperLinkMatchFakta_{currentRow}\"]");
                    var resultNode = tableNode.SelectSingleNode($"//span[@id=\"MainContentPlaceHolder_MatchScheme1_ListViewMatchScheme_LabelMatchResult_{currentRow}\"]");
                    var oilPatternNameNode = tableNode.SelectSingleNode($"//a[@id=\"MainContentPlaceHolder_MatchScheme1_ListViewMatchScheme_LabelMatchOilPattern_{currentRow}\"]");
                    var oilPatternIdNode = tableNode.SelectSingleNode($"//input[@id=\"MainContentPlaceHolder_MatchScheme1_ListViewMatchScheme_HiddenOilPatternId_{currentRow}\"]");
                    var locationNode = tableNode.SelectSingleNode($"//a[@id=\"MainContentPlaceHolder_MatchScheme1_ListViewMatchScheme_HyperLinkHall_{currentRow}\"]");

                    var turn = int.Parse(turnNode.Attributes["value"].Value.Replace("Omgång ", string.Empty));
                    var formattedDate = Regex.Replace(
                        matchDateNode.Attributes["value"].Value,
                        @"(?<date>\d{4}-\d\d-\d\d) 00:00:00",
                        @"${date}");
                    var date = DateTime.Parse($"{formattedDate}T{matchTimeNode.InnerText}");
                    var dateChanged = matchTimeNode.Attributes["style"].Value.IndexOf("color:Red", StringComparison.OrdinalIgnoreCase) >= 0;
                    var bitsMatchId = int.Parse(matchIdNode.Attributes["value"].Value);
                    var oilPatternId = int.Parse(oilPatternIdNode.Attributes["value"].Value);
                    var locationUrl = HttpUtility.HtmlDecode($"http://bits.swebowl.se/Matches/{locationNode.Attributes["href"].Value}");
                    var location = HttpUtility.HtmlDecode(locationNode.InnerText);
                    matches.Add(new ParseMatchSchemeResult.MatchItem
                    {
                        RowFromHtml = currentRow,
                        Turn = turn,
                        Date = date,
                        DateChanged = dateChanged,
                        BitsMatchId = bitsMatchId,
                        Teams = teamsNode.InnerText,
                        MatchResult = resultNode.InnerText,
                        OilPatternName = oilPatternNameNode.InnerText,
                        OilPatternId = oilPatternId,
                        Location = location,
                        LocationUrl = locationUrl
                    });
                }

                currentRow++;
            }

            return new ParseMatchSchemeResult(matches.ToArray());
        }

        public ParseResult Parse(BitsMatchResult bitsMatchResult, int clubId)
        {
            if (bitsMatchResult.HeadInfo.MatchFinished == false) return null;

            ParseResult parseResult;
            if (bitsMatchResult.HeadInfo.MatchHomeClubId == clubId)
            {
                var homeSeries = CreateSeries(
                    bitsMatchResult.HeadResultInfo.HomeHeadDetails,
                    bitsMatchResult.MatchScores,
                    x => GetPlayerId(x).Id,
                    0);
                var awaySeries = CreateSeries(
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
                var homeSeries = CreateSeries(
                    bitsMatchResult.HeadResultInfo.HomeHeadDetails,
                    bitsMatchResult.MatchScores,
                    x => GetPlayerId(x).Id,
                    2);
                var awaySeries = CreateSeries(
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
                    for (var j = 0; j < 4; j++)
                    {
                        var score1 = matchScores.Series[i].Boards[0 + offset].Scores[j];
                        var score2 = matchScores.Series[i].Boards[1 + offset].Scores[j];
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
                for (var i = 0; i < bitsMatchResult.HeadResultInfo.HomeHeadDetails.Length; i++)
                {
                    var homeHeadDetail = bitsMatchResult.HeadResultInfo.HomeHeadDetails[i];
                    var games = new List<ResultSeries4ReadModel.Game>();
                    foreach (var boardScore in bitsMatchResult.MatchScores.Series[i].Boards[0].Scores)
                    {
                        var game = new ResultSeries4ReadModel.Game
                        {
                            Player = GetPlayerId(boardScore.PlayerName).Id,
                            Pins = boardScore.ScoreScore,
                            Score = boardScore.LaneScore
                        };

                        games.Add(game);
                    }

                    var score = homeHeadDetail.TeamRp - games.Sum(x => x.Score);
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
                for (var i = 0; i < bitsMatchResult.HeadResultInfo.AwayHeadDetails.Length; i++)
                {
                    var awayHeadDetail = bitsMatchResult.HeadResultInfo.AwayHeadDetails[i];
                    var games = new List<ResultSeries4ReadModel.Game>();
                    foreach (var boardScore in bitsMatchResult.MatchScores.Series[i].Boards[1].Scores)
                    {
                        var game = new ResultSeries4ReadModel.Game
                        {
                            Player = GetPlayerId(boardScore.PlayerName).Id,
                            Pins = boardScore.ScoreScore,
                            Score = boardScore.LaneScore
                        };

                        games.Add(game);
                    }

                    var score = awayHeadDetail.TeamRp - games.Sum(x => x.Score);
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
            var split = name.Split(' ');
            var lastName = split.Last();
            var initial = name[0];
            var q = from player in players
                    where player.Name.EndsWith(lastName)
                    where player.Name.StartsWith(new string(initial, 1))
                    select player;
            var p = q.SingleOrDefault();
            if (p == null)
                throw new ApplicationException($"No player with name {name} was found");
            return p;
        }
    }
}