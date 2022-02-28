#nullable enable

using System.Configuration;

namespace Snittlistan.Queue.Config;

public class QueueListenerElement : ConfigurationElement
{
    [ConfigurationProperty("name", IsRequired = true)]
    public string Name
    {
        get => (string)this["name"];

        set => this["name"] = value;
    }

    [ConfigurationProperty("isEnabled", IsRequired = true)]
    public bool IsEnabled
    {
        get => (bool)this["isEnabled"];

        set => this["isEnabled"] = value;
    }

    [ConfigurationProperty("readQueue", IsRequired = true)]
    public string ReadQueue
    {
        get => (string)this["readQueue"];

        set => this["readQueue"] = value;
    }

    [ConfigurationProperty("errorQueue")]
    public string ErrorQueue
    {
        get
        {
            string value = (string)this["errorQueue"];

            if (string.IsNullOrWhiteSpace(value))
            {
                value = $"{ReadQueue}.error";
            }

            return value;
        }

        set => this["errorQueue"] = value;
    }

    [ConfigurationProperty("workerThreads", DefaultValue = 1)]
    public int WorkerThreads
    {
        get => (int)this["workerThreads"];

        set => this["workerThreads"] = value;
    }

    [ConfigurationProperty("autoCreateQueues", DefaultValue = true)]
    public bool AutoCreateQueues
    {
        get => (bool)this["autoCreateQueues"];

        set => this["autoCreateQueues"] = value;
    }

    [ConfigurationProperty("dropFailedMessages", DefaultValue = false)]
    public bool DropFailedMessages
    {
        get => (bool)this["dropFailedMessages"];

        set => this["dropFailedMessages"] = value;
    }

    public MessageQueueProcessorSettings CreateSettings()
    {
        MessageQueueProcessorSettings settings = new(
            ReadQueue,
            ErrorQueue,
            WorkerThreads,
            AutoCreateQueues,
            DropFailedMessages);
        return settings;
    }
}
