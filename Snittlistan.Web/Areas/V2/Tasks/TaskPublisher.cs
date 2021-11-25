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
    using Snittlistan.Web.Infrastructure;
    using Snittlistan.Web.Infrastructure.Database;

    public class TaskPublisher
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public Databases Databases { get; set; } = null!;

        public async Task PublishTask(ITask task, string createdBy)
        {
            await DoPublishDelayedTask(task, DateTime.MinValue, createdBy);
        }

        public async Task PublishDelayedTask(ITask task, TimeSpan sendAfter, string createdBy)
        {
            DateTime publishDate = DateTime.Now.Add(sendAfter);
            await DoPublishDelayedTask(task, publishDate, createdBy);
        }

        protected async Task<Tenant> GetCurrentTenant()
        {
            string hostname = CurrentHttpContext.Instance().Request.ServerVariables["SERVER_NAME"];
            Tenant tenant = await Databases.Snittlistan.Tenants.SingleOrDefaultAsync(x => x.Hostname == hostname);
            if (tenant == null)
            {
                throw new Exception($"No tenant found for hostname '{hostname}'");
            }

            return tenant;
        }

        private async Task DoPublishDelayedTask(ITask task, DateTime publishDate, string createdBy)
        {
            string businessKey = task.BusinessKey.ToString();
            Tenant tenant = await GetCurrentTenant();
            DelayedTask delayedTask = Databases.Snittlistan.DelayedTasks.Add(new(
                task,
                publishDate,
                tenant.TenantId,
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
