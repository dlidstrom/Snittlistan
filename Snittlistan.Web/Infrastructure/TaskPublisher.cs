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

    public async Task PublishTask(TaskBase task, string createdBy)
    {
        string businessKeyJson = task.BusinessKey.ToJson();
        int taskType = Enumerable.Aggregate(
            businessKeyJson,
            (ushort)5381, (l, r) => (ushort)((33 * l) ^ r));
        IQueryable<PublishedTask> query =
            from publishedTask in databases.Snittlistan.PublishedTasks
            where publishedTask.HandledDate == null
                && publishedTask.TaskType == taskType
            select publishedTask;
        PublishedTask? newOrExistingTask = await query.SingleOrDefaultAsync();
        if (newOrExistingTask is null)
        {
            newOrExistingTask = databases.Snittlistan.PublishedTasks.Add(
                PublishedTask.CreateImmediate(
                    task,
                    businessKeyJson,
                    taskType,
                    currentTenant.TenantId,
                    correlationId,
                    causationId,
                    createdBy));
        }
        else
        {
            Logger.Info("task exists, do not publish: {@task}", task);
            return;
        }

        try
        {
            HostingEnvironment.QueueBackgroundWorkItem(ct =>
                PublishMessage(currentTenant, newOrExistingTask.MessageId, ct));
        }
        catch (Exception ex)
        {
            Logger.Error(
                ex,
                "QueueBackgroundWorkItem failed, using fallback (publish immediately)");
            try
            {
                DoPublishMessage(currentTenant, newOrExistingTask);
            }
            catch (Exception ex2)
            {
                Logger.Error(ex2);
            }
        }

        static async void PublishMessage(Tenant tenant, Guid messageId, CancellationToken ct)
        {
            const int MaxTries = 10;
            using IDisposable logScope = NestedDiagnosticsLogicalContext.Push("QueueBackgroundWork");
            using SnittlistanContext context = new();
            try
            {
                for (int i = 0; i < MaxTries; i++)
                {
                    Logger.Info("try #{try}", i);
                    PublishedTask? publishedTask =
                        await context.PublishedTasks.SingleOrDefaultAsync(x => x.MessageId == messageId);
                    if (publishedTask == null)
                    {
                        Logger.Warn("message not found: {messageId}", messageId);
                        await Task.Delay(300);
                    }
                    else
                    {
                        DoPublishMessage(tenant, publishedTask);
                        return;
                    }
                }

                throw new Exception($"max tries reached: {MaxTries}");
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
            scope.Send(message);
            scope.Commit();
            Logger.Info("published message {@message}", message);
        }
    }

    public async Task PublishDelayedTask(TaskBase task, DateTime publishDate, string createdBy)
    {
        string businessKeyJson = task.BusinessKey.ToJson();
        int taskType = Enumerable.Aggregate(
            businessKeyJson,
            (ushort)5381, (l, r) => (ushort)((33 * l) ^ r));
        IQueryable<PublishedTask> query =
            from publishedTask in databases.Snittlistan.PublishedTasks
            where publishedTask.HandledDate == null
                && publishedTask.TaskType == taskType
            select publishedTask;
        PublishedTask? newOrExistingTask = await query.SingleOrDefaultAsync();
        if (newOrExistingTask is null)
        {
            PublishedTask publishedTask = databases.Snittlistan.PublishedTasks.Add(
                PublishedTask.CreateDelayed(
                    task,
                    businessKeyJson,
                    taskType,
                    currentTenant.TenantId,
                    correlationId,
                    causationId,
                    publishDate,
                    createdBy));
            Logger.Info("added delayed task: {@publishedTask}", publishedTask);
        }
        else
        {
            Logger.Info("task exists, do not publish: {@task}", task);
        }
    }
}
