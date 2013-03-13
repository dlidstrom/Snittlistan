using EventStoreLite;

namespace Snittlistan.Web.Areas.V2.Domain.Match.Events
{
    public class RosterChanged : Event
    {
        public RosterChanged(string oldId, string newId)
        {
            this.OldId = oldId;
            this.NewId = newId;
        }

        public string OldId { get; private set; }

        public string NewId { get; private set; }
    }
}