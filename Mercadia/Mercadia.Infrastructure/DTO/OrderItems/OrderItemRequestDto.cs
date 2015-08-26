using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercadia.Infrastructure.DTO.OrderItems
{
    public class OrderItemRequestDto
    {
        public string OrderId { get; set; }
        public string StoreId { get; set; }
        public string StoreName { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string StoreOwnerId { get; set; }
        public string StoreOwnerName { get; set; }
        public DateTime OrderDate { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal ItemPrice { get; set; }
    }
}
