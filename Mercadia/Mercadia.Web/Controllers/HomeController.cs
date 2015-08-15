using Mercadia.Infrastructure.DTO.Stores;
using Mercadia.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Mercadia.Web.Controllers
{
    public class HomeController : BaseController
    {


        public ActionResult Index()
        {
            StoresViewModel model = new StoresViewModel();
            string featured = ConfigurationManager.AppSettings["FeaturedStores"];
            var response = Get<List<StoreResponseDto>>("stores//listbyid//" + featured + "//");

            /* Test RESULTS - OKAY */
            if (response.Status == HttpStatusCode.OK)
            {
                model.Stores = response.Data;

                return View(model);
            }
            /* Test RESULTS - Api Validation Error */
            else if (response.Status == HttpStatusCode.BadRequest)
            {
                this.ModelState.AddModelError("", response.Message);
                return View();
            }
            return View();
        }

    }
}