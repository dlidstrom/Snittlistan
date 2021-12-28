using EventStoreLite;
using Raven.Client;
using Snittlistan.Queue.Messages;

#nullable enable

namespace Snittlistan.Web.Infrastructure;
public interface ICommand
{
    Task Execute(
        IDocumentSession session,
        IEventStoreSession eventStoreSession,
        Action<TaskBase> publish);
}
