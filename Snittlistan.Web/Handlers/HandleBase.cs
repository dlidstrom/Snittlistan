using Raven.Client;
using Snittlistan.Web.Infrastructure.BackgroundTasks;

namespace Snittlistan.Web.Handlers
{
    public abstract class HandleBase<TEvent> : IHandle<TEvent>
    {
        public IDocumentSession DocumentSession { get; set; }

        public abstract void Handle(TEvent @event);

        protected void SendTask<TTask>(TTask task) where TTask : class
        {
            DocumentSession.Store(BackgroundTask.Create(task));
        }
    }
}