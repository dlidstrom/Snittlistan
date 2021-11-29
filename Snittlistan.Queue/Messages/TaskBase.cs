#nullable enable

namespace Snittlistan.Queue.Messages
{
    public abstract class TaskBase
    {
        protected TaskBase(BusinessKey businessKey)
        {
            BusinessKey = businessKey;
        }

        public BusinessKey BusinessKey { get; }
    }
}
