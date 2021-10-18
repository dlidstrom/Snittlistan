namespace Snittlistan.Queue.Messages
{
    public class GetRostersFromBitsTask : ITask
    {
        public BusinessKey BusinessKey => new(GetType(), string.Empty);
    }
}
