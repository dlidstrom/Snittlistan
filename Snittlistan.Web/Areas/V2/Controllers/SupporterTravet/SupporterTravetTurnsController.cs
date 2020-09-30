namespace Snittlistan.Web.Areas.V2.Controllers.SupporterTravet
{
    using System.Web.Mvc;
    using Web.Controllers;

    public class SupporterTravetTurnsController : AbstractController
    {
        public ActionResult Index()
        {
            var viewModel = new ViewModel(
                new SupporterTravetModels.Header("sp-123"));
            return View(viewModel);
        }

        public class ViewModel
        {
            public ViewModel(SupporterTravetModels.Header header)
            {
                Header = header;
            }

            public SupporterTravetModels.Header Header { get; }
        }
    }
}
