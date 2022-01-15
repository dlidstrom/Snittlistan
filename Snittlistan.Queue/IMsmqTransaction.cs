#nullable enable

using Snittlistan.Queue.Messages;

namespace Snittlistan.Queue;

public interface IMsmqTransaction
{
    void Send(MessageEnvelope envelope);

    void Commit();
}
