
using System.Configuration;

namespace Snittlistan.Queue.Config;
public class MessagingConfigSection : ConfigurationSection
{
    [ConfigurationProperty("queueListeners", IsRequired = true)]
    public QueueListenersElement QueueListeners
    {
        get => (QueueListenersElement)this["queueListeners"];

        set => this["queueListeners"] = value;
    }
}
