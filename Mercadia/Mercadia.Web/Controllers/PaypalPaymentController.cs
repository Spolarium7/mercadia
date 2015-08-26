using Mercadia.Infrastructure.DTO.Stores;
using Mercadia.Web.Securities;
using Mercadia.Web.ViewModels.Stores;
using Moolah;
using Moolah.PayPal;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mercadia.Web.Controllers
{
    public class PaypalPaymentController : BaseController
    {
        [AllowAnonymous]
        public ActionResult CreatePayment()
        {
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


        public ActionResult Confirm()
        {
            return View();
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