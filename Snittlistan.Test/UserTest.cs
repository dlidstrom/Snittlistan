using System;
using Castle.Windsor;
using Snittlistan.Web.DomainEvents;
using Snittlistan.Web.Models;
using Xunit;

namespace Snittlistan.Test
{
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

        [Fact]
        public void CanActivate()
        {
            var user = new User("F", "L", "e@d.com", "pwd");
            user.Activate();
            Assert.True(user.IsActive);
        }
    }
}