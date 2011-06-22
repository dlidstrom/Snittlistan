using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using SnittListan.Models;
using Castle.Windsor;
using SnittListan.Events;

namespace SnittListan.Test
{
	public class UserTest
	{
		[Fact]
		public void ShouldNotBeActiveWhenCreated()
		{
			var user = new User("first name", "last name", "username", "email", "password");
			Assert.False(user.IsActive);
		}

		[Fact]
		public void ShouldRaiseEventWhenCreated()
		{
			NewUserCreatedEvent createdEvent = null;
			using (DomainEvent.TestWith(e => createdEvent = (NewUserCreatedEvent)e))
			{
				var user = new User("first name", "last name", "username", "email", "password");
				user.Initialize();
			}

			Assert.NotNull(createdEvent);
			Assert.Equal("first name", createdEvent.User.FirstName);
			Assert.Equal("last name", createdEvent.User.LastName);
			Assert.Equal("username", createdEvent.User.UserName);
			Assert.Equal("email", createdEvent.User.Email);
			Assert.False(createdEvent.User.IsActive);
		}
	}
}
