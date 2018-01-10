using System;
using System.Collections.Generic;
using System.Web;
using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Areas.V2.ReadModels;

namespace Snittlistan.Web.Areas.V2.ViewModels
{
    public class EliteMedalsViewModel
    {
        private readonly int season;

        public EliteMedalsViewModel(
            int season,
            Dictionary<string, Player> playersDict,
            EliteMedals eliteMedals,
            SeasonResults seasonResults)
        {
            this.season = season;
            var players = new List<PlayerInfo>();
            foreach (var playerId in playersDict.Keys)
            {
                var player = playersDict[playerId];
                var existingMedal = eliteMedals.GetExistingMedal(playerId);
                var topThreeResults = seasonResults.GetTopThreeResults(playerId, existingMedal.Value);
                var formattedExistingMedal = eliteMedals.GetFormattedExistingMedal(playerId);
                var nextMedal = eliteMedals.GetNextMedal(playerId);
                var playerInfo = new PlayerInfo(
                    playerId,
                    player.Name,
                    player.PersonalNumber,
                    topThreeResults,
                    existingMedal.Value,
                    formattedExistingMedal,
                    nextMedal);
                players.Add(playerInfo);
            }

            Players = players.ToArray();
        }

        public PlayerInfo[] Players { get; private set; }

        public HtmlString FormatSeason()
        {
            return new HtmlString($"{season} &minus; {season + 1}");
        }

        public class PlayerInfo
        {
            private readonly Tuple<string, string> formattedExistingMedal;
            private readonly Tuple<string, string> formattedNextMedal;

            public PlayerInfo(
                string playerId,
                string name,
                int personalNumber,
                HashSet<Tuple<SeasonResults.PlayerResult, bool>> topThreeResults,
                EliteMedals.EliteMedal.EliteMedalValue existingMedal,
                Tuple<string, string> formattedExistingMedal,
                Tuple<string, string> formattedNextMedal)
            {
                PlayerId = playerId;
                Name = name;
                PersonalNumber = personalNumber;
                TopThreeResults = topThreeResults;
                ExistingMedal = existingMedal;
                this.formattedExistingMedal = formattedExistingMedal;
                this.formattedNextMedal = formattedNextMedal;
            }

            public string PlayerId { get; private set; }

            public string Name { get; private set; }

            public int PersonalNumber { get; private set; }

            public EliteMedals.EliteMedal.EliteMedalValue ExistingMedal { get; private set; }

            public HashSet<Tuple<SeasonResults.PlayerResult, bool>> TopThreeResults { get; private set; }

            public Tuple<string, string> FormattedExistingMedal()
            {
                return formattedExistingMedal;
            }

            public Tuple<string, string> FormattedNextMedal()
            {
                return formattedNextMedal;
            }
        }
    }
}