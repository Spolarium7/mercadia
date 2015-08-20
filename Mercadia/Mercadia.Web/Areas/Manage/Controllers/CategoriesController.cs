using Mercadia.Infrastructure.DTO.Categories;
using Mercadia.Web.Controllers;
using Mercadia.Web.Securities;
using Mercadia.Web.ViewModels.Categories;
using Mercadia.Web.ViewModels.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Mercadia.Web.Areas.Manage.Controllers
{
    [InPrivate]
    [RoutePrefix("manage/categories")]
    public class CategoriesController : BaseController
    {
        #region Index
        [HttpGet, Route("bystore/{id}")]
        public ActionResult ByStore(string id)
        {
            var response = Get<List<CategoryResponseDto>>("categories//bystore//" + id);

            /* Test RESULTS - OKAY */
            if (response.Status == HttpStatusCode.OK)
            {
                CategoriesByStoreViewModel categoriesByStore = new CategoriesByStoreViewModel();
                categoriesByStore.Categories = response.Data;
                categoriesByStore.StoreId = id;
                return View(categoriesByStore);
            }
            /* Test RESULTS - Api Validation Error */
            else if (response.Status == HttpStatusCode.BadRequest)
            {
                this.ModelState.AddModelError("", response.Message);
                return View();
            }

            return View();
        }
        #endregion

        #region AddNew
        [HttpGet, Route("{id}")]
        public ActionResult Add(string id)
        {
            CategoryRequestDto request = new CategoryRequestDto();
            request.StoreId = id;
            return View(request);
        }

        [HttpPost]
        public ActionResult Add(CategoryRequestDto request)
        {
            /* Api CALL */
            var response = Post<string>("categories", request);

            /* Test RESULTS - OKAY */
            if (response.Status == HttpStatusCode.OK)
            {
                return RedirectToAction("bystore", new { Id = request.StoreId });
            }
            /* Test RESULTS - Api Validation Error */
            else if (response.Status == HttpStatusCode.BadRequest)
            {
                this.ModelState.AddModelError("", response.Message);
                return View(request);
            }

            /* If we got this far, something failed, redisplay form */
            return View(request);

        }
        #endregion

        #region Update
        [HttpGet, Route("{id}")]
        public ActionResult Update(string id)
        {
            var response = Get<CategoryResponseDto>("categories//" + id);

            /* Test RESULTS - OKAY */
            if (response.Status == HttpStatusCode.OK)
            {
                CategoryRequestDto category = new CategoryRequestDto();
                category.Name = response.Data.Name;
                category.StoreId = response.Data.StoreId.ToString();
                category.Description = response.Data.Description;
                category.Id = response.Data.Id.ToString();
                category.ParentId = response.Data.ParentId.ToString();
                return View(category);
            }
            /* Test RESULTS - Api Validation Error */
            else if (response.Status == HttpStatusCode.BadRequest)
            {
                this.ModelState.AddModelError("", response.Message);
                return View();
            }

            return View();
        }

        [HttpPost]
        public ActionResult Update(CategoryRequestDto request)
        {
            /* Api CALL */
            var response = Put<string>("categories", request);

            /* Test RESULTS - OKAY */
            if (response.Status == HttpStatusCode.OK)
            {
                return RedirectToAction("bystore", new { Id = request.StoreId });
            }
            /* Test RESULTS - Api Validation Error */
            else if (response.Status == HttpStatusCode.BadRequest)
            {
                this.ModelState.AddModelError("", response.Message);
                return View(request);

            }

            /* If we got this far, something failed, redisplay form */
            return View(request);


        }
        #endregion
    }
}