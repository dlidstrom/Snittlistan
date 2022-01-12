
using System.Configuration;

namespace Snittlistan.Queue.Config;
public class QueueListenersCollection : ConfigurationElementCollection
{
    protected override ConfigurationElement CreateNewElement()
    {
        return new QueueListenerElement();
    }

    protected override object GetElementKey(ConfigurationElement element)
    {
        return ((QueueListenerElement)element).Name;
    }
}
