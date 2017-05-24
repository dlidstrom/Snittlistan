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
            return new HtmlString(string.Format("{0} &minus; {1}", season, season + 1));
        }

        public bool IsValid(int totalPins)
        {
            throw new System.NotImplementedException();
        }

        public class PlayerInfo
        {
            private readonly string formattedExistingMedal;
            private readonly string nextMedal;

            public PlayerInfo(
                string playerId,
                string name,
                int personalNumber,
                HashSet<Tuple<SeasonResults.PlayerResult, bool>> topThreeResults,
                EliteMedals.EliteMedal.EliteMedalValue existingMedal,
                string formattedExistingMedal,
                string nextMedal)
            {
                PlayerId = playerId;
                Name = name;
                PersonalNumber = personalNumber;
                TopThreeResults = topThreeResults;
                ExistingMedal = existingMedal;
                this.formattedExistingMedal = formattedExistingMedal;
                this.nextMedal = nextMedal;
            }

            public string PlayerId { get; private set; }

            public string Name { get; private set; }

            public int PersonalNumber { get; private set; }

            public EliteMedals.EliteMedal.EliteMedalValue ExistingMedal { get; private set; }

            public HashSet<Tuple<SeasonResults.PlayerResult, bool>> TopThreeResults { get; private set; }

            public HtmlString FormattedExistingMedal()
            {
                return new HtmlString(formattedExistingMedal);
            }

            public HtmlString FormattedNextMedal()
            {
                return new HtmlString(nextMedal);
            }
        }
    }
}