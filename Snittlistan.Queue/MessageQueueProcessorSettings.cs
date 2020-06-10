namespace Snittlistan.Queue
{
    using System;

    public class MessageQueueProcessorSettings
    {
        /// <summary>
        /// Initializes a new instance of the MessageQueueProcessorSettings class.
        /// </summary>
        /// <param name="readQueue">Message queue to read from.</param>
        /// <param name="errorQueue">Message queue to use for error handling.</param>
        /// <param name="workerThreadCount">Number of processing threads.</param>
        /// <param name="autoCreateQueues">Automatically create queues.</param>
        public MessageQueueProcessorSettings(string readQueue, string errorQueue, int workerThreadCount, bool autoCreateQueues)
        {
            ReadQueue = readQueue ?? throw new ArgumentNullException(nameof(readQueue));
            ErrorQueue = errorQueue ?? throw new ArgumentNullException(nameof(errorQueue));
            WorkerThreadCount = workerThreadCount;
            AutoCreateQueues = autoCreateQueues;
        }

        public string ReadQueue { get; }

        public string ErrorQueue { get; }

        public int WorkerThreadCount { get; }

        public bool AutoCreateQueues { get; }

        public override string ToString()
        {
            return $"Read={ReadQueue}, Error={ErrorQueue}, Workers={WorkerThreadCount}";
        }
    }
}