using Mercadia.Infrastructure.DTO.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mercadia.Store.ViewModels
{
    public class StoresViewModel
    {
        public List<StoreResponseDto> Stores { get; set; }
    }
}