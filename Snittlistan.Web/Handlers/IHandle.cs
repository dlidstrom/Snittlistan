namespace Snittlistan.Web.Handlers
{
    public interface IHandle<TEvent>
    {
        void Handle(TEvent @event);
    }
}