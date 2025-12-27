using NUnit.Framework;
using Snittlistan.Queue.Messages;
using Snittlistan.Web.Models;

#nullable enable

namespace Snittlistan.Test;
[TestFixture]
public class UserTest
{
    [Test]
    public void ShouldNotBeActiveWhenCreated()
    {
        User user = new("first name", "last name", "email", "password");
        Assert.That(user.IsActive, Is.False);
    }

    [Test]
    public void ShouldRaiseEventWhenCreated()
    {
        NewUserCreatedTask? createdEvent = null;
        User user = new("first name", "last name", "email", "password") { Id = "users-2" };
        user.Initialize(e => createdEvent = (NewUserCreatedTask)e);

        Assert.That(createdEvent, Is.Not.Null);
        Assert.That(createdEvent!.Email, Is.EqualTo("email"));
        Assert.That(createdEvent.UserId, Is.EqualTo("users-2"));
    }

    [Test]
    public void CanActivate()
    {
        User user = new("F", "L", "e@d.com", "pwd");
        user.Activate();
        Assert.That(user.IsActive, Is.True);
    }
}
