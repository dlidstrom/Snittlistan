#nullable enable

namespace Snittlistan.Web.Areas.V2.Domain.Match.Events
{
    using EventStoreLite;

    public class AwardedMedal : Event
    {
        public AwardedMedal(
            int bitsMatchId,
            string rosterId,
            string player,
            MedalType medalType,
            int value)
        {
            BitsMatchId = bitsMatchId;
            RosterId = rosterId;
            Value = value;
            MedalType = medalType;
            Player = player;
        }

        public int BitsMatchId { get; private set; }

        public string RosterId { get; private set; }

        public string Player { get; private set; }

        public MedalType MedalType { get; private set; }

        public int Value { get; private set; }
    }
}
