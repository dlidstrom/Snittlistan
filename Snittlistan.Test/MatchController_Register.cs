using System;
using System.Linq;
using MvcContrib.TestHelper;
using Snittlistan.Controllers;
using Snittlistan.Models;
using Snittlistan.ViewModels;
using Xunit;

namespace Snittlistan.Test
{
	public class MatchController_Register : DbTest
	{
		[Fact]
		public void ViewIsCreate()
		{
			// Arrange
			var controller = new MatchController(Session);

			// Act
			var now = DateTime.Now;
			var matchDetails = new MatchViewModel.MatchDetails
			{
				BitsMatchId = 1,
				Date = now,
				Location = "Somewhere"
			};
			var homeTeam = new TeamViewModel { Name = "HomeTeam", Score = 13 };
			var awayTeam = new TeamViewModel { Name = "AwayTeam", Score = 6 };
			var result = controller.Register(new RegisterMatchViewModel
			{
				Location = "Somewhere",
				Date = now,
				BitsMatchId = 1,
				HomeTeam = new TeamViewModel
				{
					Name = "HomeTeam",
					Score = 13
				},
				AwayTeam = new TeamViewModel
				{
					Name = "AwayTeam",
					Score = 6
				}
			});
			Session.SaveChanges();
			WaitForNonStaleResults<Match>();

			// Assert
			var match = Session.Query<Match>().Single();
			match.Location.ShouldBe("Somewhere");
			match.BitsMatchId.ShouldBe(1);
			match.Date.ShouldBe(now);
			match.HomeTeam.Name.ShouldBe("HomeTeam");
			match.HomeTeam.Score.ShouldBe(13);
			match.AwayTeam.Score.ShouldBe(6);
		}
	}
}
