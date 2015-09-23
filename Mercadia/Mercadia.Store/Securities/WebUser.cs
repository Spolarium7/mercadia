using Mercadia.Infrastructure.DTO.Stores;
using Mercadia.Infrastructure.DTO.Users;
using Mercadia.Store.ViewModels.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mercadia.Store.Securities
{
    public static class WebUser
    {
        public static UserResponseDto CurrentUser
        {
            get
            {
                if (HttpContext.Current.Session != null && HttpContext.Current.Session["CurrentUser"] != null)
                {
                    return (UserResponseDto)HttpContext.Current.Session["CurrentUser"];
                }

                return null;
            }
            set { HttpContext.Current.Session["CurrentUser"] = value; }
        }

        public static List<StoreResponseDto> UserStores
        {
            get
            {
                if (HttpContext.Current.Session != null && HttpContext.Current.Session["UserStores"] != null)
                {
                    return (List<StoreResponseDto>)HttpContext.Current.Session["UserStores"];
                }

                return null;
            }
            set { HttpContext.Current.Session["UserStores"] = value; }
        }

        public static StoreResponseDto CurrentStore
        {
            get
            {
                if (HttpContext.Current.Session != null && HttpContext.Current.Session["CurrentStore"] != null)
                {
                    return (StoreResponseDto)HttpContext.Current.Session["CurrentStore"];
                }

                return null;
            }
            set { HttpContext.Current.Session["CurrentStore"] = value; }
        }

        public static string CurrentCategory{
            get
            {
                if (HttpContext.Current.Session != null && HttpContext.Current.Session["CurrentCategory"] != null)
                {
                    return (string)HttpContext.Current.Session["CurrentCategory"];
                }

                return null;
            }
            set { HttpContext.Current.Session["CurrentCategory"] = value; }
        }

        public static List<CartItemViewModel> ShoppingCart
        {
            get
            {
                if (HttpContext.Current.Session != null && HttpContext.Current.Session["ShoppingCart"] != null)
                {
                    return (List<CartItemViewModel>)HttpContext.Current.Session["ShoppingCart"];
                }

                return null;
            }
            set { HttpContext.Current.Session["ShoppingCart"] = value; }
        }

        public static string RedirectAction
        {
            get
            {
                if (HttpContext.Current.Session != null && HttpContext.Current.Session["RedirectAction"] != null)
                {
                    return (string)HttpContext.Current.Session["RedirectAction"];
                }

                return null;
            }
            set { HttpContext.Current.Session["RedirectAction"] = value; }
        }

        public static string RedirectController
        {
            get
            {
                if (HttpContext.Current.Session != null && HttpContext.Current.Session["RedirectController"] != null)
                {
                    return (string)HttpContext.Current.Session["RedirectController"];
                }

                return null;
            }
            set { HttpContext.Current.Session["RedirectController"] = value; }
        }
    }
}