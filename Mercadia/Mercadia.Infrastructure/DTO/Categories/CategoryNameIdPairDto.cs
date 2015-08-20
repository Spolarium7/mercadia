using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercadia.Infrastructure.DTO.Categories
{
    public class CategoryNameIdPairDto
    {
        public string StoreId { get; set; }
        public string StoreName { get; set; }
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}
