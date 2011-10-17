using System;
using System.Collections.Generic;
using System.Linq;
using MvcContrib.TestHelper;
using Snittlistan.Controllers;
using Snittlistan.Models;
using Snittlistan.ViewModels;
using Xunit;

namespace Snittlistan.Test
{
	public class MatchController_Index : DbTest
	{
		[Fact]
		public void ShouldViewIndex()
		{
			// Arrange
			var controller = new MatchController(Session);

			// Act
			var result = controller.Index();

			// Assert
			result.AssertViewRendered().ForView(string.Empty);
		}

		[Fact]
		public void ShouldListMatches()
		{
			// Arrange
			var now = DateTime.Now;
			Session.Store(new Match("P1", now, 1, new Team("Home", 1), new Team("Away", 2)));
			Session.Store(new Match("P2", now.AddDays(1), 2, new Team("Home2", 3), new Team("Away2", 4)));
			Session.SaveChanges();
			WaitForNonStaleResults<Match>();

			// Act
			var controller = new MatchController(Session);
			var result = controller.Index().Model as IEnumerable<MatchViewModel>;

			// Assert
			var matches = result.ToArray();
			matches.Length.ShouldBe(2);
			matches[0].Match.Location.ShouldBe("P2");
			matches[1].Match.Location.ShouldBe("P1");
		}
	}
}