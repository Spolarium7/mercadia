using Mercadia.Api.Mailers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mercadia.Api.Helpers;

namespace Mercadia.Api.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            /* Send Email */
            new UserMailer()
                .Welcome(name: string.Format("{0} {1}", "Ponce", "Omar"), email: "ponceomar.20@gmail.com", verificationCode: "xxxxx")
                .SendNow();

            return View();
        }
    }
}
