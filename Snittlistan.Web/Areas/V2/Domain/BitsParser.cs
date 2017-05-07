using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using Snittlistan.Web.Areas.V2.ReadModels;

namespace Snittlistan.Web.Areas.V2.Domain
{
    public class BitsParser
    {
        private readonly Player[] players;

        public BitsParser(Player[] players)
        {
            if (players == null) throw new ArgumentNullException("players");
            this.players = players;
        }

        private enum Team
        {
            Home = 0,
            Away = 2
        }

        public static ParseHeaderResult ParseHeader(string content, string[] possibleTeams)
        {
            if (content == null) throw new ArgumentNullException("content");
            if (possibleTeams == null) throw new ArgumentNullException("possibleTeams");

            var document = new HtmlDocument();
            document.LoadHtml(content);

            // find team
            var documentNode = document.DocumentNode;
            var homeTeamNode = documentNode.SelectSingleNode("//span[@id='MainContentPlaceHolder_MatchInfo_LabelHomeTeam']");
            var homeTeamName = homeTeamNode.InnerText;
            var awayTeamNode = documentNode.SelectSingleNode("//span[@id='MainContentPlaceHolder_MatchInfo_LabelAwayTeam']");
            var awayTeamName = awayTeamNode.InnerText;
            var dateNode = documentNode.SelectSingleNode("//span[@id='MainContentPlaceHolder_MatchInfo_LabelMatchDate']");
            var dateText = dateNode.InnerText;
            var locationNode = documentNode.SelectSingleNode("//span[@id='MainContentPlaceHolder_MatchInfo_LabelHallName']");
            var locationText = locationNode.InnerText;

            var homeTeamNameSplit = homeTeamName.Split();
            string homeTeam = null;
            foreach (var possibleTeam in possibleTeams)
            {
                var equals = possibleTeam.Equals(homeTeamName, StringComparison.InvariantCultureIgnoreCase);
                if (equals)
                {
                    homeTeam = possibleTeam;
                    break;
                }

                var contains = possibleTeam.IndexOf(homeTeamNameSplit.First(), StringComparison.InvariantCultureIgnoreCase);
                if (contains >= 0)
                {
                    homeTeam = possibleTeam;
                }
            }

            if (homeTeam != null)
                return new ParseHeaderResult(homeTeam, awayTeamName, DateTime.Parse(dateText), locationText);

            var awayTeamNameSplit = awayTeamName.Split();
            string awayTeam = null;
            foreach (var possibleTeam in possibleTeams)
            {
                var equals = possibleTeam.Equals(awayTeamName, StringComparison.InvariantCultureIgnoreCase);
                if (equals)
                {
                    awayTeam = possibleTeam;
                    break;
                }

                var contains = possibleTeam.IndexOf(awayTeamNameSplit.First(), StringComparison.InvariantCultureIgnoreCase);
                if (contains >= 0)
                {
                    awayTeam = possibleTeam;
                }
            }

            if (awayTeam == null)
            {
                var message = string.Format(
                    "No matching teams found (homeTeamName = {0}, awayTeamName = {1}, possible = {2})",
                    homeTeamName,
                    awayTeamName,
                    string.Join(", ", possibleTeams));
                throw new ApplicationException(message);
            }

            return new ParseHeaderResult(awayTeam, homeTeamName, DateTime.Parse(dateText), locationText);
        }

        public ParseResult Parse(string content, string team)
        {
            var document = new HtmlDocument();
            document.LoadHtml(content);

            // find which team we should import
            var documentNode = document.DocumentNode;
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
                throw new ApplicationException(string.Format("Could not find team with prefix {0}", teamPrefix));

            if (homeTeamName.StartsWith(teamPrefix) && awayTeamName.StartsWith(teamPrefix) == false)
                return ExtractTeam(documentNode, Team.Home, Team.Away);
            if (awayTeamName.StartsWith(teamPrefix) && homeTeamName.StartsWith(teamPrefix) == false)
                return ExtractTeam(documentNode, Team.Away, Team.Home);

            var message = string.Format(
                "No team with name {0} was found (homeTeamName = {1}, awayTeamName = {2})",
                team,
                homeTeamName,
                awayTeamName);
            throw new ApplicationException(message);
        }

        public Parse4Result Parse4(string content, string team)
        {
            var document = new HtmlDocument();
            document.LoadHtml(content);

            // find which team we should import
            var documentNode = document.DocumentNode;
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
                throw new ApplicationException(string.Format("Could not find team with prefix {0}", teamPrefix));

            if (homeTeamName.StartsWith(teamPrefix) && awayTeamName.StartsWith(teamPrefix) == false)
                return ExtractTeam4(documentNode, Team.Home, Team.Away);
            if (awayTeamName.StartsWith(teamPrefix) && homeTeamName.StartsWith(teamPrefix) == false)
                return ExtractTeam4(documentNode, Team.Away, Team.Home);

            throw new ApplicationException(string.Format("No team with name {0} was found", team));
        }

        private ParseResult ExtractTeam(HtmlNode documentNode, Team team, Team away)
        {
            var series = new List<ResultSeriesReadModel.Serie>();
            var tableNode = documentNode.SelectSingleNode("//table[@id='MainContentPlaceHolder_MatchFact1_TableMatch']");

            // adjust for header and footer rows
            var tableRows = documentNode.SelectNodes("//table[@id='MainContentPlaceHolder_MatchHead1_matchinfo']//tr");
            var numberOfSeries = tableRows.Count - 2;
            if (numberOfSeries < 1 || numberOfSeries > 4)
            {
                var message = string.Format("Found {0} number of series. Expected 1, 2, 3, or 4.", numberOfSeries);
                throw new ApplicationException(message);
            }

            for (var serieNumber = 1; serieNumber <= numberOfSeries; serieNumber++)
            {
                var serie = new ResultSeriesReadModel.Serie();
                var tables = new List<ResultSeriesReadModel.Table>();
                for (var tableNumber = 1; tableNumber <= 4; tableNumber++)
                {
                    var name1 = tableNode.SelectSingleNode(
                        string.Format(
                            "//span[@id='MainContentPlaceHolder_MatchFact1_lblSerie{0}Table{1}Order{2}Player']",
                            serieNumber,
                            tableNumber,
                            1 + (int)team));
                    var name2 = tableNode.SelectSingleNode(
                        string.Format(
                            "//span[@id='MainContentPlaceHolder_MatchFact1_lblSerie{0}Table{1}Order{2}Player']",
                            serieNumber,
                            tableNumber,
                            2 + (int)team));
                    var res1Node = tableNode.SelectSingleNode(
                        string.Format("//span[@id='MainContentPlaceHolder_MatchFact1_lblSerie{0}Table{1}Order{2}Result']",
                            serieNumber,
                            tableNumber,
                            1 + (int)team));
                    var res2Node = tableNode.SelectSingleNode(
                        string.Format("//span[@id='MainContentPlaceHolder_MatchFact1_lblSerie{0}Table{1}Order{2}Result']",
                            serieNumber,
                            tableNumber,
                            2 + (int)team));
                    var scoreNode = tableNode.SelectSingleNode(
                        string.Format(
                            "//span[@id='MainContentPlaceHolder_MatchFact1_lblSerie{0}Table{1}Order{2}Total']",
                            serieNumber,
                            tableNumber,
                            1 + (int)team));
                    var score = int.Parse(scoreNode.InnerText);
                    var res1 = int.Parse(res1Node.InnerText);
                    var res2 = int.Parse(res2Node.InnerText);
                    var table = new ResultSeriesReadModel.Table
                    {
                        Score = score,
                        Game1 = new ResultSeriesReadModel.Game
                        {
                            Pins = res1,
                            Player = GetPlayerId(name1.InnerText).Id
                        },
                        Game2 = new ResultSeriesReadModel.Game
                        {
                            Pins = res2,
                            Player = GetPlayerId(name2.InnerText).Id
                        }
                    };
                    tables.Add(table);
                }

                serie.Tables = tables;
                series.Add(serie);
            }

            var teamScoreNode = documentNode.SelectSingleNode(string.Format("//span[@id='MainContentPlaceHolder_MatchHead1_LblSumPoints{0}']", team));
            var teamScore = int.Parse(teamScoreNode.InnerText);

            var awayScoreNode = documentNode.SelectSingleNode(string.Format("//span[@id='MainContentPlaceHolder_MatchHead1_LblSumPoints{0}']", away));
            var awayScore = int.Parse(awayScoreNode.InnerText);

            return new ParseResult(teamScore, awayScore, series.ToArray());
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
                        string.Format(
                            "//span[@id='MainContentPlaceHolder_MatchFact1_lblSerie{0}Table{1}Order{2}Player']",
                            serieNumber,
                            tableNumber,
                            order));
                    var playerPinsNode = tableNode.SelectSingleNode(
                        string.Format(
                            "//span[@id='MainContentPlaceHolder_MatchFact1_lblSerie{0}Table{1}Order{2}Result']",
                            serieNumber,
                            tableNumber,
                            order));
                    var opponentPinsNode = tableNode.SelectSingleNode(
                        string.Format(
                            "//span[@id='MainContentPlaceHolder_MatchFact1_lblSerie{0}Table{1}Order{2}Result']",
                            serieNumber,
                            tableNumber,
                            3 - order));
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

            var teamScoreNode = documentNode.SelectSingleNode(string.Format("//span[@id='MainContentPlaceHolder_MatchHead1_LblSumPoints{0}']", team));
            var teamScore = int.Parse(teamScoreNode.InnerText);

            var awayScoreNode = documentNode.SelectSingleNode(string.Format("//span[@id='MainContentPlaceHolder_MatchHead1_LblSumPoints{0}']", away));
            var awayScore = int.Parse(awayScoreNode.InnerText);

            return new Parse4Result(teamScore, awayScore, series.ToArray());
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
                throw new ApplicationException(string.Format("No player with name {0} was found", name));
            return p;
        }
    }
}