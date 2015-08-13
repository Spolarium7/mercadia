using Mercadia.Infrastructure.DTO.Users;
using Mercadia.Web.ViewModel.Account;
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

        public ActionResult VerifyEmail(string email)
        {
            var response = Get<UserResponseDto>("users//email//" + email + "//");

            /* Test RESULTS - OKAY */
            if (response.Status == HttpStatusCode.OK)
            {
                return View(response.Data);
            }
            /* Test RESULTS - Api Validation Error */
            else if (response.Status == HttpStatusCode.BadRequest)
            {
                this.ModelState.AddModelError("", response.Message);
                return View(response.Data);
            }

            return View();
        }

        [HttpPost]
        public ActionResult VerifyEmail(VerifyViewModel request)
        {
            /* Api CALL */
            var response = Post<string>("users//verify", request);

            /* Test RESULTS - OKAY */
            if (response.Status == HttpStatusCode.OK)
            {
                return RedirectToAction("dashboard", "user", new { id = request.Id });
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