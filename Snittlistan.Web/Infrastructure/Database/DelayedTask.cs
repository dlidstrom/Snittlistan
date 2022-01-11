#nullable enable

using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Snittlistan.Queue.Messages;

namespace Snittlistan.Web.Infrastructure.Database;

public class DelayedTask
{
    private readonly JsonSerializerSettings settings = new()
    {
        TypeNameHandling = TypeNameHandling.All,
        MetadataPropertyHandling = MetadataPropertyHandling.ReadAhead
    };

    public DelayedTask(
        TaskBase task,
        DateTime publishDate,
        int tenantId,
        Guid correlationId,
        Guid? causationId,
        Guid messageId,
        string createdBy)
    {
        BusinessKey = task.BusinessKey;
        Task = task;
        PublishDate = publishDate;
        TenantId = tenantId;
        CorrelationId = correlationId;
        CausationId = causationId;
        MessageId = messageId;
        CreatedBy = createdBy;
        CreatedDate = DateTime.Now;
    }

    private DelayedTask()
    {
    }

    public int DelayedTaskId { get; private set; }

    public int TenantId { get; private set; }

    public Guid CorrelationId { get; private set; }

    public Guid? CausationId { get; private set; }

    public Guid MessageId { get; private set; }

    public string CreatedBy { get; private set; } = null!;

    public string Version { get; private set; } = null!;

    [NotMapped]
    public BusinessKey BusinessKey
    {
        get => JsonConvert.DeserializeObject<BusinessKey>(BusinessKeyColumn)!;
        private set => BusinessKeyColumn = JsonConvert.SerializeObject(value, settings);
    }

    [NotMapped]
    public TaskBase Task
    {
        get => (TaskBase)JsonConvert.DeserializeObject(DataColumn, settings)!;
        private set => DataColumn = JsonConvert.SerializeObject(value, settings);
    }

    public DateTime PublishDate { get; private set; }

    public DateTime? PublishedDate { get; private set; }

    public DateTime CreatedDate { get; private set; }

    [Column("business_key")]
    public string BusinessKeyColumn { get; private set; } = null!;

    [Column("data")]
    public string DataColumn { get; private set; } = null!;

    public void MarkAsPublished(DateTime timestamp)
    {
        PublishedDate = timestamp;
    }
}
