namespace Snittlistan.Test
{
    using Events;
    using Helpers;
    using Models;
    using MvcContrib.TestHelper;
    using Xunit;

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

            Session
                .FindUserByEmail("e@d.com")
                .ValidatePassword("some pwd")
                .ShouldBe(true);
        }
    }
}