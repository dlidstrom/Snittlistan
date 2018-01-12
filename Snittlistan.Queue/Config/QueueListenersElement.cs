using System.Configuration;

namespace Snittlistan.Queue.Config
{
    public class QueueListenersElement : ConfigurationElement
    {
        [ConfigurationProperty("", IsRequired = true, IsDefaultCollection = true)]
        public QueueListenersCollection Listeners
        {
            get => (QueueListenersCollection)this[string.Empty];

            set => this[string.Empty] = value;
        }
    }
}