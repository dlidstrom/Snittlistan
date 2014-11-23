using Snittlistan.Web.Infrastructure.BackgroundTasks;
using Snittlistan.Web.Models;

namespace Snittlistan.Web.Tasks
{
    public class MatchRegisteredTaskHandler : IBackgroundTaskHandler<MatchRegisteredTask>
    {
        public void Handle(MatchRegisteredTask task)
        {
            Emails.MatchRegistered(task.Subject, task.Team, task.Opponent, task.Score, task.OpponentScore);
        }
    }
}