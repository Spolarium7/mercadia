using System.Web;
using System.Web.Mvc;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Web.Caching;

namespace Mercadia.Api.Helpers
{
    public class ImageFileResult : FilePathResult
    {
        public ImageFileResult(string fileName) :
            base(fileName, string.Format("image/{0}", fileName.FileExtensionForContentType()))
        {
        }

        protected override void WriteFile(HttpResponseBase response)
        {
            response.SetDefaultImageHeaders();
            base.WriteFile(response);
        }
    }

    public static class FilesystemExtensionMethods
    {
        public static string FileExtensionForContentType(this string fileName)
        {
            var pieces = fileName.Split('.');
            var extension = pieces.Length > 1 ? pieces[pieces.Length - 1] : string.Empty;
            return (extension.ToLower() == "jpg") ? "jpeg" : extension;
        }

        public static byte[] ToByteArray(this Bitmap image)
        {
            byte[] data;
            using (var memoryStream = new MemoryStream())
            {
                image.Save(memoryStream, ImageFormat.Bmp);
                data = memoryStream.ToArray();
            }
            return data;
        }
    }
    public static class HttpResponseExtensionMethods
    {
        public static void SetDefaultImageHeaders(this HttpResponseBase response)
        {
            response.Cache.SetCacheability(HttpCacheability.Public);
            response.Cache.SetExpires(Cache.NoAbsoluteExpiration);
            response.Cache.SetLastModifiedFromFileDependencies();
        }
    }

}