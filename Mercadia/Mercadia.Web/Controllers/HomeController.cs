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
    [RoutePrefix("home")]
    public class HomeController : BaseController
    {

        [AllowAnonymous, Route("index/{keyword}")]
        public ActionResult Index(string keyword)
        {
            StoresViewModel model = new StoresViewModel();
            var response = new Dto<List<StoreResponseDto>>();

            if (keyword == null)
            {
                response = Get<List<StoreResponseDto>>("stores//list");
            }
            else
            {
                response = Get<List<StoreResponseDto>>("stores//search//" + keyword);
            }

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