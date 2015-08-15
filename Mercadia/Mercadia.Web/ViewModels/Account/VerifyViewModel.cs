using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mercadia.Web.ViewModels.Account
{
    public class VerifyViewModel
    {
        public Guid Id { get; set; }
        public string VerificationCode { get; set; }
    }
}