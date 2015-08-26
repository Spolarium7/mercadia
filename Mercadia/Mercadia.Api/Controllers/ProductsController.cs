using ImageResizer;
using Mercadia.Infrastructure.DTO;
using Mercadia.Infrastructure.DTO.Products;
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
    [RoutePrefix("api/products")]
    public class ProductsController : BaseApiController
    {
        [HttpGet, Route("{store}/{category}")]
        public List<ProductResponseDto> List(string store, string category)
        {
            Guid storeId = Guid.Parse(store);
            Guid categoryId = Guid.Parse(category);

            var path = Url.Content("/productimages/render?file=");

            return db.Products
                .Where(a => a.StoreId == storeId
                        && a.CategoryId == categoryId)
                .Select(a => new ProductResponseDto()
                {
                    Name = a.Name,
                    Description = a.Description,
                    Id = a.Id.ToString(),
                    CategoryId = a.CategoryId,
                    ProfilePic = path + a.ProfilePic,
                    Sku= a.Sku,
                    Price = a.Price,
                    Status = a.Status,
                    StoreId = a.StoreId
                }).ToList();

        }

        [HttpGet, Route("{id}")]
        public ProductResponseDto ById(string id)
        {
            Guid idCompare = Guid.Parse(id);

            var path = Url.Content("/productimages/render?file=");

            return db.Products
                .Where(a => a.Id == idCompare)
                .Select(a => new ProductResponseDto()
                {
                    Name = a.Name,
                    Description = a.Description,
                    Id = a.Id.ToString(),
                    CategoryId = a.CategoryId,
                    ProfilePic = path + a.ProfilePic,
                    Sku = a.Sku,
                    Price = a.Price,
                    Status = a.Status,
                    StoreId = a.StoreId
                }).FirstOrDefault();
        }

        [HttpPost, Route("")]
        public string Post(ProductRequestDto request)
        {
            Guid storeId = Guid.Parse(request.StoreId);
            Guid categoryId = Guid.Parse(request.CategoryId);

            if (db.Products
                .Where(a => a.StoreId == storeId
                        && a.Name.ToLower() == request.Name.ToLower()).Count() > 0)
            {
                ThrowError("Product already in use");
                return null;
            }


            Product product = new Product();
            product.StoreId = storeId;
            product.Description = request.Description;
            product.Name = request.Name;
            product.CategoryId = categoryId;
            product.Sku = request.Sku;
            product.Price = request.Price;
            product.ProfilePic = "default.png";

            db.Products.Add(product);
            db.SaveChanges();

            return product.Id.ToString();

        }

        [HttpPut, Route("")]
        public string Put(ProductRequestDto request)
        {
            Guid idCompare = Guid.Parse(request.Id);
            Guid storeId = Guid.Parse(request.StoreId);
            Guid categoryId = Guid.Parse(request.CategoryId);

            if (db.Categories
                .Where(a => a.StoreId == storeId
                && a.Name.ToLower() == request.Name.ToLower()).Count() > 0)
            {
                ThrowError("Product already in use");
                return null;
            }

            Product product = db.Products
                 .Where(a => a.Id == idCompare)
                 .FirstOrDefault();

            product.StoreId = storeId;
            product.Description = request.Description;
            product.Name = request.Name;
            product.CategoryId = categoryId;
            product.Sku = request.Sku;
            product.Price = request.Price;
            db.SaveChanges();

            return product.Id.ToString();

        }

        [HttpPut, Route("changestatus")]
        public string Put(ChangeStatusDto request)
        {
            Guid idCompare = Guid.Parse(request.Id);

            Product product = db.Products
                 .Where(a => a.Id == idCompare)
                 .FirstOrDefault();

            product.Status = request.Status;
            db.SaveChanges();

            return product.Id.ToString();

        }

        [HttpPut, Route("ChangeProfilePic")]
        public string ChangeProfilePic(ProductUploadThumbnailRequestDto request)
        {
            var appDataPath = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();
            var path = Path.Combine(appDataPath, "Images/Products");

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            var filename = string.Format("{0}.jpg", Guid.NewGuid().ToString().Replace("-", ""));

            /* {company-id}/{user-id}/{filename} */
            var filePath = Path.Combine(path, filename);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

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

            Product product = db.Products
                 .Where(a => a.Id == idCompare)
                 .FirstOrDefault();

            product.ProfilePic = filename;
            db.SaveChanges();

            return product.Id.ToString();
        }
    }
}
