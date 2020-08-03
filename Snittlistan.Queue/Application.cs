namespace Snittlistan.Queue
{
    using System.Collections.Generic;
    using System.Configuration;
    using Snittlistan.Queue.Config;

    public class Application
    {
        private readonly List<TaskQueueListener> taskQueueListeners = new List<TaskQueueListener>();

        public Application()
        {
            var messagingConfigSection = (MessagingConfigSection)ConfigurationManager.GetSection("messaging");
            foreach (QueueListenerElement listener in messagingConfigSection.QueueListeners.Listeners)
            {
                taskQueueListeners.Add(new TaskQueueListener(listener.CreateSettings()));
            }
        }

        public void Start()
        {
            foreach (TaskQueueListener taskQueueListener in taskQueueListeners)
            {
                taskQueueListener.Start();
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
