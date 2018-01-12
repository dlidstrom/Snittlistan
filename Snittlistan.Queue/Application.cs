using System.Collections.Generic;
using System.Configuration;
using Snittlistan.Queue.Config;

namespace Snittlistan.Queue
{
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
            foreach (var taskQueueListener in taskQueueListeners)
            {
                taskQueueListener.Start();
            }
        }

        public void Stop()
        {
            foreach (var taskQueueListener in taskQueueListeners)
            {
                taskQueueListener.Stop();
            }
        }
    }
}
