namespace Snittlistan.Test
{
	using Newtonsoft.Json;
	using Snittlistan.Models;
	using Xunit;

	public class SerializationTest
	{
		[Fact]
		public void CanSerializeMatch()
		{
			// Arrange
			string output = JsonConvert.SerializeObject(DbSeed.CreateMatch());

			// Act
			var match = JsonConvert.DeserializeObject<Match>(output);

			// Assert
			TestData.VerifyMatch(match);
		}
	}
}
