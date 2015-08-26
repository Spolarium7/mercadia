using Mercadia.Infrastructure.DTO.OrderItems;
using Mercadia.Infrastructure.DTO.Orders;
using Mercadia.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mercadia.Api.Controllers
{
    [RoutePrefix("api/orders")]
    public class OrdersController : BaseApiController
    {
        [HttpGet, Route("bystore/{id}")]
        public List<OrderResponseDto> ListByStore(string id, string date)
        {
            DateTime dateCompare = DateTime.Parse(date);
            Guid idCompare = Guid.Parse(id);

            return db.Orders
                .Where(a => a.StoreId == idCompare
                    && a.Timestamp.Year == dateCompare.Year
                    && a.Timestamp.Month == dateCompare.Month
                    && a.Timestamp.Day == dateCompare.Day
                    && a.PaymentStatus == Infrastructure.Enums.PaymentStatus.Paid
                )
                .Select(a => new OrderResponseDto()
                {
                    StoreName = a.StoreName,
                    StoreId = a.StoreId.ToString(),
                    UserId = a.UserId.ToString(),
                    UserName = a.UserName,
                    TotalPrice = a.TotalPrice,
                    PayDate = a.PayDate,
                    PaymentDetails = a.PaymentDetails,
                    PaymentReference = a.PaymentReference,                    
                }).ToList();
        }

        [HttpPost, Route("")]
        public string Post(OrderRequestDto request)
        {
            Guid guidStoreId = Guid.Parse(request.StoreId);
            Guid guidOwnerId = Guid.Parse(request.StoreOwnerId);
            Guid guidUserId = Guid.Parse(request.UserId);

            Order order = new Order();
            order.PayDate = request.PayDate;
            order.PaymentDetails = request.PaymentDetails;
            order.PaymentReference = request.PaymentReference;
            order.PaymentStatus = request.PaymentStatus;
            order.StoreId = guidStoreId;
            order.StoreName = request.StoreName;
            order.StoreOwnerId = guidOwnerId;
            order.StoreOwnerName = request.StoreOwnerName;
            order.TaxDue = request.TaxDue;
            order.TotalDiscount = request.TotalDiscount;
            order.TotalPrice = request.TotalPrice;
            order.UserId = guidUserId;
            order.UserName = request.UserName;

            db.Orders.Add(order);
            db.SaveChanges();

            return order.Id.ToString();

        }

        [HttpGet, Route("items/{id}")]
        public List<OrderItemResponseDto> GetOrderItems(string id)
        {
            Guid guidOrderId = Guid.Parse(id);
            return db.OrderItems
                .Where(a => a.OrderId == guidOrderId)
                .Select(a => new OrderItemResponseDto()
                {
                    ProductId = a.ProductId.ToString(),
                    ProductName = a.ProductName,
                    Quantity = a.Quantity,
                    UnitPrice = a.UnitPrice,
                    ItemPrice = a.ItemPrice
                }).ToList();
        }

        [HttpPost, Route("items")]
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