using Snittlistan.Queue.Messages;

namespace Snittlistan.Queue
{
    public interface IMsmqTransaction
    {
        void PublishMessage(MessageEnvelope envelope);
        void Commit();
    }
}