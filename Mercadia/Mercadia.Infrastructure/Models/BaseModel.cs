using Mercadia.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercadia.Infrastructure.Models
{
    public class BaseModel
    {
        public BaseModel()
        {
            this.Timestamp = DateTime.Now;
            this.Status = Status.Inactive;
            this.Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        public DateTime Timestamp { get; set; }

        public Status Status {get; set; }
    }
}
