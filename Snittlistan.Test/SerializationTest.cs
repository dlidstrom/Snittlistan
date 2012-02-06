namespace Snittlistan.Test
{
    using System.IO;
    using System.Text;
    using Newtonsoft.Json;
    using Snittlistan.Models;
    using Xunit;

    public class SerializationTest : DbTest
    {
        [Fact]
        public void CanSerialize8x4Match()
        {
            // Arrange
            var serializer = Store.Conventions.CreateSerializer();
            var builder = new StringBuilder();

            // Act
            serializer.Serialize(new StringWriter(builder), DbSeed.Create8x4Match());
            string text = builder.ToString();
            var match = serializer.Deserialize<Match8x4>(new JsonTextReader(new StringReader(text)));

            // Assert
            TestData.VerifyMatch(match);
        }

        [Fact]
        public void CanSerialize4x4Match()
        {
            // Arrange
            var serializer = Store.Conventions.CreateSerializer();
            var builder = new StringBuilder();

            // Act
            serializer.Serialize(new StringWriter(builder), DbSeed.Create4x4Match());
            string text = builder.ToString();
            var match = serializer.Deserialize<Match4x4>(new JsonTextReader(new StringReader(text)));

            // Assert
            TestData.VerifyTeam(match.HomeTeam);
        }
    }
}