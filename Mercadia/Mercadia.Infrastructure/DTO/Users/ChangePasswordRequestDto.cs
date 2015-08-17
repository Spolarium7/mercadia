using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercadia.Infrastructure.DTO.Users
{
    public class ChangePasswordRequestDto
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string NewPassword { get; set; }
    }
}
