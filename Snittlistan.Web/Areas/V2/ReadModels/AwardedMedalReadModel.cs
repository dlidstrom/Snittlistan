namespace Snittlistan.Web.Areas.V2.ReadModels
{
    using Snittlistan.Web.Areas.V2.Domain.Match.Events;

    public class AwardedMedalReadModel
    {
        public AwardedMedalReadModel(string player, MedalType medalType, int value)
        {
            Player = player;
            MedalType = medalType;
            Value = value;
        }

        public string Player { get; private set; }

        public MedalType MedalType { get; private set; }

        public int Value { get; private set; }

        public string ToHtml()
        {
            if (MedalType == MedalType.PinsInSerie)
            {
                if (Value == 300)
                    return @"<span class=""label label-300"">" + Value + "</span>";

                return @"<span class=""label label-pins-accomplishment"">" + Value + "</span>";
            }

            return @"<span class=""label label-score-accomplishment"">" + Value + "P</span>";
        }
    }
}