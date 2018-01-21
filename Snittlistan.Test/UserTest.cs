using NUnit.Framework;
using Snittlistan.Queue.Messages;
using Snittlistan.Web.Models;

namespace Snittlistan.Test
{
    [TestFixture]
    public class UserTest
    {
        [Test]
        public void ShouldNotBeActiveWhenCreated()
        {
            var user = new User("first name", "last name", "email", "password");
            Assert.False(user.IsActive);
        }

        [Test]
        public void ShouldRaiseEventWhenCreated()
        {
            NewUserCreatedEvent createdEvent = null;
            var user = new User("first name", "last name", "email", "password") { Id = "users-2" };
            user.Initialize(e => createdEvent = (NewUserCreatedEvent)e);

            Assert.NotNull(createdEvent);
            Assert.That(createdEvent.Email, Is.EqualTo("email"));
            Assert.That(createdEvent.UserId, Is.EqualTo("users-2"));
        }

        [Test]
        public void CanActivate()
        {
            var user = new User("F", "L", "e@d.com", "pwd");
            user.Activate();
            Assert.True(user.IsActive);
        }
    }
}