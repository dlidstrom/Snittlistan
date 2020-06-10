namespace Snittlistan.Web.Areas.V2.Controllers
{
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;
    using Domain;
    using ViewModels;
    using Web.Controllers;

    [Authorize(Roles = WebsiteRoles.Uk.UkTasks)]
    public class AdminRosterAcceptController : AbstractController
    {
        public ActionResult AcceptPlayer(string rosterId, int season, int turn)
        {
            Roster roster = DocumentSession.Load<Roster>(rosterId);
            Player[] players = DocumentSession.Load<Player>(roster.Players.Where(x => roster.AcceptedPlayers.Contains(x) == false));
            ViewBag.PlayerId = DocumentSession.CreatePlayerSelectList(
                getPlayers: () => players);
            return View(
                new ViewModel
                {
                    Header = new RosterHeaderViewModel(
                        roster.Id,
                        roster.Team,
                        roster.TeamLevel,
                        roster.Location,
                        roster.Opponent,
                        roster.Date,
                        roster.OilPattern,
                        roster.MatchResultId),
                    Model = new ViewModel.PostModel
                    {
                        RosterId = rosterId,
                        Season = season,
                        Turn = turn
                    }
                });
        }

        [HttpPost]
        public ActionResult AcceptPlayer(ViewModel vm)
        {
            Roster roster = DocumentSession.Load<Roster>(vm.Model.RosterId);
            roster.Accept(vm.Model.PlayerId);
            return RedirectToAction("View", "Roster", new
            {
                vm.Model.Season,
                vm.Model.Turn
            });
        }

        public class ViewModel
        {
            public RosterHeaderViewModel Header { get; set; }

            public PostModel Model { get; set; }

            public class PostModel
            {
                [Required]
                public string RosterId { get; set; }

                [Required]
                public int Season { get; set; }

                [Required]
                public int Turn { get; set; }

                [Required]
                [Display(Name = "Spelare")]
                public string PlayerId { get; set; }
            }
        }
    }
}