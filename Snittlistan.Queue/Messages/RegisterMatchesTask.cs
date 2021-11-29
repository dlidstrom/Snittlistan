#nullable enable

namespace Snittlistan.Queue.Messages
{
    public class RegisterMatchesTask : TaskBase
    {
        public RegisterMatchesTask()
            : base(new(typeof(RegisterMatchesTask), string.Empty))
        {
        }
    }
}
