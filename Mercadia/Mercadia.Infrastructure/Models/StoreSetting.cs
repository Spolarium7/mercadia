using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercadia.Infrastructure.Models
{
    public class StoreSetting : BaseModel
    {
        public Guid StoreId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }

    }
}
