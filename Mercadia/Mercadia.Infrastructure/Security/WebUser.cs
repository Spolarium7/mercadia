using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Mercadia.Infrastructure.Security
{
    public static class WebUser
    {
        public string Token {
            get
            {
                if (HttpContext.Current.Session != null && HttpContext.Current.Session["Token"] != null)
                {
                    return (string)HttpContext.Current.Session["Token"];
                }

                return null;
            }
            set { HttpContext.Current.Session["Token"] = value; }
        }
    }
}
