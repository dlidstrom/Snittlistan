using NUnit.Framework;
using Snittlistan.Web.Helpers;

namespace Snittlistan.Test
{
    [TestFixture]
    public class Match_ByBitsMatchIdTest : DbTest
    {
        [Test]
        public void ShouldFindBitsId()
        {
            // Arrange
            var match = DbSeed.Create8x4Match();
            Session.Store(match);
            Session.SaveChanges();

            // Act
            bool found = Session.BitsIdExists(3003231);

            // Assert
            Assert.True(found);
        }
    }
}