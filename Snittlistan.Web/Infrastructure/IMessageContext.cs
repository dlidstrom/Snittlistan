#nullable enable

namespace Snittlistan.Web.Infrastructure;

public interface IMessageContext
{
    PublishMessageDelegate PublishMessageDelegate { get; set; }
}
