#nullable enable

namespace Snittlistan.Queue;

public record MessageQueueProcessorSettings(
    string ReadQueue,
    string ErrorQueue,
    int WorkerThreadCount,
    bool AutoCreateQueues,
    bool DropFailedMessages);
