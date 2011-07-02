using SnittListan.Events;
using SnittListan.Models;
using Xunit;

namespace SnittListan.Test
{
	public class UserTest
	{
		[Fact]
		public void ShouldNotBeActiveWhenCreated()
		{
			var user = new User("first name", "last name", "email", "password");
			Assert.False(user.IsActive);
		}

		[Fact]
		public void ShouldRaiseEventWhenCreated()
		{
			NewUserCreatedEvent createdEvent = null;
			using (DomainEvent.TestWith(e => createdEvent = (NewUserCreatedEvent)e))
			{
				var user = new User("first name", "last name", "email", "password");
				user.Initialize();
			}

			Assert.NotNull(createdEvent);
			Assert.Equal("first name", createdEvent.User.FirstName);
			Assert.Equal("last name", createdEvent.User.LastName);
			Assert.Equal("email", createdEvent.User.Email);
			Assert.False(createdEvent.User.IsActive);
		}
	}
}
