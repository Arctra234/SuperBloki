using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SuperBloki.Domain.Entities;
using SuperBloki.Domain.Abstract;
using SuperBloki.WebUI.Models;

namespace SuperBloki.WebUI.Controllers
{
    public class CartController : Controller
    {
        private IConstructorRepository repository;
        private IOrderProcessor orderProcessor;
        public CartController(IConstructorRepository repo, IOrderProcessor processor)
        {
            repository = repo;
            orderProcessor = processor;
        }
       
        public ViewResult Index(Cart cart, string returnUrl)
        {
            return View(new CartIndexViewModel
            {
                Cart = cart,
                ReturnUrl = returnUrl
            });
        }

        public RedirectToRouteResult AddToCart(Cart cart, int constructorID, string returnUrl)
        {
            Constructor constructor = repository.Constructors
                .FirstOrDefault(g => g.ConstructorID == constructorID);

            if (constructor != null)
            {
                cart.AddItem(constructor, 1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToRouteResult RemoveFromCart(Cart cart, int constructorID, string returnUrl)
        {
            Constructor constructor = repository.Constructors
                .FirstOrDefault(g => g.ConstructorID == constructorID);

            if (constructor != null)
            {
                cart.RemoveLine(constructor);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public PartialViewResult Summary(Cart cart)
        {
            return PartialView(cart);
        }

        public ViewResult Checkout()
        {
            return View(new ShippingDetails());
        }


        [HttpPost]
        public ViewResult Checkout(Cart cart, ShippingDetails shippingDetails)
        {
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Przepraszamy, twój koszyk jest pusty!");
            }

            if (ModelState.IsValid)
            {
                orderProcessor.ProcessOrder(cart, shippingDetails);
                cart.Clear();
                return View("Completed");
            }
            else
            {
                return View(shippingDetails);
            }
        }

    }
}