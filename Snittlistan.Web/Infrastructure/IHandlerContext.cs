#nullable enable

using Snittlistan.Queue.Messages;

namespace Snittlistan.Web.Infrastructure;

public delegate void PublishMessageDelegate(TaskBase task, DateTime? publishDate = null);

public interface IHandlerContext
{
    PublishMessageDelegate PublishMessage { get; set; }
}
