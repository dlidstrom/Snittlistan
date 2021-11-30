
using NUnit.Framework;
using Snittlistan.Web.Helpers;
using Snittlistan.Web.Models;

#nullable enable

namespace Snittlistan.Test;
[TestFixture]
public class User_ValidatePassword : DbTest
{
    [Test]
    public void StoredCorrectly()
    {
        User original = new("F", "L", "e@d.com", "some pwd");
        original.Activate();
        Session.Store(original);
        Session.SaveChanges();

        User user = Session.FindUserByEmail("e@d.com");
        Assert.That(user.ActivationKey, Is.EqualTo(original.ActivationKey));
        Assert.That(user.Email, Is.EqualTo(original.Email));
        Assert.That(user.FirstName, Is.EqualTo(original.FirstName));
        Assert.That(user.Id, Is.EqualTo(original.Id));
        Assert.That(user.IsActive, Is.EqualTo(original.IsActive));
        Assert.That(user.LastName, Is.EqualTo(original.LastName));
    }

    [Test]
    public void CanValidatePassword()
    {
        Session.Store(new User("F", "L", "e@d.com", "some pwd"));
        Session.SaveChanges();

        bool valid = Session.FindUserByEmail("e@d.com").ValidatePassword("some pwd");
        Assert.True(valid);
    }
}
