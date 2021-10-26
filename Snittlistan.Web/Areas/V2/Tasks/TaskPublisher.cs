#nullable enable

namespace Snittlistan.Web.Areas.V2.Tasks
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Hosting;
    using NLog;
    using Raven.Client;
    using Snittlistan.Queue;
    using Snittlistan.Queue.Messages;
    using Snittlistan.Queue.Models;
    using Snittlistan.Web.Infrastructure.Database;

    public class TaskPublisher
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public DatabaseContext Database { get; set; } = null!;

        public TenantConfiguration TenantConfiguration { get; set; } = null!;

        public void PublishTask(ITask task)
        {
            DoPublishDelayedTask(task, DateTime.MinValue);
        }

        private void DoPublishDelayedTask(ITask task, DateTime publishDate)
        {
            string businessKey = task.BusinessKey.ToString();
            DelayedTask delayedTask = Database.DelayedTasks.Add(new(
                task,
                publishDate,
                TenantConfiguration.TenantId,
                CorrelationId,
                null,
                Guid.NewGuid()));
            Logger.Info("added delayed task: {@delayedTask}", delayedTask);
            HostingEnvironment.QueueBackgroundWorkItem(async ct => await PublishMessage(businessKey, ct));

            async Task PublishMessage(string businessKey, CancellationToken ct)
            {
                try
                {
                    using MsmqGateway.MsmqTransactionScope scope = MsmqGateway.AutoCommitScope();
                    using DatabaseContext context = new();
                    DelayedTask delayedTask = await context.DelayedTasks.SingleOrDefaultAsync(x => x.BusinessKeyColumn == businessKey, ct);
                    MessageEnvelope message = new(
                        delayedTask.Task,
                        delayedTask.TenantId,
                        delayedTask.CorrelationId,
                        delayedTask.CausationId,
                        delayedTask.MessageId);
                    scope.PublishMessage(message);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "failed to publish message");
                }
            }
        }

        private Guid CorrelationId
        {
            get
            {
                if (HttpContext.Current.Items["CorrelationId"] is Guid correlationId)
                {
                    return correlationId;
                }

                correlationId = Guid.NewGuid();
                HttpContext.Current.Items["CorrelationId"] = correlationId;
                return correlationId;
            }
        }
    }
}
