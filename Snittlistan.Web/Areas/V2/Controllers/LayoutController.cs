﻿namespace Snittlistan.Web.Areas.V2.Controllers
{
    using System.Web.Mvc;
    using Snittlistan.Web.Controllers;
    using Snittlistan.Web.Models;

    public class LayoutController : AbstractController
    {
        [ChildActionOnly]
        public ActionResult NavPart()
        {
            WebsiteConfig websiteConfig = DocumentSession.Load<WebsiteConfig>(WebsiteConfig.GlobalId);
            return View(websiteConfig);
        }
    }
}