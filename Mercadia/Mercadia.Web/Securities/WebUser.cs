using Mercadia.Infrastructure.DTO.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mercadia.Web.Securities
{
    public static class WebUser
    {
        public static UserResponseDto CurrentUser
        {
            get
            {
                if (HttpContext.Current.Session != null && HttpContext.Current.Session["CurrentUser"] != null)
                {
                    return (UserResponseDto)HttpContext.Current.Session["CurrentUser"];
                }

                return null;
            }
            set { HttpContext.Current.Session["CurrentUser"] = value; }
        }
    }
}