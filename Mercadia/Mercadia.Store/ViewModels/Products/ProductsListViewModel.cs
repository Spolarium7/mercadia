using Mercadia.Infrastructure.DTO.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mercadia.Store.ViewModels.Products
{
    public class ProductsListViewModel
    {
        public List<ProductResponseDto> Products { get; set; }
        public string CategoryId { get; set; }
        public string Category { get; set; }
        public string StoreId { get; set; }
        public string Store { get; set; }
    }
}