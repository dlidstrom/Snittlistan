namespace Snittlistan.Web.Infrastructure.BackgroundTasks
{
    public interface IBackgroundTaskHandler<in TTask>
    {
        void Handle(TTask task);
    }
}