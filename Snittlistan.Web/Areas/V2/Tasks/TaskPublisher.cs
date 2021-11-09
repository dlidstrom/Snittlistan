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

        public Databases Databases { get; set; } = null!;

        public TenantConfiguration TenantConfiguration { get; set; } = null!;

        public void PublishTask(ITask task, string createdBy)
        {
            DoPublishDelayedTask(task, DateTime.MinValue, createdBy);
        }

        public void PublishDelayedTask(ITask task, TimeSpan sendAfter, string createdBy)
        {
            DateTime publishDate = DateTime.Now.Add(sendAfter);
            DoPublishDelayedTask(task, publishDate, createdBy);
        }

        private void DoPublishDelayedTask(ITask task, DateTime publishDate, string createdBy)
        {
            string businessKey = task.BusinessKey.ToString();
            DelayedTask delayedTask = Databases.Snittlistan.DelayedTasks.Add(new(
                task,
                publishDate,
                TenantConfiguration.TenantId,
                CorrelationId,
                null,
                Guid.NewGuid(),
                createdBy));
            Logger.Info("added delayed task: {@delayedTask}", delayedTask);
            HostingEnvironment.QueueBackgroundWorkItem(async ct => await PublishMessage(businessKey, ct));

            async Task PublishMessage(string businessKey, CancellationToken ct)
            {
                try
                {
                    using MsmqGateway.MsmqTransactionScope scope = MsmqGateway.AutoCommitScope();
                    using SnittlistanContext context = new();
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
