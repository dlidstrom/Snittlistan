#nullable enable

using Snittlistan.Queue.Messages;
using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Areas.V2.Queries;
using Snittlistan.Web.Infrastructure;
using Snittlistan.Web.Models;

namespace Snittlistan.Web.Commands;

public class RegisterMatchesCommandHandler : CommandHandler<RegisterMatchesCommandHandler.Command>
{
    public override Task Handle(HandlerContext<Command> context)
    {
        WebsiteConfig websiteConfig = CompositionRoot.DocumentSession.Load<WebsiteConfig>(WebsiteConfig.GlobalId);
        Roster[] pendingMatches = ExecuteQuery(new GetPendingMatchesQuery(websiteConfig.SeasonId));
        foreach (Roster pendingMatch in pendingMatches.Where(x => x.SkipRegistration == false))
        {
            context.PublishMessage(new RegisterPendingMatchTask(pendingMatch.Id!, pendingMatch.BitsMatchId));
        }

        return Task.CompletedTask;
    }

    public record Command();
}
