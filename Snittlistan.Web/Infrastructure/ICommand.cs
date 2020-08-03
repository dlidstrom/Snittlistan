namespace Snittlistan.Web.Infrastructure
{
    using System;
    using EventStoreLite;
    using Raven.Client;

    public interface ICommand
    {
        void Execute(IDocumentSession session, IEventStoreSession eventStoreSession, Action<object> publish);
    }
}