using Mercadia.Infrastructure.DTO.Stores;
using Mercadia.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Mercadia.Api.Controllers
{
    [RoutePrefix("api/storesettings")]
    public class StoreSettingsController : BaseApiController
    {

        #region Paypal
        [HttpGet, Route("paypal/{id}")]
        public PaypalAccountSettingsDto Get(string id)
        {
            Guid idCompare = Guid.Parse(id);

            var storeSettings = db.StoreSettings
                .Where(a => a.StoreId == idCompare && a.GroupName == "Paypal");

            PaypalAccountSettingsDto settings = new PaypalAccountSettingsDto();

            if(storeSettings.Count() > 0)
            {
                settings.PaypalFacilitator = storeSettings.Where(a => a.Key == "PaypalFacilitator").FirstOrDefault().Value;
                settings.PaypalPassword = storeSettings.Where(a => a.Key == "PaypalPassword").FirstOrDefault().Value;
                settings.PaypalSignature = storeSettings.Where(a => a.Key == "PaypalSignature").FirstOrDefault().Value;
            }

            return settings;
        }


        [HttpPost, Route("paypal/{id}")]
        public string Post(PaypalAccountSettingsDto request)
        {
            Guid idCreate = Guid.Parse(request.Id);

            StoreSetting facilitatorSetting = new StoreSetting();
            facilitatorSetting.StoreId = idCreate;
            facilitatorSetting.GroupName = "Paypal";
            facilitatorSetting.Key = "PaypalFacilitator";
            facilitatorSetting.Value = request.PaypalFacilitator;
            Upsert(facilitatorSetting);

            StoreSetting passwordSetting = new StoreSetting();
            passwordSetting.StoreId = idCreate;
            passwordSetting.GroupName = "Paypal";
            passwordSetting.Key = "PaypalPassword";
            passwordSetting.Value = request.PaypalPassword;
            Upsert(passwordSetting);

            StoreSetting signSetting = new StoreSetting();
            signSetting.StoreId = idCreate;
            signSetting.GroupName = "Paypal";
            signSetting.Key = "PaypalSignature";
            signSetting.Value = request.PaypalSignature;
            Upsert(signSetting);

            return request.Id;

        }

        private void Upsert(StoreSetting setting)
        {
            StoreSetting editMe = db.StoreSettings
                .Where(a => a.StoreId == setting.StoreId && a.GroupName == setting.GroupName && a.Key == setting.Key).FirstOrDefault();

            if (editMe != null)
            {
                editMe.Value = setting.Value;
            }
            else
            {
                editMe = setting;
                db.StoreSettings.Add(setting);
            }

            db.SaveChanges();
        }
        #endregion
    }
}
