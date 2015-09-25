﻿using Mercadia.Infrastructure.DTO.OrderItems;
using Mercadia.Infrastructure.DTO.Orders;
using Mercadia.Infrastructure.DTO.Stores;
using Mercadia.Infrastructure.DTO.Users;
using Mercadia.Store.Securities;
using Mercadia.Store.ViewModels.Stores;
using Moolah;
using Moolah.PayPal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Mercadia.Store.Controllers
{
    public class PaypalPaymentController : BaseController
    {
        public void SaveSession()
        {
            HttpCookie userCookie = new HttpCookie("userCookie");
            HttpCookie storeCookie = new HttpCookie("storeCookie");
            HttpCookie categoryCookie = new HttpCookie("categoryCookie");
            HttpCookie cartCookie = new HttpCookie("cartCookie");

            DateTime now = DateTime.Now;

            // Set the cookie value.
            userCookie.Value = JsonConvert.SerializeObject(WebUser.CurrentUser);
            storeCookie.Value = JsonConvert.SerializeObject(WebUser.CurrentStore);
            categoryCookie.Value = JsonConvert.SerializeObject(WebUser.CurrentCategory);
            cartCookie.Value = JsonConvert.SerializeObject(WebUser.ShoppingCart);

            // Set the cookie expiration date.
            userCookie.Expires = now.AddHours(1);
            storeCookie.Expires = now.AddHours(1);
            categoryCookie.Expires = now.AddHours(1);
            cartCookie.Expires = now.AddHours(1);


            userCookie.Name = "userCookie";
            storeCookie.Name = "storeCookie";
            categoryCookie.Name = "categoryCookie";
            cartCookie.Name = "cartCookie";

            Response.Cookies.Add(userCookie);
            Response.Cookies.Add(storeCookie);
            Response.Cookies.Add(categoryCookie);
            Response.Cookies.Add(cartCookie);
        }

        public void RecoverSession()
        {
            if (WebUser.CurrentUser == null) { 
                WebUser.CurrentUser = JsonConvert.DeserializeObject<UserResponseDto>(Request.Cookies["userCookie"].ToString());
            }
            if (WebUser.CurrentCategory == null)
            {
                WebUser.CurrentCategory = JsonConvert.DeserializeObject<string>(Request.Cookies["categoryCookie"].ToString());
            }
            if (WebUser.CurrentStore == null)
            {
                WebUser.CurrentStore = JsonConvert.DeserializeObject<StoreResponseDto>(Request.Cookies["storeCookie"].ToString());
            }
            if (WebUser.ShoppingCart == null)
            {
                WebUser.ShoppingCart = JsonConvert.DeserializeObject<List<CartItemViewModel>>(Request.Cookies["cartCookie"].ToString());
            }
        }

        [HttpGet, AllowAnonymous]
        public ActionResult CreatePayment()
        {

            SaveSession();
            var settings = Get<PaypalAccountSettingsDto>("storesettings//paypal//" + WebUser.CurrentStore.Id);

            // TODO - Store Settings
            //var configuration   = new PayPalConfiguration(
            //                            environment:    paymentEnvironment, 
            //                            userId:         "lbarlin-facilitator_api1.outlook.com", 
            //                            password:       "1404227915", 
            //                            signature:      "ASpmyWnhstQZf4wFrRvt0Q0NP.ynANerA6E9OMmsrZtXvPXe7LFMO2iw");
            var configuration = new PayPalConfiguration(environment: PaymentEnvironment.Test, userId: settings.Data.PaypalFacilitator, password: settings.Data.PaypalPassword, signature: settings.Data.PaypalSignature);

            var gateway = new PayPalExpressCheckout(configuration);

            // return to Checkout
            // {domain}/store/{storeUrl}/checkout
            var cancelUrl = string.Format("{0}store/shoppingcart", ConfigurationManager.AppSettings["Domain"].ToString().ToLower());

            // return to Paypal confirmation
            // {domain}/payments/paypal/confirmation
            var confirmationUrl = string.Format("{0}paypalpayment/confirm", ConfigurationManager.AppSettings["Domain"].ToString().ToLower());

            var order = this.GetOrder();

            if (order == null)
                return RedirectPermanent(string.Format("{0}store/shoppingcart", ConfigurationManager.AppSettings["Domain"].ToString().ToLower()));

            var response = gateway.SetExpressCheckout(order, cancelUrl, confirmationUrl);

            if (response.Status == PaymentStatus.Failed)
                return RedirectPermanent(string.Format("{0}store/shoppingcart", ConfigurationManager.AppSettings["Domain"].ToString().ToLower()));

            return RedirectPermanent(response.RedirectUrl);
        }

        [HttpGet, AllowAnonymous]
        public ActionResult Confirm(string token, string PayerID)
        {
            RecoverSession();
            ViewBag.Token = token;
            ViewBag.OrderId = FinalizeTransaction(token);
            return View(string.Format("../{0}/confirm", WebUser.CurrentStore.Template));
        }


        public string FinalizeTransaction(string token)
        {
            decimal totalPrice = 0;
            foreach (CartItemViewModel cartItem in WebUser.ShoppingCart)
            {
                totalPrice = totalPrice + (cartItem.Price * cartItem.Quantity);
            }

            var orderId = Post<string>("orders", new OrderRequestDto
            {
                PayDate = DateTime.Now,
                PaymentDetails = "Token: " + token,
                PaymentReference = token,
                PaymentStatus = Infrastructure.Enums.PaymentStatus.Paid,
                StoreId = WebUser.CurrentStore.Id.ToString(),
                StoreName = WebUser.CurrentStore.Name,
                StoreOwnerId = WebUser.CurrentStore.StoreOwnerId.ToString(),
                TaxDue = 0,
                TotalDiscount = 0,
                TotalPrice = totalPrice,
                UserId = WebUser.CurrentUser.Id.ToString(),
                UserName = WebUser.CurrentUser.FirstName + " " + WebUser.CurrentUser.LastName
            });

            /* Test RESULTS - OKAY */
            if (orderId.Status == HttpStatusCode.OK)
            {
                SaveOrderItems(orderId.Data);
                return orderId.Data;
            }
            /* Test RESULTS - Api Validation Error */
            else if (orderId.Status == HttpStatusCode.BadRequest)
            {
                this.ModelState.AddModelError("", orderId.Message);
                return "";
            }
            return "";
        }

        private void SaveOrderItems(string orderId)
        {
            List<OrderItemRequestDto> items = new List<OrderItemRequestDto>();
            foreach (CartItemViewModel cartItem in WebUser.ShoppingCart)
            {
                OrderItemRequestDto item = new OrderItemRequestDto();
                item.ItemPrice = cartItem.Quantity * cartItem.Price;
                item.OrderDate = DateTime.Now;
                item.OrderId = orderId.ToString();
                item.ProductId = cartItem.ProductId;
                item.ProductName = cartItem.Name;
                item.Quantity = cartItem.Quantity;
                item.StoreId = WebUser.CurrentStore.Id.ToString();
                item.StoreName = WebUser.CurrentStore.Name;
                item.StoreOwnerId = WebUser.CurrentStore.StoreOwnerId;
                item.UnitPrice = cartItem.Price;
                item.UserId = WebUser.CurrentUser.Id.ToString();
                item.UserName = WebUser.CurrentUser.FirstName + " " + WebUser.CurrentUser.LastName;
                items.Add(item);
            };

            var tranId = Post<string>("orderitems", items);

            /* Test RESULTS - OKAY */
            if (tranId.Status == HttpStatusCode.OK)
            {

            }
            /* Test RESULTS - Api Validation Error */
            else if (tranId.Status == HttpStatusCode.BadRequest)
            {
                this.ModelState.AddModelError("", tranId.Message);
            }
        }

        private OrderDetails GetOrder()
        {
            OrderDetails paypalOrder = null;

            if (WebUser.ShoppingCart != null)
            {
                var order = WebUser.ShoppingCart;
                paypalOrder = new OrderDetails();
                decimal total = 0;
                var ctr = 1;
                foreach (CartItemViewModel item in order)
                {
                    item.Number = ctr;
                    ctr = ctr + 1;
                    total = total + (item.Price * item.Quantity);
                }


                paypalOrder.Items = WebUser.ShoppingCart
                    .Select(a => new OrderDetailsItem()
                    {
                        Description = a.Name,
                        Name = a.Name,
                        ItemUrl = a.Name,
                        Number = a.Number,
                        Quantity = a.Quantity,
                        Tax = 0,
                        UnitPrice = a.Price
                    });

                paypalOrder.OrderDescription = "";

                paypalOrder.OrderTotal = total;

                paypalOrder.ShippingDiscount = 0;

                paypalOrder.ShippingTotal = 0;

                paypalOrder.TaxTotal = 0;

                // paypalOrder.Discounts = order.Discounts
                //   .Select(a => new DiscountDetails()
                //   {
                //       Amount = a.Amount,
                //       Description = a.Description,
                //       Quantity = a.Quantity,
                //       Tax = a.Tax
                //    });

                paypalOrder.CurrencyCodeType = CurrencyCodeType.PHP;
            }

            return paypalOrder;
        }
    }
}