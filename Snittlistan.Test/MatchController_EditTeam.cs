using System;
using System.Web.Mvc;
using MvcContrib.TestHelper;
using Snittlistan.Controllers;
using Snittlistan.Infrastructure;
using Snittlistan.Models;
using Snittlistan.ViewModels;
using Xunit;

namespace Snittlistan.Test
{
	public class MatchController_EditTeam : DbTest
	{
		[Fact]
		public void CanEditTeam()
		{
			// Arrange
			Match originalMatch = new Match("Place", DateTime.Now, 1, new Team("Home", 13), new Team("Away", 6));
			Session.Store(originalMatch);
			Session.SaveChanges();

			// Act
			var controller = new MatchController(Session);
			var result = controller.EditTeam(new EditTeamViewModel
			{
				Id = originalMatch.Id,
				IsHomeTeam = false,
				Team = TestData.CreateMatch().AwayTeam.MapTo<TeamViewModel>()
			});

			// Assert
			result.AssertActionRedirect().ToAction("Details").WithParameter("id", originalMatch.Id);
			var match = Session.Load<Match>(originalMatch.Id);
			match.AwayTeam.Name.ShouldBe("Fredrikshof IF");
			match.AwayTeam.PinsForPlayer("Peter Sjöberg").ShouldBe(787);
			match.AwayTeam.Series.Count.ShouldBe(4);
			match.AwayTeam.Pins().ShouldBe(6216);
			match.AwayTeam.PinsFor(1).ShouldBe(1598);
			match.AwayTeam.PinsFor(2).ShouldBe(1573);
			match.AwayTeam.PinsFor(3).ShouldBe(1505);
			match.AwayTeam.PinsFor(4).ShouldBe(1540);
			match.AwayTeam.ScoreFor(1).ShouldBe(2);
			match.AwayTeam.ScoreFor(2).ShouldBe(2);
			match.AwayTeam.ScoreFor(3).ShouldBe(1);
			match.AwayTeam.ScoreFor(4).ShouldBe(1);
			match.AwayTeam.Score.ShouldBe(6);
		}

		[Fact]
		public void CannotEditNonExistingMatch()
		{
			var controller = new MatchController(Session);
			var result = controller.EditTeam(1, true);

			// Assert
			result.AssertResultIs<HttpNotFoundResult>();
		}

		[Fact]
		public void CannotPostNonExistingMatch()
		{
			var controller = new MatchController(Session);
			var result = controller.EditTeam(new EditTeamViewModel { Id = 1 });

			// Assert
			result.AssertResultIs<HttpNotFoundResult>();
		}
	}
}
