using Mercadia.Infrastructure.DTO.Stores;
using Mercadia.Web.Controllers;
using Mercadia.Web.Securities;
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
    [RoutePrefix("manage/stores")]
    public class StoresController : BaseController
    {
        #region AddNew
        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(StoreRequestDto request)
        {          
            /* Api CALL */
            var response = Post<string>("stores", request);

            /* Test RESULTS - OKAY */
            if (response.Status == HttpStatusCode.OK)
            {
                WebUser.UserStores = GetStoresByUser();
                return RedirectToAction("update", new { Id = response.Data });
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
        [HttpGet, Route("/{id}")]
        public ActionResult Update(string Id)
        {

            var response = Get<StoreResponseDto>("stores//" + Id);

            /* Test RESULTS - OKAY */
            if (response.Status == HttpStatusCode.OK)
            {
                StoreRequestDto store = new StoreRequestDto();
                store.Name = response.Data.Name;
                store.Address = response.Data.Address;
                store.Description = response.Data.Description;
                store.Id = response.Data.Id.ToString();
                store.ZipCode = response.Data.ZipCode;
                store.ProfilePic = response.Data.ProfilePic;
                store.Template = response.Data.Template;
                return View(store);
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
        public ActionResult Update(StoreRequestDto request)
        {
            /* Api CALL */
            var response = Put<string>("stores", request);

            /* Test RESULTS - OKAY */
            if (response.Status == HttpStatusCode.OK)
            {
                return RedirectToAction("update", new { Id = request.Id });
            }
            /* Test RESULTS - Api Validation Error */
            else if (response.Status == HttpStatusCode.BadRequest)
            {
                this.ModelState.AddModelError("", response.Message);
                return RedirectToAction("update", new { Id = request.Id });
            }

            /* If we got this far, something failed, redisplay form */
            return RedirectToAction("update", new { Id = request.Id });

        }

        [HttpPost, Route("UpdateTemplate")]
        public ActionResult UpdateTemplate(StoreRequestDto request)
        {
            /* Api CALL */
            var response = Put<string>("stores/changetemplate", request);

            /* Test RESULTS - OKAY */
            if (response.Status == HttpStatusCode.OK)
            {
                return RedirectToAction("update", new { Id = request.Id });
            }
            /* Test RESULTS - Api Validation Error */
            else if (response.Status == HttpStatusCode.BadRequest)
            {
                this.ModelState.AddModelError("", response.Message);
                return RedirectToAction("update", new { Id = request.Id });
            }

            /* If we got this far, something failed, redisplay form */
            return RedirectToAction("update", new { Id = request.Id });

        }

        public ActionResult ChangeProfilePic(StoreUploadThumbnailViewModel model)
        {
            var file = model.File;

            if (!ModelState.IsValid || file == null)
            {
                 this.ModelState.AddModelError("", "Please verify and complete the required details");
                 return RedirectToAction("update", new { Id = model.Id });
            }
            
            if(file.ContentLength >  1048576 )
            {
                 this.ModelState.AddModelError("", "Maximum allowed file size is 1 MB");
                 return RedirectToAction("update", new { Id = model.Id });
            }

            /* Api CALL */
            byte[] image = FileToByteArray(file.InputStream, file.ContentLength);

            var Image = Convert.ToBase64String(image);
            /* Api CALL */
            var response = Put<string>("stores/changeprofilepic", new StoreUploadThumbnailRequestDto { Id = model.Id, Image = Image});

            /* Test RESULTS - OKAY */
            if (response.Status == HttpStatusCode.OK)
            {
                return RedirectToAction("update", new { Id = response.Data.ToString() });
            }
            /* Test RESULTS - Api Validation Error */
            else if (response.Status == HttpStatusCode.BadRequest)
            {
                this.ModelState.AddModelError("", response.Message);
                return RedirectToAction("update", new { Id = model.Id });
            }

            /* If we got this far, something failed, redisplay form */
            return RedirectToAction("update", new { Id = model.Id });
        }
        #endregion

        public List<StoreResponseDto> GetStoresByUser()
        {
            var response = Get<List<StoreResponseDto>>("stores//byowner//" + WebUser.CurrentUser.Id);

            /* Test RESULTS - OKAY */
            if (response.Status == HttpStatusCode.OK)
            {
                return response.Data;
            }
            /* Test RESULTS - Api Validation Error */
            else if (response.Status == HttpStatusCode.BadRequest)
            {
                return null;
            }

            return null;
        }

    }
}