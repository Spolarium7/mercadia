using Mercadia.Infrastructure.DTO.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mercadia.Web.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            //var user = Get<UserResponseDto>("users/email/gjimenez@mercadia.com/");
            var user = Post<Guid>("users", new UserRequestDto());
            return View();
        }

    }
}