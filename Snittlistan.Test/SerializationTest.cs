#nullable enable

namespace Snittlistan.Test
{
    using System.IO;
    using System.Text;
    using NUnit.Framework;
    using Raven.Client;
    using Raven.Imports.Newtonsoft.Json;
    using Snittlistan.Web.Areas.V1.Models;
    using Snittlistan.Web.Models;

    [TestFixture]
    public class SerializationTest : DbTest
    {
        [Test]
        public void CanSerialize8X4Match()
        {
            // Arrange
            JsonSerializer serializer = Store.Conventions.CreateSerializer();
            StringBuilder builder = new();

            // Act
            serializer.Serialize(new StringWriter(builder), DbSeed.Create8x4Match());
            string text = builder.ToString();
            Match8x4 match = serializer.Deserialize<Match8x4>(new JsonTextReader(new StringReader(text)));

            // Assert
            TestData.VerifyTeam(match.AwayTeam);
        }

        [Test]
        public void CanSerialize4X4Match()
        {
            // Arrange
            JsonSerializer serializer = Store.Conventions.CreateSerializer();
            StringBuilder builder = new();

            // Act
            serializer.Serialize(new StringWriter(builder), DbSeed.Create4x4Match());
            string text = builder.ToString();
            Match4x4 match = serializer.Deserialize<Match4x4>(new JsonTextReader(new StringReader(text)));

            // Assert
            TestData.VerifyTeam(match.HomeTeam);
        }

        [Test]
        public void CanSerializeUser()
        {
            // Arrange
            User user = new("firstName", "lastName", "e@d.com", "some-pass");

            // Act
            using (IDocumentSession session = Store.OpenSession())
            {
                session.Store(user);
                session.SaveChanges();
            }

            // Assert
            using (IDocumentSession session = Store.OpenSession())
            {
                User loadedUser = session.Load<User>(user.Id);
                Assert.True(loadedUser.ValidatePassword("some-pass"), "Password validation failed");
            }
        }
    }
}
