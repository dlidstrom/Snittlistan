#nullable enable

namespace Snittlistan.Test.Domain
{
    using System.Collections.Generic;
    using NUnit.Framework;
    using Snittlistan.Web.Areas.V2.Domain.Match;

    [TestFixture]
    public class MatchSerieTest
    {
        [Test]
        public void ValidMatchSerie()
        {
            // Arrange
            MatchTable table1 = new(1, new MatchGame("p1", 0, 0, 0), new MatchGame("p2", 0, 0, 0), 0);
            MatchTable table2 = new(2, new MatchGame("p3", 0, 0, 0), new MatchGame("p4", 0, 0, 0), 0);
            MatchTable table3 = new(3, new MatchGame("p5", 0, 0, 0), new MatchGame("p6", 0, 0, 0), 0);
            MatchTable table4 = new(4, new MatchGame("p7", 0, 0, 0), new MatchGame("p8", 0, 0, 0), 0);

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
