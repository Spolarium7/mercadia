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

namespace Mercadia.Api.Controllers
{

    [RoutePrefix("api/users")]
    public class UsersController : ApiController
    {
        private MercadiaDbContext db = new MercadiaDbContext();

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
        public Guid Post(UserRequestDto request)
        {
            
        }
    }
}