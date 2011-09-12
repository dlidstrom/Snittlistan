using Snittlistan.Models;

namespace Snittlistan.Events
{
	public class NewUserCreatedEvent : IDomainEvent
	{
		public User User { get; set; }
	}
}
