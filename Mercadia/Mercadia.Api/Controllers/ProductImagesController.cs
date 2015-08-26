using ImageResizer;
using Mercadia.Api.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;

namespace Mercadia.Api.Controllers
{
    public class ProductImagesController : Controller
    {
        public ActionResult Render(string file)
        {
            var fullFilePath = this.GetFullFilePath(file ?? "default.png");

            if (this.ImageFileNotAvailable(fullFilePath))
            {
                //return this.Instantiate404ErrorResult(file); // string.Format("{0}/{1}", Server.MapPath("~/App_Data/UserProfiles"), "default.png")
                return new ImageFileResult(string.Format("{0}/{1}", Server.MapPath("~/App_Data/Images/Products"), "default.png"));
            }

            return new ImageFileResult(fullFilePath);
        }

        private string GetFullFilePath(string file)
        {
            return string.Format("{0}/{1}", Server.MapPath("~/App_Data/Images/Products"), file);
        }

        private bool ImageFileNotAvailable(string fullFilePath)
        {
            return !System.IO.File.Exists(fullFilePath);
        }

        private HttpNotFoundResult Instantiate404ErrorResult(string file)
        {
            return new HttpNotFoundResult(string.Format("The file {0} does not exist.", file));
        }
    }
}