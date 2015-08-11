using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mercadia.Web.Controllers
{
    public class MenuController : Controller
    {
        // GET: Menu
        public ActionResult _Menu()
        {
            return PartialView();
        }
    }
}