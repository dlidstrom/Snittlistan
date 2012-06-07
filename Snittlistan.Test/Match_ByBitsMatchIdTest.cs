namespace Snittlistan.Test
{
    using Helpers;
    using MvcContrib.TestHelper;
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
