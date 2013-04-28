using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Raven.Abstractions;
using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Areas.V2.Domain.Match;
using Snittlistan.Web.Areas.V2.ViewModels;
using Snittlistan.Web.Controllers;
using Snittlistan.Web.Helpers;
using Snittlistan.Web.Infrastructure.AutoMapper;

namespace Snittlistan.Web.Areas.V2.Controllers
{
    public class MatchResultAdminController : AdminController
    {
        public ActionResult RegisterBits(int? season)
        {
            if (season.HasValue == false)
                season = DocumentSession.LatestSeasonOrDefault(SystemTime.UtcNow.Year);

            ViewBag.rosterid = DocumentSession.CreateRosterSelectList(season.Value);
            return View(new RegisterBitsVerifyModel { Season = season.Value });
        }

        [Authorize]
        public ActionResult RegisterBitsVerify(RegisterBitsVerifyModel model)
        {
            if (ModelState.IsValid == false)
            {
                ViewBag.rosterid = DocumentSession.CreateRosterSelectList(model.Season);
                return View("RegisterBits", model);
            }

            var roster = DocumentSession.Include<Roster>(r => r.Players)
                .Load<Roster>(model.RosterId);
            if (roster == null)
                throw new HttpException(404, "Roster not found");

            var players = roster.Players.Select(x => DocumentSession.Load<Player>(x))
                .ToArray();
            var parser = new BitsParser(players);
            using (var client = new WebClient())
            {
                var address = string.Format(
                    "http://bits.swebowl.se/MatchFact.aspx?MatchId={0}",
                    model.BitsMatchId);
                var content = client.DownloadString(address);
                var result = parser.Parse(content, roster.Team);
                Debug.Assert(model.BitsMatchId != null, "model.BitsMatchId != null");
                var vm = new RegisterBitsResult
                {
                    BitsMatchId = model.BitsMatchId.Value,
                    TeamScore = result.TeamScore,
                    OpponentScore = result.OpponentScore,
                    RosterId = model.RosterId,
                    Serie1 = result.Series.ElementAtOrDefault(0),
                    Serie2 = result.Series.ElementAtOrDefault(1),
                    Serie3 = result.Series.ElementAtOrDefault(2),
                    Serie4 = result.Series.ElementAtOrDefault(3)
                };
                ViewBag.players = players.MapTo<PlayerViewModel>()
                    .ToArray();
                return View(vm);
            }
        }

        [HttpPost, ActionName("RegisterBitsVerify")]
        public ActionResult RegisterBitsVerifyConfirmed(RegisterBitsResult vm)
        {
            if (ModelState.IsValid == false) return View(vm);

            var roster = DocumentSession.Load<Roster>(vm.RosterId);
            if (roster == null)
                throw new HttpException(404, "Roster not found");

            var matchResult = new MatchResult(
                roster,
                vm.TeamScore,
                vm.OpponentScore,
                vm.BitsMatchId);
            foreach (var serie in new[] { vm.Serie1, vm.Serie2, vm.Serie3, vm.Serie4 }.Where(x => x != null))
            {
                var tables = new List<MatchTable>();
                for (var i = 0; i < 4; i++)
                {
                    var game1 = new MatchGame(
                        serie.Tables[i].Game1.Player,
                        serie.Tables[i].Game1.Pins,
                        serie.Tables[i].Game1.Strikes,
                        serie.Tables[i].Game1.Spares);
                    var game2 = new MatchGame(
                        serie.Tables[i].Game2.Player,
                        serie.Tables[i].Game2.Pins,
                        serie.Tables[i].Game2.Strikes,
                        serie.Tables[i].Game2.Spares);
                    tables.Add(new MatchTable(game1, game2, serie.Tables[i].Score));
                }

                matchResult.RegisterSerie(new MatchSerie(tables));
            }

            EventStoreSession.Store(matchResult);

            return RedirectToAction("Index", "MatchResult");
        }
    }
}