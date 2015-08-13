using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercadia.Infrastructure.DTO.Users
{
    public class UserVerifyRequestDto
    {
        public string Id { get; set; }
        public string VerificationCode { get; set; }
    }
}
