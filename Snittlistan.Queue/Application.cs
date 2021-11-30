
using Snittlistan.Queue.Config;

#nullable enable

namespace Snittlistan.Queue;
public class Application
{
    private readonly List<TaskQueueListener> taskQueueListeners = new();
    private readonly MessagingConfigSection messagingConfigSection;
    private readonly string urlScheme;
    private readonly int port;

    public Application(
        MessagingConfigSection messagingConfigSection,
        string urlScheme,
        int port)
    {
        this.messagingConfigSection = messagingConfigSection;
        this.urlScheme = urlScheme;
        this.port = port;
    }

    public void Start()
    {
        foreach (QueueListenerElement listener in messagingConfigSection.QueueListeners.Listeners)
        {
            TaskQueueListener taskQueueListener = new(
                listener.CreateSettings(),
                urlScheme,
                port);
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
