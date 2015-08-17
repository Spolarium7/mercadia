using Mercadia.Infrastructure.DTO.Stores;
using Mercadia.Infrastructure.DTO.Users;
using Mercadia.Web.Securities;
using Mercadia.Web.ViewModels;
using Mercadia.Web.ViewModels.Account;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;


namespace Mercadia.Web.Controllers
{
    public class AccountController : BaseController
    {

        #region Register
        [HttpGet, AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost, AllowAnonymous]
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


        #region EditProfile
        [HttpGet]
        public ActionResult EditProfile()
        {
            UserProfileRequestDto request = new UserProfileRequestDto();
            request.Id = WebUser.CurrentUser.Id.ToString();
            request.FirstName = WebUser.CurrentUser.FirstName;
            request.LastName = WebUser.CurrentUser.LastName;
            request.Phone = WebUser.CurrentUser.Phone;
            request.DeliveryAddress = WebUser.CurrentUser.DeliveryAddress;
            request.DeliveryState = WebUser.CurrentUser.DeliveryState;
            request.DeliveryCountry = WebUser.CurrentUser.DeliveryCountry;
            return View(request);
        }

        [HttpPost]
        public ActionResult EditProfile(UserProfileRequestDto request)
        {
            /* Api CALL */
            var response = Put<string>("users//editprofile", request);

            /* Test RESULTS - OKAY */
            if (response.Status == HttpStatusCode.OK)
            {
                SignIn(WebUser.CurrentUser.Id.ToString());
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

        #region EditProfile
        [HttpGet]
        public ActionResult ChangePassword()
        {
            ChangePasswordViewModel request = new ChangePasswordViewModel();
            request.Id = WebUser.CurrentUser.Id.ToString();
            return View(request);
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel request)
        {
            /* Api CALL */

            if (request.Password != request.ConfirmPassword)
            {
                //this.ModelState.AddModelError("", "Password confirmation does not match");
                return View(request);
            }

            var response = Put<string>("users//changepassword", request);

            /* Test RESULTS - OKAY */
            if (response.Status == HttpStatusCode.OK)
            {
                SignIn(WebUser.CurrentUser.Id.ToString());
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

        #region VerifyEmail
        [HttpGet, AllowAnonymous]
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

        [HttpPost, AllowAnonymous]
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
        [HttpGet, AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost, AllowAnonymous]
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

                //var identity = UserManager.CreateIdentity();


                //AuthenticationManager.SignIn(
                //   new AuthenticationProperties()
                //   {
                //       IsPersistent = true
                //   }, identity);

            }
            /* Test RESULTS - Api Validation Error */
            else if (response.Status == HttpStatusCode.BadRequest)
            {
                this.ModelState.AddModelError("", response.Message);
            }
        }
        #endregion

        #region Logoff
        public ActionResult Logoff()
        {
            //AuthenticationManager.SignOut();

            Session.Abandon();
            Session.RemoveAll();
            Session.Clear();

            /* clear session cookie */
            HttpCookie cookie = new HttpCookie("ASP.NET_SessionId", "");
            cookie.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie);

            HttpRuntime.Close();

            return RedirectToAction("index", "home");
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