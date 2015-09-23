using Mercadia.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mercadia.Store.ViewModels.Products
{
    public class ProductChangeStatusViewModel
    {
        public string Id { get; set; }
        public Status Status { get; set; }
        public string CategoryId { get; set; }
    }
}