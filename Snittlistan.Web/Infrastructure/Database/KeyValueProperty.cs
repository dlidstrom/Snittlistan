#nullable enable

using Snittlistan.Queue;
using System.ComponentModel.DataAnnotations.Schema;

namespace Snittlistan.Web.Infrastructure.Database;

public class KeyValueProperty : HasVersion
{
    public KeyValueProperty(
        int tenantId,
        string key,
        object value)
    {
        TenantId = tenantId;
        Key = key;
        Value = value;
    }

    private KeyValueProperty()
    {
    }

    public int KeyValuePropertyId { get; private set; }

    public int TenantId { get; private set; }

    public string Key { get; private set; } = null!;

    [NotMapped]
    public object Value
    {
        get => ValueColumn.FromJson();
        private set => ValueColumn = value.ToJson();
    }

    [Column("value")]
    public string ValueColumn { get; private set; } = null!;

    public DateTime UpdatedDate { get; private set; }

    public void SetValue(object value)
    {
        Value = value;
        UpdatedDate = DateTime.Now;
    }
}
