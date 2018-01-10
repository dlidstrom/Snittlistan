using System;
using Castle.Windsor;
using NUnit.Framework;
using Snittlistan.Web.DomainEvents;
using Snittlistan.Web.Models;

namespace Snittlistan.Test
{
    [TestFixture]
    public class UserTest : IDisposable
    {
        private readonly IWindsorContainer oldContainer;

        public UserTest()
        {
            oldContainer = DomainEvent.SetContainer(new WindsorContainer());
        }

        public void Dispose()
        {
            DomainEvent.SetContainer(oldContainer);
        }

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
            using (DomainEvent.TestWith(e => createdEvent = (NewUserCreatedEvent)e))
            {
                var user = new User("first name", "last name", "email", "password");
                user.Initialize();
            }

            Assert.NotNull(createdEvent);
            Assert.That(createdEvent.User.FirstName, Is.EqualTo("first name"));
            Assert.That(createdEvent.User.LastName, Is.EqualTo("last name"));
            Assert.That(createdEvent.User.Email, Is.EqualTo("email"));
            Assert.False(createdEvent.User.IsActive);
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