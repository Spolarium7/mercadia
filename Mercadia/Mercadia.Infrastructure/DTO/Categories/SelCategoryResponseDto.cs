using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercadia.Infrastructure.DTO.Categories
{
    public class SelCategoryResponseDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid ParentId { get; set; }
        public Guid StoreId { get; set; }
        public Boolean IsSelected { get; set; }
    }
}
