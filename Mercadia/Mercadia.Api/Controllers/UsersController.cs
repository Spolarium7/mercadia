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

        [HttpGet, Route("{id}")]
        public UserResponseDto Get(string id)
        {
            Guid idCompare = Guid.Parse(id);

            return db.Users
                .Where(a => a.Id == idCompare)
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

        [HttpPost, Route("")]
        public string Post(UserRequestDto request)
        {
                if (db.Users.Where(a => a.Email.ToLower() == request.Email.ToLower()).Count() != 0)
                {
                    ThrowError("Email already in use");
                    return null;
                };

                string verifyCode = System.Web.Security.Membership.GeneratePassword(6, 0);

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

        [HttpPut, Route("editprofile")]
        public string Put(UserProfileRequestDto request)
        {

            Guid idCompare = Guid.Parse(request.Id);
            User user = db.Users.Where(a => a.Id == idCompare).FirstOrDefault();

            if (user == null)
            {
                ThrowError("User not found");
                return null;
            };

            user.DeliveryAddress = request.DeliveryAddress;
            user.DeliveryCountry = request.DeliveryCountry;
            user.DeliveryState = request.DeliveryState;
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Phone = request.Phone;
            db.SaveChanges();

            return user.Id.ToString();

        }

        [HttpPut, Route("changepassword")]
        public string ChangePassword(ChangePasswordRequestDto request)
        {
            Guid idCompare = Guid.Parse(request.Id);
            User user = db.Users.Where(a => a.Id == idCompare).FirstOrDefault();

            /* update user status */
            user.ClearTextPassword = request.NewPassword;
            user.PasswordIsGenerated = false;
            db.SaveChanges();

            return user.Id.ToString();

        }

        [HttpPost, Route("verify")]
        public string Verify(UserVerifyRequestDto request)
        {
            Guid Id = new Guid(request.Id);
            var user = db.Users.Where(a => a.Id == Id && a.VerificationCode == request.VerificationCode).FirstOrDefault();

            if (user == null)
            {
                ThrowError("User not found");
                return null;
            }

            user.Status = UserStatus.Active;
            db.Entry(user).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return user.Id.ToString();
        }


        [HttpPost, Route("login"), AllowAnonymous]
        public UserResponseDto Login(UserLoginRequestDto request)
        {
            var user = db.Users.Where(a => a.Email.ToLower() == request.Email.ToLower()).FirstOrDefault();

            if (BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                user.NoOfLoginRetries = 0;
                db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                user.NoOfLoginRetries = user.NoOfLoginRetries + 1;
                if (user.NoOfLoginRetries >= 3 && (user.Status == UserStatus.Active || user.Status == UserStatus.Unverified))
                {
                    user.Status = UserStatus.Locked;
                }
                db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                user = null;
            }


            if (user == null)
            {
                ThrowError("Email or password is invalid.");
                return null;
            }

            if (user.Status == UserStatus.Unverified
                || user.Status == UserStatus.Inactive
                || user.Status == UserStatus.Locked)
            {
                ThrowError("User account is inactive.");
                return null;
            }

            return new UserResponseDto()
            {
                Email = user.Email,
                DeliveryAddress = user.DeliveryAddress,
                DeliveryCountry = user.DeliveryCountry,
                DeliveryState = user.DeliveryState,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phone = user.Phone,
                Id = user.Id,
                Status = user.Status,
                Timestamp = user.Timestamp
            };
        }
    }
}