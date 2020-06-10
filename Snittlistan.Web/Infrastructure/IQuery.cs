namespace Snittlistan.Web.Infrastructure
{
    using Raven.Client;

    public interface IQuery<out TResult>
    {
        TResult Execute(IDocumentSession session);
    }
}