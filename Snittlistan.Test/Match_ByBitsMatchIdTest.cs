namespace Snittlistan.Test
{
    using NUnit.Framework;
    using Snittlistan.Web.Helpers;
    using Web.Areas.V1.Models;

    [TestFixture]
    public class Match_ByBitsMatchIdTest : DbTest
    {
        [Test]
        public void ShouldFindBitsId()
        {
            // Arrange
            Match8x4 match = DbSeed.Create8x4Match();
            Session.Store(match);
            Session.SaveChanges();

            // Act
            bool found = Session.BitsIdExists(3003231);

            // Assert
            Assert.True(found);
        }
    }
}