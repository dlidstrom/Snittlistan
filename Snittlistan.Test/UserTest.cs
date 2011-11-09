namespace Snittlistan.Test
{
    using MvcContrib.TestHelper;
    using Snittlistan.Events;
    using Snittlistan.Models;
    using Xunit;

    public class UserTest
    {
        [Fact]
        public void ShouldNotBeActiveWhenCreated()
        {
            var user = new User("first name", "last name", "email", "password");
            user.IsActive.ShouldBe(false);
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

            createdEvent.ShouldNotBeNull("Event not raised");
            createdEvent.User.FirstName.ShouldBe("first name");
            createdEvent.User.LastName.ShouldBe("last name");
            createdEvent.User.Email.ShouldBe("email");
            createdEvent.User.IsActive.ShouldBe(false);
        }

        [Fact]
        public void CanActivate()
        {
            var user = new User("F", "L", "e@d.com", "pwd");
            user.Activate();
            user.IsActive.ShouldBe(true);
        }
    }
}