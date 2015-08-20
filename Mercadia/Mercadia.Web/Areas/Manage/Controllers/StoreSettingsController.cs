using Mercadia.Infrastructure.DTO.Stores;
using Mercadia.Web.Controllers;
using Mercadia.Web.Securities;
using Mercadia.Web.ViewModels.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Mercadia.Web.Areas.Manage.Controllers
{
    [InPrivate]
    [RoutePrefix("manage/storesettings")]
    public class StoreSettingsController : BaseController
    {
        #region Get
        [HttpGet, Route("paypal/{id}")]
        public JsonResult Paypal(string id)
        {

            var response = Get<PaypalAccountSettingsDto>("storesettings//paypal//" + id);

            /* Test RESULTS - OKAY */
            if (response.Status == HttpStatusCode.OK)
            {
                PaypalAccountSettingsViewModel paypalSettings = new PaypalAccountSettingsViewModel();
                paypalSettings.Id = id;
                paypalSettings.PaypalFacilitator = response.Data.PaypalFacilitator;
                paypalSettings.PaypalPassword = response.Data.PaypalPassword;
                paypalSettings.PaypalSignature = response.Data.PaypalSignature;
                return Json(paypalSettings,JsonRequestBehavior.AllowGet);
            }
            /* Test RESULTS - Api Validation Error */
            else if (response.Status == HttpStatusCode.BadRequest)
            {
                this.ModelState.AddModelError("", response.Message);
                return null;
            }

            return null;
        }
        #endregion


        #region Upsert
        [HttpPost, Route("upsertpaypal/{id}")]
        public JsonResult UpsertPaypal(PaypalAccountSettingsViewModel model, string id)
        {
            /* Api CALL */
            var response = Post<string>("storesettings//paypal//" + id, model);

            /* Test RESULTS - OKAY */
            if (response.Status == HttpStatusCode.OK)
            {
                Json(response.Data, JsonRequestBehavior.AllowGet);
            }
            /* Test RESULTS - Api Validation Error */
            else if (response.Status == HttpStatusCode.BadRequest)
            {
                this.ModelState.AddModelError("", response.Message);
                return null;
            }

            /* If we got this far, something failed, redisplay form */
            return null;
        }
        #endregion
    }
}