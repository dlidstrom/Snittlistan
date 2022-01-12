#nullable enable

using Snittlistan.Queue.Messages;
using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Infrastructure;
using Snittlistan.Web.Models;

namespace Snittlistan.Web.TaskHandlers;

public class SendUpdateMailTaskHandler : TaskHandler<SendUpdateMailTask>
{
    public override async Task Handle(HandlerContext<SendUpdateMailTask> context)
    {
        Player player = CompositionRoot.DocumentSession.Load<Player>(context.Payload.PlayerId);
        Roster roster = CompositionRoot.DocumentSession.Include<Roster>(x => x.Players).Load<Roster>(context.Payload.RosterId);
        FormattedAuditLog formattedAuditLog = roster.GetFormattedAuditLog(CompositionRoot.DocumentSession, context.CorrelationId);
        Player[] players = CompositionRoot.DocumentSession.Load<Player>(roster.Players);
        string teamLeader =
            roster.TeamLeader != null
            ? CompositionRoot.DocumentSession.Load<Player>(roster.TeamLeader).Name
            : string.Empty;
        UpdateRosterEmail email = new(
            player.Email,
            formattedAuditLog,
            players.Select(x => x.Name).ToArray(),
            teamLeader);
        await CompositionRoot.EmailService.SendAsync(email);
    }
}
