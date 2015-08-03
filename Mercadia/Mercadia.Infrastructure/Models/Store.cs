using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercadia.Infrastructure.Models
{
    public class Store : BaseModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string LocationLat { get; set; }
        public string LocationLong { get; set; }
        public string Template { get; set; }
    }
}
