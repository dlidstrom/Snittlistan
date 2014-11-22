using System;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Threading;
using System.Timers;
using Castle.Core;
using Castle.MicroKernel;
using NLog;
using Raven.Client;
using Raven.Client.Indexes;
using Raven.Client.Linq;
using Snittlistan.Web.Infrastructure.Indexes;
using Timer = System.Timers.Timer;

namespace Snittlistan.Web.Infrastructure.BackgroundTasks
{
    public class TaskRunner : IStartable
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private readonly IKernel kernel;
        private readonly IDocumentStore documentStore;
        private readonly Timer timer;
        private int counter;

        public TaskRunner(IKernel kernel, IDocumentStore documentStore)
        {
            this.kernel = kernel;
            this.documentStore = documentStore;

            var typeCatalog = new TypeCatalog(new[] { typeof(BackgroundTasksIndex) });
            IndexCreation.CreateIndexes(new CompositionContainer(typeCatalog), documentStore);
            timer = new Timer();
        }

        public void Start()
        {
            timer.Interval = 1000;
            timer.Elapsed += TimerOnElapsed;
            timer.Start();
        }

        public void Stop()
        {
            timer.Stop();
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            if (Interlocked.CompareExchange(ref counter, 1, 0) != 0) return;
            PerformWork();
            counter--;
        }

        private void PerformWork()
        {
            try
            {
                using (var session = documentStore.OpenSession())
                {
                    ProcessTask(session);
                    session.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Log.ErrorException(e.GetType().ToString(), e);
                Elmah.ErrorSignal.FromCurrentContext().Raise(e);
            }
        }

        private void ProcessTask(IDocumentSession session)
        {
            var task = session.Query<BackgroundTask, BackgroundTasksIndex>()
                              .Where(x => x.IsFinished == false && x.IsFailed == false)
                              .OrderBy(x => x.NextTry)
                              .FirstOrDefault();
            if (task == null) return;

            object handler = null;
            try
            {
                var handlerType = typeof(IBackgroundTaskHandler<>).MakeGenericType(task.Body.GetType());
                handler = kernel.Resolve(handlerType);
                var method = handler.GetType().GetMethod("Handle");
                method.Invoke(handler, new[] { task.Body });
                task.MarkFinished();
            }
            catch (Exception e)
            {
                Log.ErrorException(e.GetType().ToString(), e);
                Elmah.ErrorSignal.FromCurrentContext().Raise(e);
                task.UpdateNextTry(e);
            }
            finally
            {
                if (handler != null) kernel.ReleaseComponent(handler);
            }
        }
    }
}