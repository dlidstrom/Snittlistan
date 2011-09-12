using System;
using MvcContrib.TestHelper;
using Snittlistan.Controllers;
using Snittlistan.Models;
using Snittlistan.ViewModels;
using Xunit;

namespace Snittlistan.Test
{
	public class MatchController_Details : DbTest
	{
		[Fact]
		public void ShouldViewMatch()
		{
			// Arrange
			Session.Store(new Match("P1", DateTime.Now, 1, new Team("Home1", 4), new Team("Away1", 7)) { Id = 1 });
			Session.Store(new Match("P2", DateTime.Now, 2, new Team("Home2", 5), new Team("Away2", 8)) { Id = 2 });
			Session.Store(new Match("P3", DateTime.Now, 3, new Team("Home3", 6), new Team("Away3", 9)) { Id = 3 });
			WaitForNonStaleResults<Match>();
			var controller = new MatchController(Session);

			// Act
			var model1 = controller.Details(1).Model as MatchViewModel;
			var model2 = controller.Details(2).Model as MatchViewModel;
			var model3 = controller.Details(3).Model as MatchViewModel;

			// Assert
			model1.Match.Id.ShouldBe(1);
			model2.Match.Id.ShouldBe(2);
			model3.Match.Id.ShouldBe(3);
		}
	}
}
