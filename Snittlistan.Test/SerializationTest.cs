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
		public void CanSerializeMatch()
		{
			// Arrange
			var serializer = Store.Conventions.CreateSerializer();
			var builder = new StringBuilder();

			// Act
			serializer.Serialize(new StringWriter(builder), DbSeed.CreateMatch());
			var match = serializer.Deserialize<Match>(new JsonTextReader(new StringReader(builder.ToString())));

			// Assert
			TestData.VerifyMatch(match);
		}
	}
}