using Mercadia.Infrastructure.DTO.Categories;
using Mercadia.Infrastructure.DTO.Stores;
using Mercadia.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Mercadia.Api.Controllers
{
    [RoutePrefix("api/categories")]
    public class CategoriesController : BaseApiController
    {
        [HttpGet, Route("bystore/{id}")]
        public List<CategoryResponseDto> ListByStore(string id)
        {
            Guid idCompare = Guid.Parse(id);

            return db.Categories
                .Where(a => a.StoreId == idCompare)
                .Select(a => new CategoryResponseDto()
                {
                    Name = a.Name,
                    Description = a.Description,
                    Id = a.Id.ToString(),
                    ParentId = a.ParentId,
                    StoreId = a.StoreId
                }).ToList();
        }

        [HttpGet, Route("{id}")]
        public CategoryResponseDto ById(string id)
        {
            Guid idCompare = Guid.Parse(id);

            return db.Categories
                .Where(a => a.Id == idCompare)
                .Select(a => new CategoryResponseDto()
                {
                    Name = a.Name,
                    Description = a.Description,
                    Id = a.Id.ToString(),
                    ParentId = a.ParentId,
                    StoreId = a.StoreId
                }).FirstOrDefault();
        }

        //CategoryNameIdPairDto
        [HttpGet, Route("get-details/{id}")]
        public CategoryNameIdPairDto GetDetails(string id)
        {
            Guid idCompare = Guid.Parse(id);

            CategoryNameIdPairDto nameIdPair = new CategoryNameIdPairDto();

            var Category = db.Categories
                .Where(a => a.Id == idCompare)
                .Select(a => new CategoryResponseDto()
                {
                    Id = a.Id.ToString(),
                    Name = a.Name,
                    StoreId = a.StoreId
                }).FirstOrDefault();

            if (Category != null) {
                nameIdPair.CategoryId = Category.Id;
                nameIdPair.CategoryName = Category.Name;

                var Store = db.Stores
                    .Where(a => a.Id == Category.StoreId)
                    .Select(a => new StoreResponseDto()
                    {
                        Id = a.Id,
                        Name = a.Name,
                    }).FirstOrDefault();

                if (Store != null) {
                    nameIdPair.StoreId = Store.Id.ToString();
                    nameIdPair.StoreName = Store.Name;
                }
            }

            return nameIdPair;
        }

        [HttpPost, Route("")]
        public string Post(CategoryRequestDto request)
        {
            Guid guidStoreId = Guid.Parse(request.StoreId);

            if(db.Categories
                .Where( a => a.StoreId == guidStoreId 
                        && a.Name.ToLower() == request.Name.ToLower()).Count() > 0)
            {
                ThrowError("Category name already in use");
                return null;
            }

            Category category = new Category();
            category.StoreId = guidStoreId;
            //category.ParentId = (request.ParentId != null ? Guid.Parse(request.ParentId) : null);
            category.Description = request.Description;
            category.Name = request.Name;


            db.Categories.Add(category);
            db.SaveChanges();

            return category.Id.ToString();

        }

        [HttpPut, Route("")]
        public string Put(CategoryRequestDto request)
        {
            Guid idCompare = Guid.Parse(request.Id);
            Guid guidStoreId = Guid.Parse(request.StoreId);

            if (db.Categories
                .Where(a => a.StoreId == guidStoreId
                && a.Name.ToLower() == request.Name.ToLower()).Count() > 0)
            {
                ThrowError("Category name already in use");
                return null;
            }

            Category category = db.Categories
                 .Where(a => a.Id == idCompare)
                 .FirstOrDefault();

            category.StoreId = guidStoreId;
            //category.ParentId = (request.ParentId != null ? Guid.Parse(request.ParentId) : null);
            category.Description = request.Description;
            category.Name = request.Name;
            db.SaveChanges();

            return category.Id.ToString();

        }
    }
}
