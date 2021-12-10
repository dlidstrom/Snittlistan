using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Snittlistan.Queue.Messages;

#nullable enable

namespace Snittlistan.Web.Infrastructure.Database;
public class PublishedTask
{
    private readonly JsonSerializerSettings settings = new()
    {
        TypeNameHandling = TypeNameHandling.All
    };

    public PublishedTask(
        TaskBase task,
        int tenantId,
        Guid correlationId,
        Guid? causationId,
        Guid messageId,
        string createdBy)
    {
        Task = task;
        BusinessKey = task.BusinessKey;
        TenantId = tenantId;
        CorrelationId = correlationId;
        CausationId = causationId;
        MessageId = messageId;
        CreatedBy = createdBy;
        CreatedDate = DateTime.Now;
    }

    private PublishedTask()
    {
    }

    public int PublishedTaskId { get; private set; }

    public int TenantId { get; private set; }

    public Guid CorrelationId { get; private set; }

    public Guid? CausationId { get; private set; }

    public Guid MessageId { get; private set; }

    public string CreatedBy { get; private set; } = null!;

    [NotMapped]
    public BusinessKey BusinessKey
    {
        get => JsonConvert.DeserializeObject<BusinessKey>(BusinessKeyColumn, settings)!;
        private set => BusinessKeyColumn = JsonConvert.SerializeObject(value, settings);
    }

    [NotMapped]
    public TaskBase Task
    {
        get => (TaskBase)JsonConvert.DeserializeObject(DataColumn, settings)!;
        private set => DataColumn = JsonConvert.SerializeObject(value, settings);
    }

    [Column("BusinessKey")]
    public string BusinessKeyColumn { get; private set; } = null!;

    [Column("Data")]
    public string DataColumn { get; private set; } = null!;

    public DateTime CreatedDate { get; private set; }

    public DateTime? HandledDate { get; private set; }

    public string Version { get; private set; } = null!;

    public void MarkHandled(DateTime when)
    {
        HandledDate = when;
    }
}
