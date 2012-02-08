namespace Snittlistan.Test
{
    using System;
    using System.Linq;
    using MvcContrib.TestHelper;
    using Snittlistan.Controllers;
    using Snittlistan.Models;
    using Snittlistan.ViewModels.Match;
    using Xunit;

    public class MatchController_Register4x4 : DbTest
    {
        [Fact]
        public void ViewIsCreate()
        {
            // Arrange
            var controller = new MatchController(Session);

            // Act
            var now = DateTimeOffset.Now;
            var result = controller.Register4x4(new Register4x4MatchViewModel
            {
                Location = "Somewhere",
                Date = now,
                HomeTeam = new Team4x4ViewModel
                {
                    Name = "HomeTeam",
                    Score = 13,
                    Serie1 = new Team4x4ViewModel.Serie
                    {
                        Game1 = new Team4x4ViewModel.Game
                        {
                            Player = "Lennart Axelsson",
                            Pins = 155,
                            Score = 1
                        }
                    }
                },
                AwayTeam = new Team4x4ViewModel
                {
                    Name = "AwayTeam",
                    Score = 6
                }
            });
            Session.SaveChanges();
            WaitForNonStaleResults<Match4x4>();

            // Assert
            var match = Session.Query<Match4x4>().Single();
            match.Location.ShouldBe("Somewhere");
            match.Date.ShouldBe(now);
            match.HomeTeam.Name.ShouldBe("HomeTeam");
            match.HomeTeam.Score.ShouldBe(13);
            var game = match.HomeTeam.Series.ElementAt(0).Games.ElementAt(0);
            game.Pins.ShouldBe(155);
            game.Player.ShouldBe("Lennart Axelsson");
            game.Score.ShouldBe(1);
            match.AwayTeam.Score.ShouldBe(6);
        }

        [Fact]
        public void WhenErrorReturnView()
        {
            // Arrange
            var controller = new MatchController(Session);
            controller.ModelState.AddModelError("key", "error");

            // Act
            var result = controller.Register4x4(new Register4x4MatchViewModel());

            // Assert
            result.AssertViewRendered().ForView(string.Empty);
        }
    }
}