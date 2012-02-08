namespace Snittlistan.Test
{
    using MvcContrib.TestHelper;
    using Snittlistan.Helpers;
    using Xunit;

    public class Match_ByBitsMatchIdTest : DbTest
    {
        [Fact]
        public void ShouldFindBitsId()
        {
            // Arrange
            var match = DbSeed.Create8x4Match();
            Session.Store(match);
            Session.SaveChanges();

            // Act
            bool found = Session.BitsIdExists(3003231);

            // Assert
            found.ShouldBe(true);
        }
    }
}
