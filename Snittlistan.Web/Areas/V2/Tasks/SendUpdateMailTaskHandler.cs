#nullable enable

namespace Snittlistan.Web.Areas.V2.Tasks
{
    using System.Linq;
    using System.Threading.Tasks;
    using Snittlistan.Queue.Messages;
    using Snittlistan.Web.Areas.V2.Domain;
    using Snittlistan.Web.Models;

    public class SendUpdateMailTaskHandler : TaskHandler<SendUpdateMailTask>
    {
        public override async Task Handle(SendUpdateMailTask task)
        {
            Player player = DocumentSession.Load<Player>(task.PlayerId);
            Roster roster = DocumentSession.Include<Roster>(x => x.Players).Load<Roster>(task.RosterId);
            FormattedAuditLog formattedAuditLog = roster.GetFormattedAuditLog(DocumentSession, task.CorrelationId);
            Player[] players = DocumentSession.Load<Player>(roster.Players);
            string teamLeader =
                roster.TeamLeader != null
                ? DocumentSession.Load<Player>(roster.TeamLeader).Name
                : string.Empty;
            UpdateRosterEmail email = new(
                player.Email,
                formattedAuditLog,
                players.Select(x => x.Name).ToArray(),
                teamLeader);
            await EmailService.SendAsync(email);
        }
    }
}
