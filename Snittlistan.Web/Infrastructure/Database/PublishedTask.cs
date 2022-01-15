#nullable enable

using System.ComponentModel.DataAnnotations.Schema;
using Snittlistan.Queue;
using Snittlistan.Queue.Messages;

namespace Snittlistan.Web.Infrastructure.Database;

public class PublishedTask
{
    private PublishedTask(
        TaskBase task,
        int tenantId,
        Guid correlationId,
        Guid? causationId,
        DateTime publishDate,
        string createdBy)
    {
        Task = task;
        BusinessKey = task.BusinessKey;
        TenantId = tenantId;
        CorrelationId = correlationId;
        CausationId = causationId;
        MessageId = Guid.NewGuid();
        PublishDate = publishDate;
        CreatedBy = createdBy;
    }

    private PublishedTask()
    {
    }

    public static PublishedTask CreateImmediate(
        TaskBase task,
        int tenantId,
        Guid correlationId,
        Guid? causationId,
        string createdBy)
    {
        return new(
            task,
            tenantId,
            correlationId,
            causationId,
            DateTime.Now,
            createdBy);
    }

    public static PublishedTask CreateDelayed(
        TaskBase task,
        int tenantId,
        Guid correlationId,
        Guid? causationId,
        DateTime publishDate,
        string createdBy)
    {
        return new(
            task,
            tenantId,
            correlationId,
            causationId,
            publishDate,
            createdBy);
    }

    public int PublishedTaskId { get; private set; }

    public int TenantId { get; private set; }

    public virtual Tenant Tenant { get; private set; } = null!;

    public Guid CorrelationId { get; private set; }

    public Guid? CausationId { get; private set; }

    public Guid? MessageId { get; private set; }

    [NotMapped]
    public BusinessKey BusinessKey
    {
        get => BusinessKeyColumn.FromJson<BusinessKey>();
        private set => BusinessKeyColumn = value.ToJson();
    }

    [NotMapped]
    public TaskBase Task
    {
        get => DataColumn.FromJson<TaskBase>();
        private set => DataColumn = value.ToJson();
    }

    [Column("business_key")]
    public string BusinessKeyColumn { get; private set; } = null!;

    [Column("data")]
    public string DataColumn { get; private set; } = null!;

    /// <summary>
    /// When to publish (if delayed, otherwise use DateTime.Now)
    /// </summary>
    public DateTime? PublishDate { get; private set; }

    /// <summary>
    /// When it was published.
    /// </summary>
    public DateTime? PublishedDate { get; private set; }

    /// <summary>
    /// When it was handled.
    /// </summary>
    public DateTime? HandledDate { get; private set; }

    public string CreatedBy { get; private set; } = null!;

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime CreatedDate { get; private set; }

    public string Version { get; private set; } = null!;

    public void MarkPublished(DateTime when)
    {
        PublishedDate = when;
    }

    public void MarkHandled(DateTime when)
    {
        HandledDate = when;
    }
}
