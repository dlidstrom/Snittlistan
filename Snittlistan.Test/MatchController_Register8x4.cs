namespace Snittlistan.Test
{
    using System;
    using System.Linq;
    using MvcContrib.TestHelper;
    using Snittlistan.Controllers;
    using Snittlistan.Models;
    using Snittlistan.ViewModels.Match;
    using Xunit;

    public class MatchController_Register8x4 : DbTest
    {
        [Fact]
        public void ViewIsCreate()
        {
            // Arrange
            var controller = new MatchController(Session);

            // Act
            var now = DateTimeOffset.Now;
            var result = controller.Register8x4(new Register8x4MatchViewModel
            {
                Location = "Somewhere",
                Date = now,
                BitsMatchId = 1,
                HomeTeam = new Team8x4ViewModel
                {
                    Name = "HomeTeam",
                    Score = 13
                },
                AwayTeam = new Team8x4ViewModel
                {
                    Name = "AwayTeam",
                    Score = 6
                }
            });
            Session.SaveChanges();
            WaitForNonStaleResults<Match8x4>();

            // Assert
            var match = Session.Query<Match8x4>().Single();
            match.Location.ShouldBe("Somewhere");
            match.BitsMatchId.ShouldBe(1);
            match.Date.ShouldBe(now);
            match.HomeTeam.Name.ShouldBe("HomeTeam");
            match.HomeTeam.Score.ShouldBe(13);
            match.AwayTeam.Score.ShouldBe(6);
        }

        [Fact]
        public void WhenErrorReturnView()
        {
            // Arrange
            var controller = new MatchController(Session);
            controller.ModelState.AddModelError("key", "error");

            // Act
            var result = controller.Register8x4(new Register8x4MatchViewModel());

            // Assert
            result.AssertViewRendered().ForView(string.Empty);
        }

        [Fact]
        public void CannotRegisterSameBitsIdTwice()
        {
            // Arrange
            var controller = new MatchController(Session);
            var now = DateTime.Now;
            Register8x4MatchViewModel vm = new Register8x4MatchViewModel
            {
                Location = "Somewhere",
                Date = now,
                BitsMatchId = 1,
                HomeTeam = new Team8x4ViewModel
                {
                    Name = "HomeTeam",
                    Score = 13
                },
                AwayTeam = new Team8x4ViewModel
                {
                    Name = "AwayTeam",
                    Score = 6
                }
            };
            controller.Register8x4(vm);
            Session.SaveChanges();

            // Act
            var result = controller.Register8x4(vm);

            // Assert
            result.AssertViewRendered().ForView(string.Empty);
        }
    }
}