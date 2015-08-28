using Mercadia.Infrastructure.DTO.OrderItems;
using Mercadia.Infrastructure.DTO.Orders;
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
    [RoutePrefix("api/orders")]
    public class OrdersController : BaseApiController
    {
        [HttpGet, Route("bystore/{id}/")]
        public List<OrderResponseDto> ByStore(string id)
        {
            DateTime dateCompare = DateTime.Now;
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


    }
}