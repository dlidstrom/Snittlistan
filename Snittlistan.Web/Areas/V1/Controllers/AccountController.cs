using System;
using System.Web.Mvc;
using Snittlistan.Web.Controllers;

namespace Snittlistan.Web.Areas.V1.Controllers
{
    public class AccountController : AbstractController
    {
        public ActionResult LogOn()
        {
            return RedirectToActionPermanent("Index", "Roster");
        }

        /// <summary>
        /// GET: /Account/LogOff.
        /// </summary>
        /// <returns></returns>
        public ActionResult LogOff()
        {
            return RedirectToActionPermanent("Index", "Roster");
        }

        /// <summary>
        /// GET: /Account/Register.
        /// </summary>
        /// <returns></returns>
        public ActionResult Register()
        {
            return RedirectToActionPermanent("Index", "Roster");
        }

        public ActionResult ChangePasswordSuccess()
        {
            throw new Exception("deprecated");
        }

        public ActionResult RegisterSuccess()
        {
            throw new Exception("deprecated");
        }

        public ActionResult Verify()
        {
            throw new Exception("deprecated");
        }

        public ActionResult VerifySuccess()
        {
            throw new Exception("deprecated");
        }
    }
}