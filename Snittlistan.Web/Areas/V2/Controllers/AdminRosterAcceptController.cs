#nullable enable

using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Areas.V2.ViewModels;
using Snittlistan.Web.Controllers;

namespace Snittlistan.Web.Areas.V2.Controllers;

[Authorize(Roles = WebsiteRoles.Uk.UkTasks)]
public class AdminRosterAcceptController : AbstractController
{
    public ActionResult AcceptPlayer(string rosterId, int season, int turn)
    {
        Roster roster = CompositionRoot.DocumentSession.Load<Roster>(rosterId);
        Player[] players = CompositionRoot.DocumentSession.Load<Player>(roster.Players.Where(x => roster.AcceptedPlayers.Contains(x) == false));
        ViewBag.PlayerId = CompositionRoot.DocumentSession.CreatePlayerSelectList(
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
                    roster.MatchResultId,
                    roster.MatchTimeChanged),
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
        Roster roster = CompositionRoot.DocumentSession.Load<Roster>(vm.Model?.RosterId);
        Roster.Update update = new(
            Roster.ChangeType.PlayerAccepted,
            User.Identity.Name)
        {
            PlayerAccepted = vm.Model?.PlayerId!
        };
        roster.UpdateWith(CompositionRoot.CorrelationId, update);
        return RedirectToAction("View", "Roster", new
        {
            vm.Model?.Season,
            vm.Model?.Turn
        });
    }

    public class ViewModel
    {
        public RosterHeaderViewModel? Header { get; set; }

        public PostModel? Model { get; set; }

        public class PostModel
        {
            [Required]
            public string? RosterId { get; set; }

            [Required]
            public int Season { get; set; }

            [Required]
            public int Turn { get; set; }

            [Required]
            [Display(Name = "Spelare")]
            public string? PlayerId { get; set; }
        }
    }
}
