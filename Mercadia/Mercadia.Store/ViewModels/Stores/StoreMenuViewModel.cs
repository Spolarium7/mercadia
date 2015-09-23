using Mercadia.Infrastructure.DTO.Categories;
using Mercadia.Infrastructure.DTO.Products;
using Mercadia.Infrastructure.DTO.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mercadia.Store.ViewModels.Stores
{
    public class StoreMenuViewModel
    {
        public StoreResponseDto Store { get; set; }
        public List<ProductResponseDto> Products { get; set; }
        public List<SelCategoryResponseDto> Categories { get; set; }
    }
}