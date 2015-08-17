using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Mercadia.Web.Securities
{
    public class InPrivate : System.Web.Mvc.AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (WebUser.CurrentUser == null)
            {
                //HttpContext.Current.GetOwinContext().Authentication.SignOut();
                return false;
            }
            else
            {
                return true;
            }

        }
    }
}
