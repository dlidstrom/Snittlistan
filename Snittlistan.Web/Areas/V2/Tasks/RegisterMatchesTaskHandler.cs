#nullable enable

using Snittlistan.Queue.Messages;
using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Areas.V2.Queries;
using Snittlistan.Web.Infrastructure;
using Snittlistan.Web.Models;

namespace Snittlistan.Web.Areas.V2.Tasks;

public class RegisterMatchesTaskHandler : TaskHandler<RegisterMatchesTaskHandler.RegisterMatchesTask>
{
    public override Task Handle(MessageContext<RegisterMatchesTask> context)
    {
        WebsiteConfig websiteConfig = DocumentSession.Load<WebsiteConfig>(WebsiteConfig.GlobalId);
        Roster[] pendingMatches = ExecuteQuery(new GetPendingMatchesQuery(websiteConfig.SeasonId));
        foreach (Roster pendingMatch in pendingMatches.Where(x => x.SkipRegistration == false))
        {
            context.PublishMessage(new RegisterMatchTask(pendingMatch.Id!, pendingMatch.BitsMatchId));
        }

        return Task.CompletedTask;
    }

    public class RegisterMatchesTask : TaskBase
    {
        public RegisterMatchesTask()
            : base(new(typeof(RegisterMatchesTask), string.Empty))
        {
        }
    }
}
