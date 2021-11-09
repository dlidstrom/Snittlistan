#nullable enable

namespace Snittlistan.Queue.Messages
{
    public class RegisterMatchesTask : ITask
    {
        public BusinessKey BusinessKey => new(GetType(), string.Empty);
    }
}
