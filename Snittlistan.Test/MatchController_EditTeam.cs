using System;
using System.Web.Mvc;
using MvcContrib.TestHelper;
using Snittlistan.Controllers;
using Snittlistan.Infrastructure.AutoMapper;
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
				Team = DbSeed.CreateMatch().AwayTeam.MapTo<TeamViewModel>()
			});
			Session.SaveChanges();

			// Assert
			result.AssertActionRedirect().ToAction("Details").WithParameter("id", originalMatch.Id);
			var match = Session.Load<Match>(originalMatch.Id);
			TestData.VerifyMatch(match);
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

		[Fact]
		public void CorrectView()
		{
			// Arrange
			var match = DbSeed.CreateMatch();
			Session.Store(match);

			// Act
			var controller = new MatchController(Session);
			var result = controller.EditTeam(id: match.Id, isHomeTeam: true);

			// Assert
			result.AssertViewRendered().ForView(string.Empty);
		}
	}
}