namespace Snittlistan.Test
{
    using Snittlistan.Web.Events;
    using Snittlistan.Web.Helpers;
    using Snittlistan.Web.Models;

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
            Assert.Equal(original.ActivationKey, user.ActivationKey);
            Assert.Equal(original.Email, user.Email);
            Assert.Equal(original.FirstName, user.FirstName);
            Assert.Equal(original.Id, user.Id);
            Assert.Equal(original.IsActive, user.IsActive);
            Assert.Equal(original.LastName, user.LastName);
        }

        [Fact]
        public void CanValidatePassword()
        {
            Session.Store(new User("F", "L", "e@d.com", "some pwd"));
            Session.SaveChanges();

            bool valid = Session.FindUserByEmail("e@d.com").ValidatePassword("some pwd");
            Assert.True(valid);
        }
    }
}