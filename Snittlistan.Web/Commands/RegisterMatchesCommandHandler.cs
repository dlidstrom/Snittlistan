#nullable enable

using Snittlistan.Queue.Messages;
using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Areas.V2.Queries;
using Snittlistan.Web.Infrastructure;
using Snittlistan.Web.Models;

namespace Snittlistan.Web.Commands;

public class RegisterMatchesCommandHandler : CommandHandler<RegisterMatchesCommandHandler.Command>
{
    public override async Task Handle(HandlerContext<Command> context)
    {
        WebsiteConfig websiteConfig = CompositionRoot.DocumentSession.Load<WebsiteConfig>(WebsiteConfig.GlobalId);
        Roster[] pendingMatches = ExecuteQuery(new GetPendingMatchesQuery(websiteConfig.SeasonId));
        foreach (Roster pendingMatch in pendingMatches.Where(x => x.SkipRegistration == false))
        {
            await context.PublishMessage(new RegisterPendingMatchTask(pendingMatch.Id!, pendingMatch.BitsMatchId));
        }
    }

    public record Command();
}
