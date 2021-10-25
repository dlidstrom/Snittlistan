#nullable enable

namespace Snittlistan.Queue.Messages
{
    public interface ITask
    {
        BusinessKey BusinessKey { get; }
    }
}
