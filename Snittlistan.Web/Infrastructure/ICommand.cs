#nullable enable

namespace Snittlistan.Web.Infrastructure
{
    using System;
    using System.Threading.Tasks;
    using EventStoreLite;
    using Raven.Client;
    using Snittlistan.Queue.Messages;

    public interface ICommand
    {
        Task Execute(
            IDocumentSession session,
            IEventStoreSession eventStoreSession,
            Func<TaskBase, Task> publish);
    }
}
