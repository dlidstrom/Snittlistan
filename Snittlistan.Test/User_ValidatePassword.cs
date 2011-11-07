using System.Linq;
using MvcContrib.TestHelper;
using Snittlistan.Events;
using Snittlistan.Helpers;
using Snittlistan.Models;
using Xunit;

namespace Snittlistan.Test
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

			var user = Session.FindUserByEmail("e@d.com");
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
				.ValidatePassword("some pwd")
				.ShouldBe(true);
		}
	}
}
