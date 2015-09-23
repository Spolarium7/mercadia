using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mercadia.Store.ViewModels.Stores
{
    public class StoreUploadThumbnailViewModel
    {
        public string Id { get; set; }
        [DataType(DataType.Upload)]
        public HttpPostedFileBase File { get; set; }
    }
}