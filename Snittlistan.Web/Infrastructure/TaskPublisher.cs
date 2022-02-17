#nullable enable

using System.Data.Entity;
using System.Web.Hosting;
using NLog;
using Snittlistan.Queue;
using Snittlistan.Queue.Messages;
using Snittlistan.Web.Infrastructure.Database;

namespace Snittlistan.Web.Infrastructure;

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
        PublishedTask publishedTask = databases.Snittlistan.PublishedTasks.Add(
            PublishedTask.CreateImmediate(
                task,
                currentTenant.TenantId,
                correlationId,
                causationId,
                createdBy));

        try
        {
            HostingEnvironment.QueueBackgroundWorkItem(ct =>
                PublishMessage(currentTenant, publishedTask.MessageId, ct));
        }
        catch (Exception ex)
        {
            Logger.Error(ex, "QueueBackgroundWorkItem failed, using fallback (publish immediately)");
            DoPublishMessage(currentTenant, publishedTask);
        }

        static async void PublishMessage(Tenant tenant, Guid messageId, CancellationToken ct)
        {
            await Task.Delay(2000);
            using IDisposable logScope = NestedDiagnosticsLogicalContext.Push("QueueBackgroundWork");
            try
            {
                using SnittlistanContext context = new();
                PublishedTask publishedTask =
                    await context.PublishedTasks.SingleAsync(x => x.MessageId == messageId);
                DoPublishMessage(tenant, publishedTask);
                int changesSaved = await context.SaveChangesAsync();
                if (changesSaved > 0)
                {
                    Logger.Info(
                        "saved {changesSaved} to database",
                        changesSaved);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "failed to publish message");
            }
        }

        static void DoPublishMessage(Tenant tenant, PublishedTask publishedTask)
        {
            using MsmqGateway.MsmqTransactionScope scope = MsmqGateway.Create();
            MessageEnvelope message = new(
                publishedTask.Task,
                publishedTask.TenantId,
                tenant.Hostname,
                publishedTask.CorrelationId,
                publishedTask.CausationId,
                publishedTask.MessageId);
            publishedTask.MarkPublished(DateTime.Now);
            scope.Send(message);
            scope.Commit();
            Logger.Info("published message {@message}", message);
        }
    }

    public void PublishDelayedTask(TaskBase task, DateTime publishDate, string createdBy)
    {
        PublishedTask publishedTask = databases.Snittlistan.PublishedTasks.Add(
            PublishedTask.CreateDelayed(
                task,
                currentTenant.TenantId,
                correlationId,
                causationId,
                publishDate,
                createdBy));
        Logger.Info("added delayed task: {@publishedTask}", publishedTask);
    }
}
