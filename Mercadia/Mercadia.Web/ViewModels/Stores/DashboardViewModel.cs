using Mercadia.Infrastructure.DTO.OrderItems;
using Mercadia.Infrastructure.DTO.Orders;
using Mercadia.Infrastructure.DTO.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mercadia.Web.ViewModels.Stores
{
    public class DashboardViewModel
    {
        public List<OrderResponseDto> Orders { get; set; }
        public List<OrderItemResponseDto> Products { get; set; }
        public StoreResponseDto Store { get; set; }
        public Dictionary<string, int> GraphData { get; set; }
    }
}