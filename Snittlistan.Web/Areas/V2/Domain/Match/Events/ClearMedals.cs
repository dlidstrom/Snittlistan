using EventStoreLite;

namespace Snittlistan.Web.Areas.V2.Domain.Match.Events
{
    public class ClearMedals : Event
    {
        public ClearMedals(int bitsMatchId)
        {
            BitsMatchId = bitsMatchId;
        }

        public int BitsMatchId { get; private set; }
    }
}