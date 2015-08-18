using ImageResizer;
using Mercadia.Infrastructure.DTO.Stores;
using Mercadia.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Mercadia.Api.Controllers
{
    [RoutePrefix("api/stores")]
    public class StoresController : BaseApiController
    {
        [HttpGet, Route("{id}")]
        public StoreResponseDto Get(string id)
        {
            Guid idCompare = Guid.Parse(id);

            var path = Url.Content("/content/images/stores/");

            return db.Stores
                .Where(a => a.Id == idCompare)
                .Select(a => new StoreResponseDto()
                {
                    Name = a.Name,
                    Address = a.Address,
                    Description = a.Description,
                    ProfilePic = path + a.ProfilePic,
                    Template = a.Template,
                    ZipCode = a.ZipCode,
                    Id = a.Id,
                    Status = a.Status,
                    Timestamp = a.Timestamp
                }).FirstOrDefault();
        }

        [HttpGet, Route("list")]
        public List<StoreResponseDto> List()
        {
            var path = Url.Content("/content/images/stores/");

            return db.Stores
                .Select(a => new StoreResponseDto()
                {
                    Name = a.Name,
                    Address = a.Address,
                    Description = a.Description,
                    ProfilePic = path + a.ProfilePic,
                    Template = a.Template,
                    ZipCode = a.ZipCode,
                    Id = a.Id,
                    Status = a.Status,
                    Timestamp = a.Timestamp
                }).ToList();
        }

        [HttpGet, Route("byowner/{id}")]
        public List<StoreResponseDto> ListByOwner(string id)
        {
            Guid idCompare = Guid.Parse(id);

            var path = Url.Content("/content/images/stores/");

            return db.Stores
                .Where(a => a.StoreOwnerId == idCompare)    
                .Select(a => new StoreResponseDto()
                {
                    Name = a.Name,
                    Address = a.Address,
                    Description = a.Description,
                    ProfilePic = path + a.ProfilePic,
                    Template = a.Template,
                    ZipCode = a.ZipCode,
                    Id = a.Id,
                    Status = a.Status,
                    Timestamp = a.Timestamp
                }).ToList();
        }

        [HttpGet, Route("listbyid/{ids}")]
        public List<StoreResponseDto> ListById(string ids)
        {
            var idArr = ids.Split(',');
            List<Guid> idsCompare = new List<Guid>();            
            foreach(string id in idArr){
                Guid idCompare = Guid.Parse(id);
                idsCompare.Add(idCompare);
            }

            var path = Url.Content("/content/images/stores/");

            return db.Stores
                .Where(a => idsCompare.Contains(a.Id))
                .Select(a => new StoreResponseDto()
                {
                    Name = a.Name,
                    Address = a.Address,
                    Description = a.Description,
                    ProfilePic = path + a.ProfilePic,
                    Template = a.Template,
                    ZipCode = a.ZipCode,
                    Id = a.Id,
                    Status = a.Status,
                    Timestamp = a.Timestamp
                }).ToList();
        }

        [HttpPost, Route("")]
        public string Post(StoreRequestDto request)
        {
            Guid idCreate = Guid.Parse(request.StoreOwnerId);

            Store store = new Store();
            store.Address = request.Address;
            store.Description = request.Description;
            store.Name = request.Name;
            store.ProfilePic = "default.png";
            store.Template = "businessr";
            store.StoreOwnerId = idCreate;
            store.ZipCode = request.ZipCode;
            db.Stores.Add(store);
            db.SaveChanges();

            return store.Id.ToString();

        }

        [HttpPut, Route("")]
        public string Put(StoreRequestDto request)
        {
            Guid idCompare = Guid.Parse(request.Id);

            Store store = db.Stores
                 .Where(a => a.Id == idCompare)
                 .FirstOrDefault();

            store.Address = request.Address;
            store.Description = request.Description;
            store.Name = request.Name;
            store.ZipCode = request.ZipCode;
            db.SaveChanges();

            return store.Id.ToString();

        }

        [HttpPut, Route("ChangeTemplate")]
        public string ChangeTemplate(StoreRequestDto request)
        {
            Guid idCompare = Guid.Parse(request.Id);

            Store store = db.Stores
                 .Where(a => a.Id == idCompare)
                 .FirstOrDefault();

            store.Template = request.Template;
            db.SaveChanges();

            return store.Id.ToString();

        }

        [HttpPut, Route("ChangeProfilePic")]
        public string ChangeProfilePic(StoreUploadThumbnailRequestDto request)
        {
            var filePath = this.CreateFolderIfNeeded(HttpContext.Current.Server.MapPath("~/Content/Images/Stores"));
            var fileName = request.Id + ".jpg";

            CleanFiles(request.Id.ToString());

            var bytes = Convert.FromBase64String(request.Image);

            #region Image to File
            //System.Threading.Tasks.Task.Run(() =>
            //{
            using (var outStream = new MemoryStream())
            {
                using (var inStream = new MemoryStream(bytes))
                {
                    var settings = new ResizeSettings("maxwidth=350&maxheight=800");
                    ImageResizer.ImageBuilder.Current.Build(inStream, outStream, settings, true);
                    var bytesThumb = outStream.ToArray();

                    using (var imageFile = new FileStream(filePath, FileMode.Create)) // thumb
                    {
                        imageFile.Write(bytesThumb, 0, bytesThumb.Length);
                        imageFile.Flush();
                    }
                }
            }
            //});
            #endregion

            Guid idCompare = Guid.Parse(request.Id);

            Store store = db.Stores
                 .Where(a => a.Id == idCompare)
                 .FirstOrDefault();

            store.ProfilePic = fileName;
            db.SaveChanges();

            return store.Id.ToString();
        }

    }
}
