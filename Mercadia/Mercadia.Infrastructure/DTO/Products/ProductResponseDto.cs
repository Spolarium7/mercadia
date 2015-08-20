using Mercadia.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercadia.Infrastructure.DTO.Products
{
    public class ProductResponseDto
    {
        public string Id { get; set; }
        public string Sku { get; set; }
        public Guid StoreId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ProfilePic { get; set; }
        public Guid CategoryId { get; set; }
        public decimal Price { get; set; }
        public Status Status { get; set; }
    }
}
