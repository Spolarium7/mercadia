using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercadia.Infrastructure.Models
{
    public class OrderItem: BaseModel
    {
        public Guid OrderId { get; set; }
        public Guid StoreId { get; set; }
        public string StoreName { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public Guid StoreOwnerId { get; set; }
        public string StoreOwnerName { get; set; }
        public DateTime OrderDate { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal ItemPrice { get; set; }
    }
}
