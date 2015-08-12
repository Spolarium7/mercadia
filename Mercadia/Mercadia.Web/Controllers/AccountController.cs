using Mercadia.Infrastructure.DTO.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Mercadia.Web.Controllers
{
    public class AccountController : BaseController
    {
        // GET: Account
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(UserRequestDto request)
        {
            if (request.Password != request.ConfirmPassword)
            {
                this.ModelState.AddModelError("", "Password confirmation does not match");
                return View(request);
            }


            /* Api CALL */
            var response = Post<string>("users", request);

            /* Test RESULTS - OKAY */
            if (response.Status == HttpStatusCode.OK)
            {
                return RedirectToAction("verifyemail", "account", new { email = request.Email.ToLower() });
            }
            /* Test RESULTS - Api Validation Error */
            else if (response.Status == HttpStatusCode.BadRequest)
            {
                this.ModelState.AddModelError("", response.Message);
                return View(request);
            }

            /* If we got this far, something failed, redisplay form */
            return View(request);

        }
    }
}