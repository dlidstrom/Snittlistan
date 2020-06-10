namespace Snittlistan.Queue.Config
{
    using System.Configuration;

    public class MessagingConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("queueListeners", IsRequired = true)]
        public QueueListenersElement QueueListeners
        {
            get => (QueueListenersElement)this["queueListeners"];

            set => this["queueListeners"] = value;
        }
    }
}