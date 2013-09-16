using EventStoreLite;

namespace Snittlistan.Web.Areas.V2.Domain.Match.Events
{
    public class Roster4Changed : Event
    {
        public Roster4Changed(string oldId, string newId)
        {
            OldId = oldId;
            NewId = newId;
        }

        public string OldId { get; private set; }

        public string NewId { get; private set; }
    }
}