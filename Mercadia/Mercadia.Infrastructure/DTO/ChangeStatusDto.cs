using Mercadia.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercadia.Infrastructure.DTO
{
    public class ChangeStatusDto
    {
        public string Id { get; set; }
        public Status Status { get; set; }
    }
}
