using Mercadia.Infrastructure.DTO.Stores;
using Mercadia.Infrastructure.DTO.Users;
using Mercadia.Store.Securities;
using Mercadia.Store.ViewModels.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Mercadia.Store.Controllers
{
    public class StoreUserController : BaseController
    {
        #region Register
        [HttpGet, AllowAnonymous]
        public ActionResult Register()
        {
            return View(string.Format("../{0}/register", WebUser.CurrentStore.Template));
        }

        [HttpPost, AllowAnonymous]
        public ActionResult Register(UserRequestDto request)
        {
            if (request.Password != request.ConfirmPassword)
            {
                this.ModelState.AddModelError("", "Password confirmation does not match");
                return View(string.Format("../{0}/register", WebUser.CurrentStore.Template), request);
            }

            /* Api CALL */
            var response = Post<string>("users", request);

            /* Test RESULTS - OKAY */
            if (response.Status == HttpStatusCode.OK)
            {
                return RedirectToAction("verifyemail", "storeuser" , new { email = request.Email.ToLower() });
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
                return View(string.Format("../{0}/verifyemail", WebUser.CurrentStore.Template), response.Data);
            }
            /* Test RESULTS - Api Validation Error */
            else if (response.Status == HttpStatusCode.BadRequest)
            {
                this.ModelState.AddModelError("", response.Message);
                return View(string.Format("../{0}/verifyemail", WebUser.CurrentStore.Template), response.Data);
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
                return RedirectToAction(WebUser.RedirectAction, WebUser.RedirectController);
            }
            /* Test RESULTS - Api Validation Error */
            else if (response.Status == HttpStatusCode.BadRequest)
            {
                this.ModelState.AddModelError("", response.Message);
                return View(string.Format("../{0}/verifyemail", WebUser.CurrentStore.Template), request);
            }

            /* If we got this far, something failed, redisplay form */
            return View(string.Format("../{0}/verifyemail", WebUser.CurrentStore.Template), request);
        }
        #endregion

        #region Login
        [HttpGet, AllowAnonymous]
        public ActionResult Login()
        {
            return View(string.Format("../{0}/login", WebUser.CurrentStore.Template));
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
                return RedirectToAction(WebUser.RedirectAction, WebUser.RedirectController);
            }
            /* Test RESULTS - Api Validation Error */
            else if (response.Status == HttpStatusCode.BadRequest)
            {
                this.ModelState.AddModelError("", response.Message);
                return View(string.Format("../{0}/login", WebUser.CurrentStore.Template));
            }

            /* If we got this far, something failed, redisplay form */
            return View(string.Format("../{0}/login", WebUser.CurrentStore.Template));
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

        public List<StoreResponseDto> GetStoresByUser()
        {
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
    }
}