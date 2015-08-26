using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mercadia.Web.ViewModels.Stores
{
    public class CartItemViewModel
    {
        public int Number { get; set; }
        public string ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}