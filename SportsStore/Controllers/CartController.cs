using Microsoft.AspNetCore.Mvc;
using SportsStore.Infrastructure;
using SportsStore.Models;
using SportsStore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Controllers
{
    public class CartController : Controller
    {
        private IProductRepository _repo;
        private Cart _cart;
        public CartController(IProductRepository repo, Cart cartService)
        {
            _repo = repo;
            _cart = cartService;
        }
        public ViewResult Index(string returnUrl)
        {
            return View(new CartIndexViewModel
            {
                Cart = _cart,
                ReturnUrl = returnUrl
            });
        }
        public RedirectToActionResult AddToCart(int productId, string returnUrl)
        {
            Product product = _repo.Products
                .FirstOrDefault(x => x.ProductID == productId);
            if (product != null)
                _cart.AddItem(product, 1);

            return RedirectToAction("Index", new { returnUrl });
        }
        public RedirectToActionResult RemoveFromCart(int productId, string returnUrl)
        {
            Product product = _repo.Products
                .FirstOrDefault(x => x.ProductID == productId);
            if (product != null)
                _cart.RemoveLine(product);

            return RedirectToAction("Index", new { returnUrl });
        }
    }
}
