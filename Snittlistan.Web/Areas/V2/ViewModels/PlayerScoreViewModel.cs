#nullable enable

namespace Snittlistan.Web.Areas.V2.ViewModels
{
    using System.Collections.Generic;
    using Snittlistan.Web.Areas.V2.Domain;
    using Snittlistan.Web.Areas.V2.ReadModels;

    public class PlayerScoreViewModel
    {
        public PlayerScoreViewModel(PlayerScore playerScore, Roster roster)
        {
            Series = playerScore.Series;
            Pins = playerScore.Pins;
            Score = playerScore.Score;
            Name = playerScore.Name;
            PinsAndSeries = playerScore.PinsAndSeries;
            Medals = playerScore.Medals;
            TeamLevel = roster.TeamLevel!;
            Team = roster.Team;
        }

        public int Series { get; }
        public int Pins { get; }
        public string Name { get; }
        public string PinsAndSeries { get; }
        public List<AwardedMedalReadModel> Medals { get; }
        public string TeamLevel { get; }
        public string Team { get; }
        public int Score { get; }
    }
}
