#nullable enable

namespace Snittlistan.Queue.Messages;

public class PublishExpiredTasksTask : TaskBase
{
    public PublishExpiredTasksTask()
        : base(new(typeof(PublishExpiredTasksTask).FullName, string.Empty))
    {
    }
}
