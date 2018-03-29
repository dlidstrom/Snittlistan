namespace Snittlistan.Web.Areas.V2.Controllers
{
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Domain;
    using Helpers;
    using Raven.Abstractions;
    using Rotativa;
    using Rotativa.Options;
    using Web.Controllers;

    [Authorize]
    public class EliteMedalsPrintController : AbstractController
    {
        [HttpPost]
        public ActionResult GeneratePdf(PostModel postModel)
        {
            if (ModelState.IsValid == false)
            {
                return RedirectToAction("EliteMedals", "MatchResult");
            }

            // find out current season
            var season = DocumentSession.LatestSeasonOrDefault(SystemTime.UtcNow.Year);
            var eliteMedals = DocumentSession.Load<EliteMedals>(EliteMedals.TheId);

            var filename = $"Elitmedaljer_{season}-{season+1}.pdf";
            var viewModel = new EliteMedalsPrintViewModel();
            //return View(viewModel);
            return new ViewAsPdf(viewModel)
            {
                PageSize = Size.A4
                //, FileName = filename
            };
        }

        public class PostModel
        {
            [Required]
            public string Name { get; set; }
        }

        public class EliteMedalsPrintViewModel
        {
        }
    }
}