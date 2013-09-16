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
            Home,
            Away
        }

        public ParseResult Parse(string content, string team)
        {
            var document = new HtmlDocument();
            document.LoadHtml(content);

            // find which team we should import
            var documentNode = document.DocumentNode;
            var homeTeamLabel =
                documentNode.SelectSingleNode("//span[@id='MainContentPlaceHolder_MatchHead_LabelHomeTeam']");
            var awayTeamLabel =
                documentNode.SelectSingleNode("//span[@id='MainContentPlaceHolder_MatchHead_LabelAwayTeam']");

            var homeTeamName = homeTeamLabel.InnerText;
            if (team == homeTeamName)
                return ExtractTeam(documentNode, Team.Home, Team.Away);
            var awayTeamName = awayTeamLabel.InnerText;
            if (team == awayTeamName)
                return ExtractTeam(documentNode, Team.Away, Team.Home);

            // try alternate name
            var teamPrefix = team.Split(' ')
                .First();
            if (homeTeamName.StartsWith(teamPrefix) && awayTeamName.StartsWith(teamPrefix))
                throw new ApplicationException(string.Format("Could not find team with prefix {0}", teamPrefix));

            if (homeTeamName.StartsWith(teamPrefix) && awayTeamName.StartsWith(teamPrefix) == false)
                return ExtractTeam(documentNode, Team.Home, Team.Away);
            if (awayTeamName.StartsWith(teamPrefix) && homeTeamName.StartsWith(teamPrefix) == false)
                return ExtractTeam(documentNode, Team.Away, Team.Home);

            throw new ApplicationException(string.Format("No team with name {0} was found", team));
        }

        public Parse4Result Parse4(string content, string team)
        {
            var document = new HtmlDocument();
            document.LoadHtml(content);

            // find which team we should import
            var documentNode = document.DocumentNode;
            var homeTeamLabel =
                documentNode.SelectSingleNode("//span[@id='MainContentPlaceHolder_MatchHead_LabelHomeTeam']");
            var awayTeamLabel =
                documentNode.SelectSingleNode("//span[@id='MainContentPlaceHolder_MatchHead_LabelAwayTeam']");

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

            for (var round = 1; round <= 4; round++)
            {
                var row =
                    tableNode.SelectSingleNode(string.Format("tr[@id='MainContentPlaceHolder_MatchFact1_TableRow{1}{0}']",
                                                             round * 2, team));
                if (row == null) continue;
                var serie = new ResultSeriesReadModel.Serie();
                var tables = new List<ResultSeriesReadModel.Table>();
                for (var serieNumber = 1; serieNumber <= 4; serieNumber++)
                {
                    var name1 =
                        row.SelectSingleNode(
                            string.Format("//span[@id='MainContentPlaceHolder_MatchFact1_LblTable{0}Ser{1}{2}RoundNbr1Name']",
                                          round, serieNumber, team));
                    var name2 =
                        row.SelectSingleNode(
                            string.Format("//span[@id='MainContentPlaceHolder_MatchFact1_LblTable{0}Ser{1}{2}RoundNbr2Name']",
                                          round, serieNumber, team));
                    var res1Node =
                        row.SelectSingleNode(
                            string.Format(
                                "//span[@id='MainContentPlaceHolder_MatchFact1_LblTable{0}Ser{1}{2}RoundNbr1Result']", round,
                                serieNumber, team));
                    var res2Node =
                        row.SelectSingleNode(
                            string.Format(
                                "//span[@id='MainContentPlaceHolder_MatchFact1_LblTable{0}Ser{1}{2}RoundNbr2Result']", round,
                                serieNumber, team));
                    var scoreNode =
                        row.SelectSingleNode(
                            string.Format("//span[@id='MainContentPlaceHolder_MatchFact1_LblTable{0}Ser{1}{2}RoundNbr1Rank']",
                                          round, serieNumber, team));
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

            var teamScoreNode = documentNode.SelectSingleNode(string.Format("//span[@id='MainContentPlaceHolder_MatchHead_LblSumPoints{0}']", team));
            var teamScore = int.Parse(teamScoreNode.InnerText);

            var awayScoreNode = documentNode.SelectSingleNode(string.Format("//span[@id='MainContentPlaceHolder_MatchHead_LblSumPoints{0}']", away));
            var awayScore = int.Parse(awayScoreNode.InnerText);

            return new ParseResult(teamScore, awayScore, series.ToArray());
        }

        private Parse4Result ExtractTeam4(HtmlNode documentNode, Team team, Team away)
        {
            var series = new List<ResultSeries4ReadModel.Serie>();
            var tableNode = documentNode.SelectSingleNode("//table[@id='MainContentPlaceHolder_MatchFact1_TableMatch']");

            for (var round = 1; round <= 4; round++)
            {
                var row =
                    tableNode.SelectSingleNode(string.Format("tr[@id='MainContentPlaceHolder_MatchFact1_TableRow{1}{0}']",
                                                             round * 2, team));
                if (row == null) continue;
                var games = new List<ResultSeries4ReadModel.Game>();
                for (var serieNumber = 1; serieNumber <= 4; serieNumber++)
                {
                    var name1 =
                        row.SelectSingleNode(
                            string.Format("//span[@id='MainContentPlaceHolder_MatchFact1_LblTable{0}Ser{1}{2}RoundNbr1Name']",
                                          round, serieNumber, team));
                    var res1Node =
                        row.SelectSingleNode(
                            string.Format(
                                "//span[@id='MainContentPlaceHolder_MatchFact1_LblTable{0}Ser{1}{2}RoundNbr1Result']", round,
                                serieNumber, team));
                    var scoreNode =
                        row.SelectSingleNode(
                            string.Format("//span[@id='MainContentPlaceHolder_MatchFact1_LblTable{0}Ser{1}{2}RoundNbr1Rank']",
                                          round, serieNumber, team));
                    var score = int.Parse(scoreNode.InnerText);
                    var res1 = int.Parse(res1Node.InnerText);
                    var game = new ResultSeries4ReadModel.Game
                    {
                        Score = score,
                        Pins = res1,
                        Player = GetPlayerId(name1.InnerText).Id
                    };
                    games.Add(game);
                }

                var serie = new ResultSeries4ReadModel.Serie
                {
                    Games = games
                };
                series.Add(serie);
            }

            var teamScoreNode = documentNode.SelectSingleNode(string.Format("//span[@id='MainContentPlaceHolder_MatchHead_LblSumPoints{0}']", team));
            var teamScore = int.Parse(teamScoreNode.InnerText);

            var awayScoreNode = documentNode.SelectSingleNode(string.Format("//span[@id='MainContentPlaceHolder_MatchHead_LblSumPoints{0}']", away));
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

        public class ParseResult
        {
            public ParseResult(int teamScore, int awayScore, ResultSeriesReadModel.Serie[] series)
            {
                if (series == null) throw new ArgumentNullException("series");
                Series = series;
                OpponentScore = awayScore;
                TeamScore = teamScore;
            }

            public int TeamScore { get; private set; }

            public int OpponentScore { get; private set; }

            public ResultSeriesReadModel.Serie[] Series { get; private set; }
        }

        public class Parse4Result
        {
            public Parse4Result(int teamScore, int awayScore, ResultSeries4ReadModel.Serie[] series)
            {
                if (series == null) throw new ArgumentNullException("series");
                Series = series;
                OpponentScore = awayScore;
                TeamScore = teamScore;
            }

            public int TeamScore { get; private set; }

            public int OpponentScore { get; private set; }

            public ResultSeries4ReadModel.Serie[] Series { get; private set; }
        }
    }
}