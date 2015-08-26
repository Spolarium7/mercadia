using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mercadia.Web.ViewModels.Products
{
    public class ProductUploadThumbnailViewModel
    {
        public string Id { get; set; }
        [DataType(DataType.Upload)]
        public HttpPostedFileBase File { get; set; }
        public string CategoryId { get; set; }
    }
}