#nullable enable

namespace Snittlistan.Web.Areas.V2.Tasks
{
    using System;
    using System.Threading;
    using System.Web.Hosting;
    using NLog;
    using Snittlistan.Queue;
    using Snittlistan.Queue.Messages;
    using Snittlistan.Web.Infrastructure.Database;

    public class TaskPublisher
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly Tenant currentTenant;
        private readonly Databases databases;
        private readonly Guid correlationId;
        private readonly Guid? causationId;

        public TaskPublisher(
            Tenant currentTenant,
            Databases databases,
            Guid correlationId,
            Guid? causationId)
        {
            this.currentTenant = currentTenant;
            this.databases = databases;
            this.correlationId = correlationId;
            this.causationId = causationId;
        }

        public void PublishTask(TaskBase task, string createdBy)
        {
            PublishedTask publishedTask = databases.Snittlistan.PublishedTasks.Add(new(
                task,
                currentTenant.TenantId,
                correlationId,
                causationId,
                Guid.NewGuid(),
                createdBy));

            try
            {
                HostingEnvironment.QueueBackgroundWorkItem(ct => PublishMessage(currentTenant, publishedTask, ct));
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "QueueBackgroundWorkItem failed, using fallback");
                CancellationTokenSource tokenSource = new(10000);
                CancellationToken cancellationToken = tokenSource.Token;
                PublishMessage(currentTenant, publishedTask, cancellationToken);
            }

            static void PublishMessage(Tenant tenant, PublishedTask publishedTask, CancellationToken ct)
            {
                try
                {
                    using MsmqGateway.MsmqTransactionScope scope = MsmqGateway.AutoCommitScope();
                    MessageEnvelope message = new(
                        publishedTask.Task,
                        publishedTask.TenantId,
                        tenant.Hostname,
                        publishedTask.CorrelationId,
                        publishedTask.CausationId,
                        publishedTask.MessageId);
                    scope.PublishMessage(message);
                    scope.Commit();
                    Logger.Info("published message {@message}", message);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "failed to publish message");
                }
            }
        }

        public void PublishDelayedTask(TaskBase task, TimeSpan sendAfter, string createdBy)
        {
            DateTime publishDate = DateTime.Now.Add(sendAfter);
            DelayedTask delayedTask = databases.Snittlistan.DelayedTasks.Add(new(
                task,
                publishDate,
                currentTenant.TenantId,
                correlationId,
                null,
                Guid.NewGuid(),
                createdBy));
            Logger.Info("added delayed task: {@delayedTask}", delayedTask);
        }
    }
}
