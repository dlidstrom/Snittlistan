namespace Snittlistan.Web.Handlers
{
    public interface IHandle<in TEvent>
    {
        void Handle(TEvent @event);
    }
}