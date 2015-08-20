using Mercadia.Infrastructure.DTO.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mercadia.Web.ViewModels.Categories
{
    public class CategoriesByStoreViewModel
    {
        public List<CategoryResponseDto> Categories { get; set; }

        public string StoreId { get; set; }
    }
}