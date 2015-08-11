using Mercadia.Infrastructure.Enums;
using Mercadia.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercadia.Infrastructure.DTO
{
    public class BaseDto
    {
        public Guid Id { get; set; }

        public DateTime Timestamp { get; set; }

        public Status Status { get; set; }
    }
}
