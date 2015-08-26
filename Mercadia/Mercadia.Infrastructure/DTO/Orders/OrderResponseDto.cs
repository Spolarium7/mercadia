using Mercadia.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercadia.Infrastructure.DTO.Orders
{
    public class OrderResponseDto
    {
        public string StoreId { get; set; }
        public string StoreName { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string StoreOwnerId { get; set; }
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
