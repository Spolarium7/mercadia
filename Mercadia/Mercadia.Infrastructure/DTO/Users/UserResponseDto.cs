using Mercadia.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercadia.Infrastructure.DTO.Users
{
    public class UserResponseDto
    {
        public Guid Id { get; set; }
        public DateTime Timestamp { get; set; }
        public UserStatus Status { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string DeliveryAddress { get; set; }
        public string DeliveryState { get; set; }
        public string DeliveryCountry { get; set; }
    }
}
