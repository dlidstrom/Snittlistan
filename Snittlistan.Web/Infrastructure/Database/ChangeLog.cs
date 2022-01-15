#nullable enable

using Newtonsoft.Json;
using Snittlistan.Queue;
using System.ComponentModel.DataAnnotations.Schema;

namespace Snittlistan.Web.Infrastructure.Database;

public class ChangeLog
{
    public ChangeLog(
        int tenantId,
        Guid correlationId,
        Guid? causationId,
        object command,
        string createdBy)
    {
        TenantId = tenantId;
        CorrelationId = correlationId;
        CausationId = causationId;
        CommandType = command.GetType().FullName;
        Data = command.ToJson();
        CreatedBy = createdBy;
    }

    private ChangeLog()
    {
    }

    public int ChangeLogId { get; private set; }

    public int TenantId { get; private set; }

    public Guid CorrelationId { get; private set; }

    public Guid? CausationId { get; private set; }

    public string CommandType { get; private set; } = null!;

    public string Data { get; private set; } = null!;

    public string CreatedBy { get; private set; } = null!;

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime CreatedDate { get; private set; }
}
