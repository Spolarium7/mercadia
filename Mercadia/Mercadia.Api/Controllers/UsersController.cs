using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Mercadia.Api.Models;
using Mercadia.Infrastructure.Models;
using Mercadia.Infrastructure.Enums;
using Mercadia.Infrastructure.DTO;
using Mercadia.Infrastructure.DTO.Users;
using Mercadia.Api.Mailers;
using Mercadia.Api.Helpers;
using System.Net.Http.Formatting;

namespace Mercadia.Api.Controllers
{

    [RoutePrefix("api/users")]
    public class UsersController : BaseApiController
    {


        // GET: api/Users

        [HttpGet, Route("list")]
        public List<UserResponseDto> List()
        {
            return db.Users.Select(a => new UserResponseDto() { 
                Email = a.Email,
                DeliveryAddress = a.DeliveryAddress,
                DeliveryCountry = a.DeliveryCountry,
                DeliveryState = a.DeliveryState,
                FirstName = a.FirstName,
                LastName =a.LastName,
                Phone = a.Phone,
                Id = a.Id,
                Status = a.Status,
                Timestamp = a.Timestamp
            }).ToList();
        }

        [HttpGet, Route("email/{email}/")]
        public UserResponseDto GetByEmail(string email)
        {
            return db.Users
                .Where(a => a.Email.ToLower() == email.ToLower())
                .Select(a => new UserResponseDto()
                {
                    Email = a.Email,
                    DeliveryAddress = a.DeliveryAddress,
                    DeliveryCountry = a.DeliveryCountry,
                    DeliveryState = a.DeliveryState,
                    FirstName = a.FirstName,
                    LastName = a.LastName,
                    Phone = a.Phone,
                    Id = a.Id,
                    Status = a.Status,
                    Timestamp = a.Timestamp
                }).FirstOrDefault();
        }

        [HttpPost]
        public string Post(UserRequestDto request)
        {
                if (db.Users.Where(a => a.Email.ToLower() == request.Email.ToLower()).Count() != 0)
                {
                    ThrowError("Email already in use");
                    return null;
                };

                string verifyCode = System.Web.Security.Membership.GeneratePassword(6, 2);

                User user = new User();
                user.ClearTextPassword = request.Password;
                user.Email = request.Email.ToLower();
                user.DeliveryAddress = request.DeliveryAddress;
                user.DeliveryCountry = request.DeliveryCountry;
                user.DeliveryState = request.DeliveryState;
                user.FirstName = request.FirstName;
                user.LastName = request.LastName;
                user.Phone = request.Phone;
                user.Status = UserStatus.Unverified;
                user.VerificationCode = verifyCode;
                user.PasswordIsGenerated = true;
                db.Users.Add(user);
                db.SaveChanges();

                new UserMailer()
                    .Welcome(name: string.Format("{0} {1}", user.LastName, user.FirstName), email: user.Email, verificationCode: verifyCode)
                    .SendNow();

                return user.Id.ToString();

        }
    }
}