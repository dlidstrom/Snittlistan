namespace Snittlistan.Web.Areas.V2.ReadModels
{
    using System.Collections.Generic;
    using System.Globalization;

    public class PlayerScore
    {
        public PlayerScore(string playerId, string name)
        {
            Medals = new List<AwardedMedalReadModel>();
            PlayerId = playerId;
            Name = name;
        }

        public string PlayerId { get; private set; }

        public string Name { get; private set; }

        public int Score { get; set; }

        public int Pins { get; set; }

        public int Series { get; set; }

        public string PinsAndSeries => Series != 4
            ? $"{Pins} ({Series})"
            : Pins.ToString(CultureInfo.InvariantCulture);

        public List<AwardedMedalReadModel> Medals { get; set; }

        public void AddMedal(AwardedMedalReadModel awardedMedal)
        {
            Medals.Add(awardedMedal);
        }

        public void ClearMedals()
        {
            Medals.Clear();
        }
    }
}