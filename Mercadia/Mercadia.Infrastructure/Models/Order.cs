using Mercadia.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercadia.Infrastructure.Models
{
    public class Order: BaseModel
    {
        public Guid StoreId { get; set; }
        public string StoreName { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public Guid StoreOwnerId { get; set; }
        public string StoreOwnerName { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal TaxDue { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public string PaymentDetails { get; set; }
        public string PaymentReference { get; set; }
        public DateTime PayDate { get; set; }
    }
}
