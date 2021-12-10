using Snittlistan.Queue.Messages;

#nullable enable

namespace Snittlistan.Queue;
public interface IMsmqTransaction
{
    void PublishMessage(MessageEnvelope envelope);

    void Commit();
}
