#nullable enable

using Snittlistan.Queue.Messages;
using Snittlistan.Web.Infrastructure;
using Snittlistan.Web.Models;

namespace Snittlistan.Web.Commands;

public class CreateRosterMailCommandHandler : CommandHandler<CreateRosterMailCommandHandler.Command>
{
    public override async Task Handle(HandlerContext<Command> context)
    {
        IQueryable<string> query =
            from rosterMail in CompositionRoot.Databases.Snittlistan.RosterMails
            where rosterMail.RosterKey == context.Payload.RosterKey
                && rosterMail.PublishedDate == null
            select rosterMail.RosterKey;
        string[] rosterIds = await query.ToArrayAsync();
        Logger.InfoFormat("roster mails found: {@rosterIds}", rosterIds);
        if (rosterIds.Any() == false)
        {
            TenantFeatures features = await CompositionRoot.GetFeatures();
            _ = CompositionRoot.Databases.Snittlistan.RosterMails.Add(
                new(context.Payload.RosterKey));
            PublishRosterMailsTask task = new(
                context.Payload.RosterKey,
                context.Payload.RosterLink,
                context.Payload.UserProfileLink);
            context.PublishMessage(task, DateTime.Now.AddMinutes(features.RosterMailDelayMinutes));
        }
    }

    public record Command(
        string RosterKey,
        Uri RosterLink,
        Uri UserProfileLink);
}
