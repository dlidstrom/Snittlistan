namespace Snittlistan.Web.Areas.V2.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web;
    using HtmlAgilityPack;
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

        public static ParseStandingsResult ParseStandings(string content)
        {
            var document = new HtmlDocument();
            document.LoadHtml(content);
            HtmlNode documentNode = document.DocumentNode;
            HtmlNode directLinkNode = documentNode.SelectSingleNode("//span[@id=\"MainContentPlaceHolder_Standings1_LabelDirectLink\"]");

            // table
            var currentRow = 0;
            var standings = new List<ParseStandingsResult.StandingsItem>();
            var currentGroup = (string)null;
            while (true)
            {
                HtmlNode trNode = documentNode.SelectSingleNode(
                    $"//tr[@id=\"MainContentPlaceHolder_Standings1_ListViewBossStandings_TabelRowDivider_{currentRow}\"]");
                if (trNode == null) break;

                HtmlNode nameNode = trNode.SelectSingleNode($"//span[@id=\"MainContentPlaceHolder_Standings1_ListViewBossStandings_LblStandingsTeamName_{currentRow}\"]");
                if (nameNode.HasClass("GroupText"))
                {
                    currentGroup = nameNode.InnerText;
                }
                else
                {
                    HtmlNode matchesNode = trNode.SelectSingleNode($"//span[@id=\"MainContentPlaceHolder_Standings1_ListViewBossStandings_LblStandingsMatches_{currentRow}\"]");
                    HtmlNode winNode = trNode.SelectSingleNode($"//span[@id=\"MainContentPlaceHolder_Standings1_ListViewBossStandings_LblStandingsWin_{currentRow}\"]");
                    HtmlNode drawNode = trNode.SelectSingleNode($"//span[@id=\"MainContentPlaceHolder_Standings1_ListViewBossStandings_LblStandingsDraw_{currentRow}\"]");
                    HtmlNode lossNode = trNode.SelectSingleNode($"//span[@id=\"MainContentPlaceHolder_Standings1_ListViewBossStandings_LblStandingsLoss_{currentRow}\"]");
                    HtmlNode totalNode = trNode.SelectSingleNode($"//span[@id=\"MainContentPlaceHolder_Standings1_ListViewBossStandings_LblStandingsTotal_{currentRow}\"]");
                    HtmlNode diffNode = trNode.SelectSingleNode($"//span[@id=\"MainContentPlaceHolder_Standings1_ListViewBossStandings_LblStandingsDiff_{currentRow}\"]");
                    HtmlNode pointsNode = trNode.SelectSingleNode($"//span[@id=\"MainContentPlaceHolder_Standings1_ListViewBossStandings_LblStandingsPoints_{currentRow}\"]");
                    bool dividerSolid = trNode.HasClass("DividerSolid");
                    int matches = int.Parse(matchesNode.InnerText);
                    int win = int.Parse(winNode.InnerText);
                    int draw = int.Parse(drawNode.InnerText);
                    int loss = int.Parse(lossNode.InnerText);
                    int diff = int.Parse(diffNode.InnerText);
                    int points = int.Parse(pointsNode.InnerText);
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
            HtmlNode documentNode = document.DocumentNode;
            HtmlNode tableNode = documentNode.SelectSingleNode("//div[@id=\"MainContentPlaceHolder_MatchScheme1_PanelStandings\"]/table");

            var currentRow = 0;
            var matches = new List<ParseMatchSchemeResult.MatchItem>();
            while (true)
            {
                HtmlNode matchFactNode = tableNode.SelectSingleNode($"//a[@id=\"MainContentPlaceHolder_MatchScheme1_ListViewMatchScheme_HyperLinkMatchFakta_{currentRow}\"]");
                if (matchFactNode == null) break;
                HtmlNode matchDayNode = tableNode.SelectSingleNode($"//span[@id=\"MainContentPlaceHolder_MatchScheme1_ListViewMatchScheme_LblMatchDayFormatted_{currentRow}\"]");
                bool isTurnRow = matchDayNode.InnerText.IndexOf("Omgång", StringComparison.Ordinal) >= 0;
                if (isTurnRow == false)
                {
                    HtmlNode turnNode = tableNode.SelectSingleNode($"//input[@id=\"MainContentPlaceHolder_MatchScheme1_ListViewMatchScheme_HiddenRoundFormatted_{currentRow}\"]");
                    HtmlNode matchTimeNode = tableNode.SelectSingleNode($"//span[@id=\"MainContentPlaceHolder_MatchScheme1_ListViewMatchScheme_LblMatchTimeFormatted_{currentRow}\"]");
                    HtmlNode matchDateNode = tableNode.SelectSingleNode($"//input[@id=\"MainContentPlaceHolder_MatchScheme1_ListViewMatchScheme_HiddenFieldMatchDate_{currentRow}\"]");
                    HtmlNode matchIdNode = tableNode.SelectSingleNode($"//input[@id=\"MainContentPlaceHolder_MatchScheme1_ListViewMatchScheme_HiddenFieldMatchId_{currentRow}\"]");
                    HtmlNode teamsNode = tableNode.SelectSingleNode($"//a[@id=\"MainContentPlaceHolder_MatchScheme1_ListViewMatchScheme_HyperLinkMatchFakta_{currentRow}\"]");
                    HtmlNode resultNode = tableNode.SelectSingleNode($"//span[@id=\"MainContentPlaceHolder_MatchScheme1_ListViewMatchScheme_LabelMatchResult_{currentRow}\"]");
                    HtmlNode oilPatternNameNode = tableNode.SelectSingleNode($"//a[@id=\"MainContentPlaceHolder_MatchScheme1_ListViewMatchScheme_LabelMatchOilPattern_{currentRow}\"]");
                    HtmlNode oilPatternIdNode = tableNode.SelectSingleNode($"//input[@id=\"MainContentPlaceHolder_MatchScheme1_ListViewMatchScheme_HiddenOilPatternId_{currentRow}\"]");
                    HtmlNode locationNode = tableNode.SelectSingleNode($"//a[@id=\"MainContentPlaceHolder_MatchScheme1_ListViewMatchScheme_HyperLinkHall_{currentRow}\"]");

                    int turn = int.Parse(turnNode.Attributes["value"].Value.Replace("Omgång ", string.Empty));
                    string formattedDate = Regex.Replace(
                        matchDateNode.Attributes["value"].Value,
                        @"(?<date>\d{4}-\d\d-\d\d) 00:00:00",
                        @"${date}");
                    DateTime date = DateTime.Parse($"{formattedDate}T{matchTimeNode.InnerText}");
                    bool dateChanged = matchTimeNode.Attributes["style"].Value.IndexOf("color:Red", StringComparison.OrdinalIgnoreCase) >= 0;
                    int bitsMatchId = int.Parse(matchIdNode.Attributes["value"].Value);
                    int oilPatternId = int.Parse(oilPatternIdNode.Attributes["value"].Value);
                    string locationUrl = HttpUtility.HtmlDecode($"http://bits.swebowl.se/Matches/{locationNode.Attributes["href"].Value}");
                    string location = HttpUtility.HtmlDecode(locationNode.InnerText);
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
                for (var i = 0; i < headDetails.Length; i++)
                {
                    var tables = new List<ResultSeriesReadModel.Table>();
                    for (var j = 0; j < 4; j++)
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
                for (var i = 0; i < bitsMatchResult.HeadResultInfo.HomeHeadDetails.Length; i++)
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
                for (var i = 0; i < bitsMatchResult.HeadResultInfo.AwayHeadDetails.Length; i++)
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
                throw new ApplicationException($"No player with name {name} was found");
            return p;
        }
    }
}