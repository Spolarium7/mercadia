using Mercadia.Infrastructure.DTO.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mercadia.Web.ViewModels.Account
{
    public class DashboardViewModel
    {
        public List<StoreResponseDto> MyStores { get; set; }
        public List<StoreResponseDto> Stores { get; set; }
    }
}