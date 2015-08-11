using Mercadia.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercadia.Infrastructure.Models
{
    public class User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ClearTextPassword
        {
            set { Password = !string.IsNullOrEmpty(value) ? BCrypt.Net.BCrypt.HashPassword(value) : null; }
        }
        public string Phone { get; set; }
        public string DeliveryAddress { get; set; }
        public string DeliveryState { get; set; }
        public string DeliveryCountry { get; set; }
        public User()
        {
            this.Timestamp = DateTime.Now;
            this.Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public string VerificationCode { get; set; }
        public Boolean PasswordIsGenerated { get; set; }
        public DateTime Timestamp { get; set; }
        public UserStatus Status { get; set; }
    }
}
