using System;
using Castle.MicroKernel;
using Raven.Client;

namespace Snittlistan.Web.Infrastructure.BackgroundTasks
{
    public abstract class BackgroundTaskHandler<TTask> : IBackgroundTaskHandler<TTask>
    {
        private string database;

        public IKernel Kernel { get; set; }

        public void Handle(TTask task, TenantConfiguration tenantConfiguration)
        {
            database = tenantConfiguration.Database;
            Handle(task);
        }

        protected abstract void Handle(TTask task);

        protected TResult Transact<TResult>(Func<IDocumentSession, TResult> action)
        {
            var documentStore = Kernel.Resolve<IDocumentStore>();
            using (var session = documentStore.OpenSession(database))
            {
                var result = action.Invoke(session);
                session.SaveChanges();
                return result;
            }
        }

        protected void Transact(Action<IDocumentSession> action)
        {
            Transact(session =>
            {
                action.Invoke(session);
                return false;
            });
        }
    }
}