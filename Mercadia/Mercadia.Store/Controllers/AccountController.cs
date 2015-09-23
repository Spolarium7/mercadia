using Mercadia.Infrastructure.DTO.Stores;
using Mercadia.Infrastructure.DTO.Users;
using Mercadia.Store.Securities;
using Mercadia.Store.ViewModels;
using Mercadia.Store.ViewModels.Account;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Recaptcha.Web;
using Recaptcha.Web.Mvc;

namespace Mercadia.Store.Controllers
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

        #region ForgotPassword
        [HttpGet, AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost, AllowAnonymous]
        public ActionResult ForgotPassword(ForgotPasswordViewModel request)
        {
            /* Api CALL */
            #region Recaptcha
            RecaptchaVerificationHelper recaptchaHelper = this.GetRecaptchaVerificationHelper();

            if (String.IsNullOrEmpty(recaptchaHelper.Response))
            {
                this.ModelState.AddModelError("", "Captcha answer cannot be empty.");
                return View(request);
            }
            else
            {
                RecaptchaVerificationResult recaptchaResult = recaptchaHelper.VerifyRecaptchaResponse();

                if (recaptchaResult != RecaptchaVerificationResult.Success)
                {
                    this.ModelState.AddModelError("", "Incorrect captcha answer.");
                    return View(request);
                }
            }
            #endregion

            var response = Put<string>("users//forgotpassword", request);

            /* Test RESULTS - OKAY */
            if (response.Status == HttpStatusCode.OK)
            {                
                return RedirectToAction("login", "account");
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

        #region ChangePassword
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

            var response = Put<string>("users//changepassword", new ChangePasswordRequestDto { Id = request.Id, NewPassword = request.Password });

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
                return View();
            }

            /* If we got this far, something failed, redisplay form */
            return View();
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
                WebUser.UserStores = GetStoresByUser();
            }
            /* Test RESULTS - Api Validation Error */
            else if (response.Status == HttpStatusCode.BadRequest)
            {
                this.ModelState.AddModelError("", response.Message);
            }
        }

        public List<StoreResponseDto> GetStoresByUser(){
            var response = Get<List<StoreResponseDto>>("stores//byowner//" + WebUser.CurrentUser.Id);

            /* Test RESULTS - OKAY */
            if (response.Status == HttpStatusCode.OK)
            {
                return response.Data;                
            }
            /* Test RESULTS - Api Validation Error */
            else if (response.Status == HttpStatusCode.BadRequest)
            {
                return null;
            }

            return null;
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
            DashboardViewModel model = new DashboardViewModel();
            var response = Get<List<StoreResponseDto>>("stores//list");

            /* Test RESULTS - OKAY */
            if (response.Status == HttpStatusCode.OK)
            {
                model.MyStores = WebUser.UserStores;
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