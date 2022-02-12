#nullable enable

using Raven.Client.Documents.Session;

namespace Snittlistan.Web.Infrastructure;

public interface IQuery<out TResult>
{
    TResult Execute(IDocumentSession session);
}
