#nullable enable

namespace Snittlistan.Queue.Messages
{
    public class GetPlayersFromBitsTask : ITask
    {
        public BusinessKey BusinessKey => new(GetType(), string.Empty);
    }
}
