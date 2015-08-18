using Mercadia.Api.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;

namespace Mercadia.Api.Controllers
{
    public class BaseApiController : ApiController
    {
        protected MercadiaDbContext db = new MercadiaDbContext();
        public object ThrowError(string message)
        {
            HttpResponseMessage responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);
            responseMessage.Content = new ObjectContent<string>(message, new JsonMediaTypeFormatter());
            throw new HttpResponseException(responseMessage);
        }

        protected string CreateFolderIfNeeded(params string[] paths)
        {
            var path = Path.Combine(paths);

            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch (Exception)
                {
                    /*TODO: You must process this exception.*/
                }
            }
            return path;
        }

        protected void CleanFiles(string filename)
        {
            var filepath = HttpContext.Current.Server.MapPath("~/Content/Images/Stores");
            if (File.Exists(filepath + "/" + filename + ".png"))
            {
                File.Delete(filepath + "/" + filename + ".png");
            }
            if (File.Exists(filepath + "/" + filename + ".jpg"))
            {
                File.Delete(filepath + "/" + filename + ".jpg");
            }
            if (File.Exists(filepath + "/" + filename + ".gif"))
            {
                File.Delete(filepath + "/" + filename + ".gif");
            }
        }
    }
}
