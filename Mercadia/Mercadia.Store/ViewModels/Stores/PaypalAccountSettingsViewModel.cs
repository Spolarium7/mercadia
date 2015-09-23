using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mercadia.Store.ViewModels.Stores
{
    public class PaypalAccountSettingsViewModel
    {
        public string Id { get; set; }
        public string PaypalFacilitator { get; set; }
        public string PaypalPassword { get; set; }
        public string PaypalSignature { get; set; }
    }
}