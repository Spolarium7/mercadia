using Mercadia.Infrastructure.DTO.Categories;
using Mercadia.Infrastructure.DTO.Products;
using Mercadia.Infrastructure.DTO.Stores;
using Mercadia.Store.Securities;
using Mercadia.Store.ViewModels;
using Mercadia.Store.ViewModels.Stores;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Mercadia.Store.Controllers
{
    [RoutePrefix("home")]
    public class HomeController : BaseController
    {
        private void ChooseCategory(string categoryId)
        {
            WebUser.CurrentCategory = categoryId;
        }

        private StoreResponseDto SelectStore(string store)
        {
            var response = Get<StoreResponseDto>("stores//findbyname//" + store);

            /* Test RESULTS - OKAY */
            if (response.Status == HttpStatusCode.OK)
            {
                WebUser.CurrentStore = response.Data;
                return response.Data;
            }
            /* Test RESULTS - Api Validation Error */
            else if (response.Status == HttpStatusCode.BadRequest)
            {
                this.ModelState.AddModelError("", response.Message);
                return null;
            }

            return null;
        }

        private List<SelCategoryResponseDto> FetchCategories(string storeId)
        {
            var response = Get<List<CategoryResponseDto>>("categories//bystore//" + storeId);

            /* Test RESULTS - OKAY */
            if (response.Status == HttpStatusCode.OK)
            {
                List<SelCategoryResponseDto> categories = new List<SelCategoryResponseDto>();

                foreach (CategoryResponseDto category in response.Data)
                {
                    var isSelected = false;
                    if (WebUser.CurrentCategory == category.Id.ToString() && WebUser.CurrentCategory != null)
                    {
                        isSelected = true;
                    }

                    SelCategoryResponseDto cat = new SelCategoryResponseDto();
                    cat.Description = category.Description;
                    cat.Id = category.Id;
                    cat.IsSelected = isSelected;
                    cat.Name = category.Name;

                    categories.Add(cat);

                }

                if (categories.Where(a => a.IsSelected == true).Count() < 1)
                {
                    categories.FirstOrDefault().IsSelected = true;
                }

                var selected = categories.Where(a => a.IsSelected == true).FirstOrDefault();
                ChooseCategory(selected.Id);


                return categories;

            }
            /* Test RESULTS - Api Validation Error */
            else if (response.Status == HttpStatusCode.BadRequest)
            {
                this.ModelState.AddModelError("", response.Message);
                return null;
            }

            return null;
        }

        private List<ProductResponseDto> FetchProducts(string storeId, string categoryId)
        {
            var response = Get<List<ProductResponseDto>>("products//" + storeId + "//" + categoryId);

            /* Test RESULTS - OKAY */
            if (response.Status == HttpStatusCode.OK)
            {
                return response.Data;
            }
            /* Test RESULTS - Api Validation Error */
            else if (response.Status == HttpStatusCode.BadRequest)
            {
                this.ModelState.AddModelError("", response.Message);
                return null;
            }

            return null;
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            string store = ConfigurationManager.AppSettings["Store"].ToString().ToLower().Replace(" ","_");

            if (store != null)
            {
                SelectStore(store);
            }

            StoreMenuViewModel viewModel = new StoreMenuViewModel();
            viewModel.Store = WebUser.CurrentStore;
            viewModel.Categories = FetchCategories(WebUser.CurrentStore.Id.ToString());
            viewModel.Products = FetchProducts(WebUser.CurrentStore.Id.ToString(), WebUser.CurrentCategory);
            return View(string.Format("../{0}/menu", WebUser.CurrentStore.Template), viewModel);
        }

        [AllowAnonymous]
        public ActionResult SelectCategory(string category)
        {
            ChooseCategory(category);
            return RedirectToAction("index", "store", new { store = WebUser.CurrentStore.Name.Replace(" ", "_") });
        }


        [HttpPost, AllowAnonymous]
        public JsonResult AddToCart(CartItemViewModel product)
        {
            List<CartItemViewModel> cartItems = new List<CartItemViewModel>();

            if (WebUser.ShoppingCart != null)
            {
                cartItems = WebUser.ShoppingCart;
            }

            CartItemViewModel cartItem = cartItems.Where(a => a.ProductId == product.ProductId).FirstOrDefault();


            if (cartItem == null)
            {
                cartItems.Add(product);
            }
            else
            {
                cartItems.Remove(cartItem);
                cartItem.Quantity = product.Quantity;
                cartItems.Add(cartItem);
            }



            WebUser.ShoppingCart = cartItems;

            return Json(product);
        }

        [HttpGet, AllowAnonymous]
        public JsonResult Product(string id)
        {
            var response = Get<ProductResponseDto>("products//" + id);

            /* Test RESULTS - OKAY */
            if (response.Status == HttpStatusCode.OK)
            {
                ProductRequestDto product = new ProductRequestDto();
                product.Name = response.Data.Name;
                product.StoreId = response.Data.StoreId.ToString();
                product.Description = response.Data.Description;
                product.Id = response.Data.Id.ToString();
                product.CategoryId = response.Data.CategoryId.ToString();
                product.Price = response.Data.Price;
                product.Sku = response.Data.Sku;
                product.ProfilePic = response.Data.ProfilePic;
                return Json(product, JsonRequestBehavior.AllowGet);
            }
            /* Test RESULTS - Api Validation Error */
            else if (response.Status == HttpStatusCode.BadRequest)
            {
                this.ModelState.AddModelError("", response.Message);
                return Json("", JsonRequestBehavior.AllowGet);
            }

            return Json("", JsonRequestBehavior.AllowGet);
        }

        [HttpGet, AllowAnonymous]
        public ActionResult ShoppingCart()
        {
            if (WebUser.CurrentUser == null)
            {
                WebUser.RedirectAction = "shoppingcart";
                WebUser.RedirectController = "store";
                return RedirectToAction("login", "storeuser", new { redirect = "store/shoppingcart" });
            }

            if (WebUser.ShoppingCart == null)
            {
                WebUser.ShoppingCart = new List<CartItemViewModel>();
            }

            return View(string.Format("../{0}/shoppingcart", WebUser.CurrentStore.Template));
        }

        public ActionResult BackToMenu()
        {
            WebUser.ShoppingCart = new List<CartItemViewModel>();
            WebUser.CurrentCategory = null;
            return RedirectToAction("index", "store", new { store = WebUser.CurrentStore.Name.Replace(" ", "_") });
        }

    }
}