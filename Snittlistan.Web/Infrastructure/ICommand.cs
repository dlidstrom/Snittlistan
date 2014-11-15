using EventStoreLite;
using Raven.Client;

namespace Snittlistan.Web.Infrastructure
{
    public interface ICommand
    {
        void Execute(IDocumentSession session, IEventStoreSession eventStoreSession);
    }
}