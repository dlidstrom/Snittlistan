using System.Collections.Generic;
using NUnit.Framework;
using Snittlistan.Web.Areas.V2.Domain.Match;

namespace Snittlistan.Test.Domain
{
    [TestFixture]
    public class MatchSerieTest
    {
        [Test]
        public void ValidMatchSerie()
        {
            // Arrange
            var table1 = new MatchTable(1, new MatchGame("p1", 0, 0, 0), new MatchGame("p2", 0, 0, 0), 0);
            var table2 = new MatchTable(2, new MatchGame("p3", 0, 0, 0), new MatchGame("p4", 0, 0, 0), 0);
            var table3 = new MatchTable(3, new MatchGame("p5", 0, 0, 0), new MatchGame("p6", 0, 0, 0), 0);
            var table4 = new MatchTable(4, new MatchGame("p7", 0, 0, 0), new MatchGame("p8", 0, 0, 0), 0);

            // Act & Assert
            Assert.DoesNotThrow(
                () => new MatchSerie(
                    1,
                    new List<MatchTable>
                    {
                        table1,
                        table2,
                        table3,
                        table4
                    }));
        }
    }
}
