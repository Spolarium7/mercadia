using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercadia.Infrastructure.DTO.Stores
{
    public class StoreResponseDto : BaseDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ProfilePic { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string LocationLat { get; set; }
        public string LocationLong { get; set; }
        public string Template { get; set; }
    }
}
