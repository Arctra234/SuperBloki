using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SuperBloki.Domain.Entities;
using System.Web.Mvc;

namespace SuperBloki.WebUI.Infrastructure.Binders
{
    public class CartModelBinder : IModelBinder
    {
        private const string sessionKey = "Cart";

        public object BindModel(ControllerContext controllerContext,
            ModelBindingContext bindingContext)
        {

            // Pobierz obiekt koszyka z sesji
            Cart cart = null;
            if (controllerContext.HttpContext.Session != null)
            {
                cart = (Cart)controllerContext.HttpContext.Session[sessionKey];
            }

            // Utwórz obiekt koszyka, jeśli nie zostanie znaleziony w sesji
            if (cart == null)
            {
                cart = new Cart();
                if (controllerContext.HttpContext.Session != null)
                {
                    controllerContext.HttpContext.Session[sessionKey] = cart;
                }
            }

            // Zwróć obiekt Cart
            return cart;
        }
    }
}