using System.Linq;
using Raven.Client.Indexes;
using Snittlistan.Web.Infrastructure.BackgroundTasks;

namespace Snittlistan.Web.Infrastructure.Indexes
{
    public class BackgroundTasksIndex : AbstractIndexCreationTask<BackgroundTask>
    {
        public BackgroundTasksIndex()
        {
            Map = tasks => from task in tasks
                           select new
                           {
                               task.NextTry,
                               task.IsFinished,
                               task.IsFailed
                           };
        }
    }
}