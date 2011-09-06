namespace Snittlistan.Handlers
{
	public interface IHandle<TEvent>
	{
		void Handle(TEvent @event);
	}
}