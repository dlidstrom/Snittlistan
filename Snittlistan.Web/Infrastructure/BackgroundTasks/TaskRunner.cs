using System;
using System.Linq;
using System.Threading;
using System.Timers;
using System.Web.Hosting;
using Castle.MicroKernel;
using Elmah;
using NLog;
using Raven.Client;
using Raven.Client.Linq;
using Snittlistan.Web.Infrastructure.Indexes;
using Timer = System.Timers.Timer;

namespace Snittlistan.Web.Infrastructure.BackgroundTasks
{
    public class TaskRunner : IRegisteredObject
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private readonly IKernel kernel;
        private readonly IDocumentStore documentStore;
        private readonly Timer timer;
        private readonly object locker = new object();
        private readonly AutoResetEvent resetEvent = new AutoResetEvent(true);

        public TaskRunner(IKernel kernel, IDocumentStore documentStore, int taskRunnerPollingInterval)
        {
            this.kernel = kernel;
            this.documentStore = documentStore;

            timer = new Timer
            {
                Interval = taskRunnerPollingInterval
            };
            timer.Elapsed += TimerOnElapsed;
            timer.Start();

            Log.Info("Registering with HostingEnvironment");
            HostingEnvironment.RegisterObject(this);
        }

        public void Stop(bool immediate)
        {
            try
            {
                timer.Stop();
                resetEvent.WaitOne(10000);
            }
            catch (Exception e)
            {
                Log.Error(e, e.GetType().ToString());
            }

            Log.Info("Unregistering from HostingEnvironment");
            HostingEnvironment.UnregisterObject(this);
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            if (Monitor.IsEntered(locker))
            {
                Log.Error("Task being processed, exiting");
                return;
            }

            lock (locker)
            {
                try
                {
                    resetEvent.Reset();
                    PerformWork();
                    resetEvent.Set();
                }
                catch (Exception e)
                {
                    Log.Error(e, e.GetType().ToString());
                }
            }
        }

        private void PerformWork()
        {
            try
            {
                using (var session = documentStore.OpenSession())
                {
                    if (ProcessTask(session))
                        session.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Log.Error(e, e.GetType().ToString());
                ErrorLog.GetDefault(null).Log(new Error(e));
            }
        }

        private bool ProcessTask(IDocumentSession session)
        {
            var task = session.Query<BackgroundTask, BackgroundTasksIndex>()
                              .Where(x => x.IsFinished == false && x.IsFailed == false)
                              .OrderBy(x => x.NextTry)
                              .FirstOrDefault();
            if (task == null) return false;

            object handler = null;
            try
            {
                Log.Info("Handling task {0}", task.GetInfo());
                var handlerType = typeof(IBackgroundTaskHandler<>).MakeGenericType(task.Body.GetType());
                handler = kernel.Resolve(handlerType);
                var method = handler.GetType().GetMethod("Handle");
                method.Invoke(handler, new[] { task.Body });
                task.MarkFinished();
                Log.Info("Task finished successfully");
            }
            catch (Exception e)
            {
                Log.Error(e, e.GetType().ToString());
                ErrorLog.GetDefault(null).Log(new Error(e));
                task.UpdateNextTry(e);
                Log.Info("Task failed attempt #{0}", task.Retries);
            }
            finally
            {
                if (handler != null) kernel.ReleaseComponent(handler);
            }

            return true;
        }
    }
}