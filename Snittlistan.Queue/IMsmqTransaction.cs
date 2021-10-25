#nullable enable

namespace Snittlistan.Queue
{
    using Snittlistan.Queue.Messages;

    public interface IMsmqTransaction
    {
        void PublishMessage(MessageEnvelope envelope);

        void Commit();
    }
}
