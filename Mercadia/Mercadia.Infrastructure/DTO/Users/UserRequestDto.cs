using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercadia.Infrastructure.DTO.Users
{
    public class UserRequestDto
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        public string Phone { get; set; }

        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        [Required]
        public string DeliveryAddress { get; set; }
        [Required]
        public string DeliveryState { get; set; }
        [Required]
        public string DeliveryCountry { get; set; }
    }
}
