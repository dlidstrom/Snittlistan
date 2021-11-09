#nullable enable

namespace Snittlistan.Web.Areas.V2.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using Snittlistan.Web.Areas.V2.Domain;
    using Snittlistan.Web.Areas.V2.ReadModels;

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
            List<PlayerInfo> players = new();
            foreach (string playerId in playersDict.Keys)
            {
                Player player = playersDict[playerId];
                EliteMedals.EliteMedal existingMedal = eliteMedals.GetExistingMedal(playerId);
                HashSet<Tuple<SeasonResults.PlayerResult, bool>> topThreeResults = seasonResults.GetTopThreeResults(playerId, existingMedal.Value);
                FormattedMedal formattedExistingMedal = eliteMedals.GetFormattedExistingMedal(playerId);
                FormattedMedal nextMedal = eliteMedals.GetNextMedal(playerId);
                PlayerInfo playerInfo = new(
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

        public PlayerInfo[] Players { get; }

        public HtmlString FormatSeason()
        {
            return new HtmlString($"{season} &minus; {season + 1}");
        }

        public class PlayerInfo
        {
            private readonly FormattedMedal formattedExistingMedal;
            private readonly FormattedMedal formattedNextMedal;

            public PlayerInfo(
                string playerId,
                string name,
                int personalNumber,
                HashSet<Tuple<SeasonResults.PlayerResult, bool>> topThreeResults,
                EliteMedals.EliteMedal.EliteMedalValue existingMedal,
                FormattedMedal formattedExistingMedal,
                FormattedMedal formattedNextMedal)
            {
                PlayerId = playerId;
                Name = name;
                PersonalNumber = personalNumber;
                TopThreeResults = topThreeResults;
                ExistingMedal = existingMedal;
                this.formattedExistingMedal = formattedExistingMedal;
                this.formattedNextMedal = formattedNextMedal;
            }

            public string PlayerId { get; }

            public string Name { get; }

            public int PersonalNumber { get; }

            public EliteMedals.EliteMedal.EliteMedalValue ExistingMedal { get; }

            public HashSet<Tuple<SeasonResults.PlayerResult, bool>> TopThreeResults { get; }

            public FormattedMedal FormattedExistingMedal()
            {
                return formattedExistingMedal;
            }

            public FormattedMedal FormattedNextMedal()
            {
                return formattedNextMedal;
            }
        }
    }
}
