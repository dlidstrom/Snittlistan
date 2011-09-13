using System;
using System.Linq;
using MvcContrib.TestHelper;
using Snittlistan.Controllers;
using Snittlistan.Models;
using Snittlistan.ViewModels;
using Xunit;

namespace Snittlistan.Test
{
	public class MatchController_Create : DbTest
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
				Place = "Somewhere"
			};
			var homeTeam = new MatchViewModel.Team { Name = "HomeTeam", Score = 13 };
			var awayTeam = new MatchViewModel.Team { Name = "AwayTeam", Score = 6 };
			var result = controller.Create(new MatchViewModel
			{
				Match = matchDetails,
				HomeTeam = homeTeam,
				AwayTeam = awayTeam
			});
			Session.SaveChanges();
			WaitForNonStaleResults<Match>();

			// Assert
			var match = Session.Query<Match>().Single();
			match.Place.ShouldBe("Somewhere");
			match.BitsMatchId.ShouldBe(1);
			match.Date.ShouldBe(now);
			match.HomeTeam.Name.ShouldBe("HomeTeam");
			match.HomeTeam.Score.ShouldBe(13);
			match.AwayTeam.Score.ShouldBe(6);
		}
	}
}
