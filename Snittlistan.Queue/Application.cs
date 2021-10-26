#nullable enable

namespace Snittlistan.Queue
{
    using System.Collections.Generic;
    using Snittlistan.Queue.Config;

    public class Application
    {
        private readonly List<TaskQueueListener> taskQueueListeners = new();
        private readonly MessagingConfigSection messagingConfigSection;
        private readonly string urlScheme;

        public Application(MessagingConfigSection messagingConfigSection, string urlScheme)
        {
            this.messagingConfigSection = messagingConfigSection;
            this.urlScheme = urlScheme;
        }

        public void Start()
        {
            foreach (QueueListenerElement listener in messagingConfigSection.QueueListeners.Listeners)
            {
                TaskQueueListener taskQueueListener = new(listener.CreateSettings(), urlScheme);
                taskQueueListener.Start();
                taskQueueListeners.Add(taskQueueListener);
            }
        }

        public void Stop()
        {
            foreach (TaskQueueListener taskQueueListener in taskQueueListeners)
            {
                taskQueueListener.Stop();
            }
        }
    }
}
