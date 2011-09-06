using System.Linq;
using MvcContrib.TestHelper;
using SnittListan.Events;
using SnittListan.Helpers;
using SnittListan.Models;
using Xunit;

namespace SnittListan.Test
{
	public class User_ValidatePassword : DbTest
	{
		[Fact]
		public void StoredCorrectly()
		{
			var original = new User("F", "L", "e@d.com", "some pwd");
			using (DomainEvent.Disable())
			{
				original.Activate();
				Session.Store(original);
				Session.SaveChanges();
				WaitForNonStaleResults<User>();
			}

			var user = Session.FindUserByEmail("e@d.com").Single();
			user.ActivationKey.ShouldBe(original.ActivationKey);
			user.Email.ShouldBe(original.Email);
			user.FirstName.ShouldBe(original.FirstName);
			user.Id.ShouldBe(original.Id);
			user.IsActive.ShouldBe(original.IsActive);
			user.LastName.ShouldBe(original.LastName);
		}

		[Fact]
		public void CanValidatePassword()
		{
			Session.Store(new User("F", "L", "e@d.com", "some pwd"));
			Session.SaveChanges();
			WaitForNonStaleResults<User>();

			Session
				.FindUserByEmail("e@d.com")
				.Single()
				.ValidatePassword("some pwd")
				.ShouldBe(true);
		}
	}
}
