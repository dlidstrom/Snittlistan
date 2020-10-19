namespace Snittlistan.Web.Areas.SupporterTravet.Controllers
{
    using System.Web.Mvc;
    using Web.Controllers;

    public class SupporterTravetViewController : AbstractController
    {
        public ActionResult Index()
        {
            SupporterTravetModels.Turn[] turns = new[]
            {
                new SupporterTravetModels.Turn("30 september", new[]
                    {
                        new SupporterTravetModels.Race("V75-1", new[]
                            {
                                new SupporterTravetModels.RaceResult(2, 242),
                                new SupporterTravetModels.RaceResult(7, 263),
                                new SupporterTravetModels.RaceResult(10, 248)
                            }
                        ),
                        new SupporterTravetModels.Race("V75-2", new[]
                            {
                                new SupporterTravetModels.RaceResult(2, 695)
                            }
                        )
                    }
                )
            };

            var viewModel = new ViewModel(
                new SupporterTravetModels.Header("sp-123"),
                turns);
            return View(viewModel);
        }

        public class ViewModel
        {
            public ViewModel(SupporterTravetModels.Header header, SupporterTravetModels.Turn[] turns)
            {
                Header = header;
                Turns = turns;
            }

            public SupporterTravetModels.Header Header { get; }
            public SupporterTravetModels.Turn[] Turns { get; }
        }
    }
}
