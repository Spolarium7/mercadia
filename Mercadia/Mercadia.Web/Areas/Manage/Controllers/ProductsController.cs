using Mercadia.Infrastructure.DTO;
using Mercadia.Infrastructure.DTO.Categories;
using Mercadia.Infrastructure.DTO.Products;
using Mercadia.Web.Controllers;
using Mercadia.Web.Securities;
using Mercadia.Web.ViewModels.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Mercadia.Web.Areas.Manage.Controllers
{
    [InPrivate]
    [RoutePrefix("manage/products")]
    public class ProductsController : BaseController
    {
        #region Index
        [HttpGet, Route("list/{id}")]
        public ActionResult List(string id)
        {
            CategoryNameIdPairDto categoryDetails = GetCategoryDetails(id);


            var response = Get<List<ProductResponseDto>>("products//" + categoryDetails.StoreId + "//" + id);

            /* Test RESULTS - OKAY */
            if (response.Status == HttpStatusCode.OK)
            {

                ProductsListViewModel viewModel = new ProductsListViewModel();
                viewModel.Products = response.Data;
                viewModel.StoreId = categoryDetails.StoreId;
                viewModel.Store = categoryDetails.StoreName;
                viewModel.CategoryId = categoryDetails.CategoryId;
                viewModel.Category = categoryDetails.CategoryName;
                return View(viewModel);
            }
            /* Test RESULTS - Api Validation Error */
            else if (response.Status == HttpStatusCode.BadRequest)
            {
                this.ModelState.AddModelError("", response.Message);
                return View();
            }

            return View();
        }

        private CategoryNameIdPairDto GetCategoryDetails(string categoryId)
        {
            var response = Get<CategoryNameIdPairDto>("categories//get-details//" + categoryId);

            /* Test RESULTS - OKAY */
            if (response.Status == HttpStatusCode.OK)
            {
                return response.Data;
            }
            /* Test RESULTS - Api Validation Error */
            else if (response.Status == HttpStatusCode.BadRequest)
            {
                this.ModelState.AddModelError("", response.Message);
                return null;
            }

            return null;
        }

        #endregion


        #region AddNew
        [HttpGet, Route("{id}")]
        public ActionResult Add(string id)
        {
            ProductRequestDto request = new ProductRequestDto();
            CategoryNameIdPairDto categoryDetails = GetCategoryDetails(id);
            request.StoreId = categoryDetails.StoreId;
            request.CategoryId = id;
            return View(request);
        }

        [HttpPost]
        public ActionResult Add(ProductRequestDto request)
        {
            /* Api CALL */
            var response = Post<string>("products", request);

            /* Test RESULTS - OKAY */
            if (response.Status == HttpStatusCode.OK)
            {
                return RedirectToAction("list", new { Id = request.CategoryId });
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
            var response = Get<ProductResponseDto>("products//" + id);

            /* Test RESULTS - OKAY */
            if (response.Status == HttpStatusCode.OK)
            {
                ProductRequestDto product = new ProductRequestDto();
                product.Name = response.Data.Name;
                product.StoreId = response.Data.StoreId.ToString();
                product.Description = response.Data.Description;
                product.Id = response.Data.Id.ToString();
                product.CategoryId = response.Data.CategoryId.ToString();
                product.Price = response.Data.Price;
                product.Sku = response.Data.Sku;                
                return View(product);
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
        public ActionResult Update(ProductRequestDto request)
        {
            /* Api CALL */
            var response = Put<string>("products", request);

            /* Test RESULTS - OKAY */
            if (response.Status == HttpStatusCode.OK)
            {
                return RedirectToAction("list", new { Id = request.CategoryId });
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

        [HttpPost]
        public ActionResult ChangeStatus(ProductChangeStatusViewModel request)
        {
            /* Api CALL */
            var response = Put<string>("products//changestatus", request);

            /* Test RESULTS - OKAY */
            if (response.Status == HttpStatusCode.OK)
            {
                return RedirectToAction("list", new { Id = request.CategoryId });
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

        #region Change Profile Pic
        public ActionResult ChangeProfilePic(ProductUploadThumbnailViewModel model)
        {
            var file = model.File;

            if (!ModelState.IsValid || file == null)
            {
                 this.ModelState.AddModelError("", "Please verify and complete the required details");
                return RedirectToAction("list", new { Id = model.CategoryId });
            }
            
            if(file.ContentLength >  1048576 )
            {
                 this.ModelState.AddModelError("", "Maximum allowed file size is 1 MB");
                return RedirectToAction("list", new { Id = model.CategoryId });
            }

            /* Api CALL */
            byte[] image = FileToByteArray(file.InputStream, file.ContentLength);

            var Image = Convert.ToBase64String(image);
            /* Api CALL */
            var response = Put<string>("products/changeprofilepic", new ProductUploadThumbnailRequestDto { Id = model.Id, Image = Image});

            /* Test RESULTS - OKAY */
            if (response.Status == HttpStatusCode.OK)
            {
                return RedirectToAction("list", new { Id = model.CategoryId });
            }
            /* Test RESULTS - Api Validation Error */
            else if (response.Status == HttpStatusCode.BadRequest)
            {
                this.ModelState.AddModelError("", response.Message);
                return RedirectToAction("list", new { Id = model.CategoryId });
            }

            /* If we got this far, something failed, redisplay form */
                return RedirectToAction("list", new { Id = model.CategoryId });
        }
        #endregion
    }
}