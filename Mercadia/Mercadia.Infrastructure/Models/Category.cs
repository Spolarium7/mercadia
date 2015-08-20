using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercadia.Infrastructure.Models
{
    public class Category: BaseModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid ParentId { get; set; }
        public Guid StoreId { get; set; }
    }
}
