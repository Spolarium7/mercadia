using Mercadia.Infrastructure.DTO.Stores;
using Mercadia.Infrastructure.DTO.Users;
using Mercadia.Web.Securities;
using Mercadia.Web.ViewModels;
using Mercadia.Web.ViewModels.Account;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Mercadia.Web.Controllers
{
    public class AccountController : BaseController
    {

        #region Register
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
        #endregion

        #region VerifyEmail
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
                SignIn(response.Data);
                return RedirectToAction("dashboard", "account");
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
        #endregion

        #region Login
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserLoginRequestDto request)
        {
            /* Api CALL */
            var response = Post<UserResponseDto>("users//login", request);

            /* Test RESULTS - OKAY */
            if (response.Status == HttpStatusCode.OK)
            {
                SignIn(response.Data.Id.ToString());
                return RedirectToAction("dashboard", "account");
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

        #endregion

        #region SignIn
        public void SignIn(string Id)
        {
            var response = Get<UserResponseDto>("users//" + Id);

            /* Test RESULTS - OKAY */
            if (response.Status == HttpStatusCode.OK)
            {
                WebUser.CurrentUser = response.Data;
            }
            /* Test RESULTS - Api Validation Error */
            else if (response.Status == HttpStatusCode.BadRequest)
            {
                this.ModelState.AddModelError("", response.Message);
            }
        }
        #endregion

        public ActionResult Dashboard()
        {
            StoresViewModel model = new StoresViewModel();
            var response = Get<List<StoreResponseDto>>("stores//byowner//" + WebUser.CurrentUser.Id);

            /* Test RESULTS - OKAY */
            if (response.Status == HttpStatusCode.OK)
            {
                model.Stores = response.Data;

                return View(model);
            }
            /* Test RESULTS - Api Validation Error */
            else if (response.Status == HttpStatusCode.BadRequest)
            {
                this.ModelState.AddModelError("", response.Message);
                return View();
            }
            return View();
        }
    }
}