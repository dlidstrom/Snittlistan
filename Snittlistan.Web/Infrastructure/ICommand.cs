namespace Snittlistan.Web.Infrastructure
{
    using System;
    using EventStoreLite;
    using Raven.Client;
    using Snittlistan.Queue.Messages;

    public interface ICommand
    {
        void Execute(IDocumentSession session, IEventStoreSession eventStoreSession, Action<ITask> publish);
    }
}
