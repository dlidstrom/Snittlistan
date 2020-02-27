using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using HtmlAgilityPack;
using Snittlistan.Web.Areas.V2.ReadModels;

namespace Snittlistan.Web.Areas.V2.Domain
{
    using Contracts;

    public class BitsParser
    {
        private readonly Player[] players;

        public BitsParser(Player[] players)
        {
            this.players = players ?? throw new ArgumentNullException(nameof(players));
        }

        private enum Team
        {
            Home = 0,
            Away = 2
        }

        public static ParseHeaderResult ParseHeader(string content)
        {
            if (content == null) throw new ArgumentNullException(nameof(content));

            var headInfo = HeadInfo.FromJson(content);

            var yearPart = headInfo.MatchDate.Substring(0, 4);
            var monthPart = headInfo.MatchDate.Substring(5, 2);
            var dayPart = headInfo.MatchDate.Substring(8, 2);
            var result = new ParseHeaderResult(
                headInfo.MatchHomeTeamName,
                headInfo.MatchAwayTeamName,
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

        public ParseResult Parse(string content, string team)
        {
            var document = new HtmlDocument();
            document.LoadHtml(content);
            var documentNode = document.DocumentNode;

            // make sure match is finished
            var finishedNode = documentNode.SelectSingleNode("//span[@id='MainContentPlaceHolder_MatchInfo_LabelFinished']");
            if (finishedNode != null)
            {
                return null;
            }

            // find which team we should import
            var homeTeamLabel =
                documentNode.SelectSingleNode("//span[@id='MainContentPlaceHolder_MatchInfo_LabelHomeTeam']");
            var awayTeamLabel =
                documentNode.SelectSingleNode("//span[@id='MainContentPlaceHolder_MatchInfo_LabelAwayTeam']");

            var homeTeamName = homeTeamLabel.InnerText;
            if (team == homeTeamName)
                return ExtractTeam(documentNode, Team.Home, Team.Away);
            var awayTeamName = awayTeamLabel.InnerText;
            if (team == awayTeamName)
                return ExtractTeam(documentNode, Team.Away, Team.Home);

            // try alternate name
            var teamPrefix = team.Split(' ').First();
            if (homeTeamName.StartsWith(teamPrefix) && awayTeamName.StartsWith(teamPrefix))
                throw new ApplicationException($"Could not find team with prefix {teamPrefix}");

            if (homeTeamName.StartsWith(teamPrefix) && awayTeamName.StartsWith(teamPrefix) == false)
                return ExtractTeam(documentNode, Team.Home, Team.Away);
            if (awayTeamName.StartsWith(teamPrefix) && homeTeamName.StartsWith(teamPrefix) == false)
                return ExtractTeam(documentNode, Team.Away, Team.Home);

            var message = $"No team with name {team} was found (homeTeamName = {homeTeamName}, awayTeamName = {awayTeamName})";
            throw new ApplicationException(message);
        }

        public Parse4Result Parse4(string content, string team)
        {
            var document = new HtmlDocument();
            document.LoadHtml(content);
            var documentNode = document.DocumentNode;

            // make sure match is finished
            var finishedNode = documentNode.SelectSingleNode("//span[@id='MainContentPlaceHolder_MatchInfo_LabelFinished']");
            if (finishedNode != null)
            {
                return null;
            }

            // find which team we should import
            var homeTeamLabel =
                documentNode.SelectSingleNode("//span[@id='MainContentPlaceHolder_MatchInfo_LabelHomeTeam']");
            var awayTeamLabel =
                documentNode.SelectSingleNode("//span[@id='MainContentPlaceHolder_MatchInfo_LabelAwayTeam']");

            var homeTeamName = homeTeamLabel.InnerText;
            if (team == homeTeamName)
                return ExtractTeam4(documentNode, Team.Home, Team.Away);
            var awayTeamName = awayTeamLabel.InnerText;
            if (team == awayTeamName)
                return ExtractTeam4(documentNode, Team.Away, Team.Home);

            // try alternate name
            var teamPrefix = team.Split(' ')
                .First();
            if (homeTeamName.StartsWith(teamPrefix) && awayTeamName.StartsWith(teamPrefix))
                throw new ApplicationException($"Could not find team with prefix {teamPrefix}");

            if (homeTeamName.StartsWith(teamPrefix) && awayTeamName.StartsWith(teamPrefix) == false)
                return ExtractTeam4(documentNode, Team.Home, Team.Away);
            if (awayTeamName.StartsWith(teamPrefix) && homeTeamName.StartsWith(teamPrefix) == false)
                return ExtractTeam4(documentNode, Team.Away, Team.Home);

            throw new ApplicationException($"No team with name {team} was found");
        }

        private ParseResult ExtractTeam(HtmlNode documentNode, Team team, Team away)
        {
            var tableNode = documentNode.SelectSingleNode("//table[@id='MainContentPlaceHolder_MatchFact1_TableMatch']");

            // adjust for header and footer rows
            var tableRows = documentNode.SelectNodes("//table[@id='MainContentPlaceHolder_MatchHead1_matchinfo']//tr");
            var numberOfSeries = tableRows.Count - 2;
            if (numberOfSeries < 1 || numberOfSeries > 4)
            {
                return null;
            }

            var teamSeries = ExtractSeriesForTeam(team, numberOfSeries, tableNode, s => GetPlayerId(s).Id);
            var opponentSeries = ExtractSeriesForTeam(away, numberOfSeries, tableNode, s => s);

            var teamScoreNode = documentNode.SelectSingleNode($"//span[@id='MainContentPlaceHolder_MatchHead1_LblSumPoints{team}']");
            var teamScore = int.Parse(teamScoreNode.InnerText);

            var awayScoreNode = documentNode.SelectSingleNode($"//span[@id='MainContentPlaceHolder_MatchHead1_LblSumPoints{away}']");
            var awayScore = int.Parse(awayScoreNode.InnerText);

            var turnNode = documentNode.SelectSingleNode("//td[@id='MainContentPlaceHolder_MatchInfo_LabelRound']");
            var turn = int.Parse(turnNode.InnerText.Replace("Omgång ", string.Empty));

            return new ParseResult(teamScore, awayScore, turn, teamSeries, opponentSeries);
        }

        private ResultSeriesReadModel.Serie[] ExtractSeriesForTeam(
            Team team,
            int numberOfSeries,
            HtmlNode tableNode,
            Func<string, string> getPlayerId)
        {
            var series = new List<ResultSeriesReadModel.Serie>();
            for (var serieNumber = 1; serieNumber <= numberOfSeries; serieNumber++)
            {
                var serie = new ResultSeriesReadModel.Serie();
                var tables = new List<ResultSeriesReadModel.Table>();
                for (var tableNumber = 1; tableNumber <= 4; tableNumber++)
                {
                    var name1 = tableNode.SelectSingleNode(
                        $"//span[@id='MainContentPlaceHolder_MatchFact1_lblSerie{serieNumber}Table{tableNumber}Order{1 + (int)team}Player']");
                    var name2 = tableNode.SelectSingleNode(
                        $"//span[@id='MainContentPlaceHolder_MatchFact1_lblSerie{serieNumber}Table{tableNumber}Order{2 + (int)team}Player']");
                    var res1Node = tableNode.SelectSingleNode(
                        $"//span[@id='MainContentPlaceHolder_MatchFact1_lblSerie{serieNumber}Table{tableNumber}Order{1 + (int)team}Result']");
                    var res2Node = tableNode.SelectSingleNode(
                        $"//span[@id='MainContentPlaceHolder_MatchFact1_lblSerie{serieNumber}Table{tableNumber}Order{2 + (int)team}Result']");
                    var scoreNode = tableNode.SelectSingleNode(
                        $"//span[@id='MainContentPlaceHolder_MatchFact1_lblSerie{serieNumber}Table{tableNumber}Order{1 + (int)team}Total']");
                    var score = int.Parse(scoreNode.InnerText);
                    int.TryParse(res1Node.InnerText, out var res1);
                    int.TryParse(res2Node.InnerText, out var res2);
                    var playerForGame1 = getPlayerId(name1.InnerText);
                    var playerForGame2 = getPlayerId(name2.InnerText);
                    var table = new ResultSeriesReadModel.Table
                    {
                        Score = score,
                        Game1 = new ResultSeriesReadModel.Game
                        {
                            Pins = res1,
                            Player = playerForGame1
                        },
                        Game2 = new ResultSeriesReadModel.Game
                        {
                            Pins = res2,
                            Player = playerForGame2
                        }
                    };
                    tables.Add(table);
                }

                serie.Tables = tables;
                series.Add(serie);
            }

            return series.ToArray();
        }

        private Parse4Result ExtractTeam4(HtmlNode documentNode, Team team, Team away)
        {
            var series = new List<ResultSeries4ReadModel.Serie>();
            var tableNode = documentNode.SelectSingleNode("//table[@id='MainContentPlaceHolder_MatchFact1_TableMatch']");
            var order = team == Team.Home ? 1 : 2;

            for (var serieNumber = 1; serieNumber <= 4; serieNumber++)
            {
                var games = new List<ResultSeries4ReadModel.Game>();
                for (var tableNumber = 1; tableNumber <= 4; tableNumber++)
                {
                    var playerNameNode = tableNode.SelectSingleNode(
                        $"//span[@id='MainContentPlaceHolder_MatchFact1_lblSerie{serieNumber}Table{tableNumber}Order{order}Player']");
                    var playerPinsNode = tableNode.SelectSingleNode(
                        $"//span[@id='MainContentPlaceHolder_MatchFact1_lblSerie{serieNumber}Table{tableNumber}Order{order}Result']");
                    var opponentPinsNode = tableNode.SelectSingleNode(
                        $"//span[@id='MainContentPlaceHolder_MatchFact1_lblSerie{serieNumber}Table{tableNumber}Order{3 - order}Result']");
                    var playerPins = int.Parse(playerPinsNode.InnerText);
                    var opponentPins = int.Parse(opponentPinsNode.InnerText);
                    var score = playerPins > opponentPins ? 1 : 0;
                    var game = new ResultSeries4ReadModel.Game
                    {
                        Score = score,
                        Pins = playerPins,
                        Player = GetPlayerId(playerNameNode.InnerText).Id
                    };
                    games.Add(game);
                }

                var serie = new ResultSeries4ReadModel.Serie
                {
                    Games = games
                };
                series.Add(serie);
            }

            var teamScoreNode = documentNode.SelectSingleNode($"//span[@id='MainContentPlaceHolder_MatchHead1_LblSumPoints{team}']");
            var teamScore = int.Parse(teamScoreNode.InnerText);

            var awayScoreNode = documentNode.SelectSingleNode($"//span[@id='MainContentPlaceHolder_MatchHead1_LblSumPoints{away}']");
            var awayScore = int.Parse(awayScoreNode.InnerText);

            var turnNode = documentNode.SelectSingleNode("//td[@id='MainContentPlaceHolder_MatchInfo_LabelRound']");
            var turn = int.Parse(turnNode.InnerText.Replace("Omgång ", string.Empty));

            return new Parse4Result(teamScore, awayScore, turn, series.ToArray());
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