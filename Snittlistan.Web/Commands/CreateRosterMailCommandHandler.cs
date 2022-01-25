#nullable enable

using Snittlistan.Queue.Messages;
using Snittlistan.Web.Infrastructure;
using System.Data.Entity;

namespace Snittlistan.Web.Commands;

public class CreateRosterMailCommandHandler : CommandHandler<CreateRosterMailCommandHandler.Command>
{
    public override async Task Handle(HandlerContext<Command> context)
    {
        IQueryable<string> query =
            from rosterMail in CompositionRoot.Databases.Snittlistan.RosterMails
            where rosterMail.RosterId == context.Payload.RosterId
                && rosterMail.PublishedDate == null
            select rosterMail.RosterId;
        string[] rosterIds = await query.ToArrayAsync();
        if (rosterIds.Any() == false)
        {
            _ = CompositionRoot.Databases.Snittlistan.RosterMails.Add(new(context.Payload.RosterId));
            PublishRosterMailsTask task = new(context.Payload.RosterId);
            context.PublishMessage(task, DateTime.Now.AddSeconds(10));
        }
    }

    public record Command(string RosterId);
}
