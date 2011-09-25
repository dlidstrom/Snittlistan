using System;
using System.Collections.Generic;
using System.Web.Mvc;
using MvcContrib.TestHelper;
using Snittlistan.Controllers;
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
			WaitForNonStaleResults<Match>();

			// Act
			var controller = new MatchController(Session);
			var result = controller.EditTeam(new EditTeamViewModel
			{
				Id = originalMatch.Id,
				IsHomeTeam = true,
				Team = new TeamViewModel
				{
					Name = "AnotherTeam",
					Series = new List<TeamViewModel.Serie>
					{
						new TeamViewModel.Serie
						{
							Tables = new List<TeamViewModel.Table>
							{
								new TeamViewModel.Table
								{
									Score = 1,
									FirstGame = new TeamViewModel.Game { Player = "P1", Pins = 200 },
									SecondGame = new TeamViewModel.Game { Player = "P2", Pins = 230 }
								}
							}
						}
					}
				}
			});

			// Assert
			result.AssertActionRedirect().ToAction("Details").WithParameter("id", originalMatch.Id);
			var match = Session.Load<Match>(originalMatch.Id);
			match.HomeTeam.Name.ShouldBe("AnotherTeam");
			match.HomeTeam.Series.Count.ShouldBe(1);
			match.HomeTeam.Series[0].Pins.ShouldBe(430);
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
