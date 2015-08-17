using Mercadia.Infrastructure.DTO.Stores;
using Mercadia.Web.Controllers;
using Mercadia.Web.Securities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Mercadia.Web.Areas.Manage.Controllers
{
    public class StoresController : BaseController
    {
        #region AddNew
        [HttpGet, AllowAnonymous]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost, AllowAnonymous]
        public ActionResult Add(StoreRequestDto request)
        {          
            /* Api CALL */
            var response = Post<string>("stores", request);

            /* Test RESULTS - OKAY */
            if (response.Status == HttpStatusCode.OK)
            {
                WebUser.UserStores = GetStoresByUser();
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
    }
}