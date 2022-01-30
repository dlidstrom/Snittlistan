
using Raven.Client;

namespace Snittlistan.Web.Infrastructure;
public interface IQuery<out TResult>
{
    TResult Execute(IDocumentSession session);
}
