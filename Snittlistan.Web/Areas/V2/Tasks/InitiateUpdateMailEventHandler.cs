#nullable enable

namespace Snittlistan.Web.Areas.V2.Tasks
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Snittlistan.Queue.Messages;
    using Snittlistan.Web.Areas.V2.Domain;

    public class InitiateUpdateMailEventHandler : TaskHandler<InitiateUpdateMailEvent>
    {
        public override Task Handle(InitiateUpdateMailEvent task)
        {
            Roster roster = DocumentSession.Load<Roster>(task.RosterId);
            AuditLogEntry auditLogEntry = roster.AuditLogEntries.Single(x => x.CorrelationId == task.CorrelationId);
            RosterState before = (RosterState)auditLogEntry.Before;
            RosterState after = (RosterState)auditLogEntry.After;
            IEnumerable<string> affectedPlayers = before.Players.Concat(after.Players);
            foreach (string playerId in new HashSet<string>(affectedPlayers))
            {
                SendUpdateMailEvent message = new(
                    task.RosterId,
                    playerId,
                    task.CorrelationId);
                PublishMessage(message);
            }

            return Task.CompletedTask;
        }
    }
}
