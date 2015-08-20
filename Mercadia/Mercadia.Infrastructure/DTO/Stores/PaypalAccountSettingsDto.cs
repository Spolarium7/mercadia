using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercadia.Infrastructure.DTO.Stores
{
    public class PaypalAccountSettingsDto
    {
        public string Id { get; set; }
        public string PaypalFacilitator { get; set; }
        public string PaypalPassword { get; set; }
        public string PaypalSignature { get; set; }
    }
}
