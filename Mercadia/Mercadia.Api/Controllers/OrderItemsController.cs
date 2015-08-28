using Mercadia.Infrastructure.DTO.OrderItems;
using Mercadia.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Mercadia.Api.Models;
using Mercadia.Infrastructure.Models;
using Mercadia.Infrastructure.Enums;
using Mercadia.Infrastructure.DTO;
using Mercadia.Infrastructure.DTO.Users;
using Mercadia.Api.Mailers;
using Mercadia.Api.Helpers;
using System.Net.Http.Formatting;

namespace Mercadia.Api.Controllers
{
    [RoutePrefix("api/orderitems")]
    public class OrderItemsController : BaseApiController
    {

        [HttpGet, Route("{id}")]
        public List<OrderItemResponseDto> GetOrderItems(string id)
        {
            DateTime dateCompare = DateTime.Now;
            Guid idCompare = Guid.Parse(id);

            return db.OrderItems
                .Where(a => a.StoreId == idCompare
                    && a.Timestamp.Year == dateCompare.Year
                    && a.Timestamp.Month == dateCompare.Month
                    && a.Timestamp.Day == dateCompare.Day)
                .Select(a => new OrderItemResponseDto()
                {
                    ProductId = a.ProductId.ToString(),
                    ProductName = a.ProductName,
                    Quantity = a.Quantity,
                    UnitPrice = a.UnitPrice,
                    ItemPrice = a.ItemPrice
                }).ToList();
        }

        [HttpPost, Route("")]
        public string SaveOrderItems(List<OrderItemRequestDto> request)
        {

            foreach (OrderItemRequestDto item in request)
            {
                Guid guidStoreId = Guid.Parse(item.StoreId);
                Guid guidOwnerId = Guid.Parse(item.StoreOwnerId);
                Guid guidUserId = Guid.Parse(item.UserId);
                Guid guidOrderId = Guid.Parse(item.OrderId);
                Guid guidProductId = Guid.Parse(item.ProductId);

                OrderItem orderItem = new OrderItem();
                orderItem.ItemPrice = item.ItemPrice;
                orderItem.OrderDate = item.OrderDate;
                orderItem.OrderId = guidOrderId;
                orderItem.ProductId = guidProductId;
                orderItem.StoreId = guidStoreId;
                orderItem.StoreName = item.StoreName;
                orderItem.StoreOwnerId = guidOwnerId;
                orderItem.StoreOwnerName = item.StoreOwnerName;
                orderItem.UserId = guidUserId;
                orderItem.UserName = item.UserName;
                orderItem.ProductName = item.ProductName;
                orderItem.Quantity = item.Quantity;
                orderItem.UnitPrice = item.UnitPrice;

                db.OrderItems.Add(orderItem);
                db.SaveChanges();
            }

            return request[0].OrderId.ToString();
        }
    }
}
