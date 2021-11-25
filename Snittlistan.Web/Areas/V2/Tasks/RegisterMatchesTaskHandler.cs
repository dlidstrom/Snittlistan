#nullable enable

namespace Snittlistan.Web.Areas.V2.Tasks
{
    using System.Linq;
    using System.Threading.Tasks;
    using Snittlistan.Queue.Messages;
    using Snittlistan.Web.Areas.V2.Domain;
    using Snittlistan.Web.Areas.V2.Queries;
    using Snittlistan.Web.Models;

    public class RegisterMatchesTaskHandler : TaskHandler<RegisterMatchesTask>
    {
        public override async Task Handle(MessageContext<RegisterMatchesTask> context)
        {
            WebsiteConfig websiteConfig = DocumentSession.Load<WebsiteConfig>(WebsiteConfig.GlobalId);
            Roster[] pendingMatches = ExecuteQuery(new GetPendingMatchesQuery(websiteConfig.SeasonId));
            foreach (Roster pendingMatch in pendingMatches.Where(x => x.SkipRegistration == false))
            {
                await context.PublishMessage(new RegisterMatchTask(pendingMatch.Id!, pendingMatch.BitsMatchId));
            }
        }
    }
}
