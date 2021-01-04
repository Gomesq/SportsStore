using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Controllers
{
    public class OrderController : Controller
    {
        private IOrderRepository _repo;
        private Cart _cart;

        public OrderController(IOrderRepository repo, Cart cart)
        {
            _repo = repo;
            _cart = cart;
        }

        [Authorize]
        public ViewResult List() =>
            View(_repo.Orders.Where(x => !x.Shipped));

        [HttpPost]
        [Authorize]
        public IActionResult MarkShipped(int orderID)
        {
            Order order = _repo.Orders.FirstOrDefault(x => x.OrderID == orderID);
            if (order != null)
            {
                order.Shipped = true;
                _repo.SaveOrder(order);
            }
            return RedirectToAction(nameof(List));
        }

        public ViewResult Checkout() => View(new Order());

        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            if (_cart.Lines.Count() == 0)
                ModelState.AddModelError("", "Your cart is empty!");
            if (ModelState.IsValid)
            {
                order.Lines = _cart.Lines.ToArray();
                _repo.SaveOrder(order);
                return RedirectToAction(nameof(Completed));
            }
            else
                return View(order);
        }
        public ViewResult Completed()
        {
            _cart.Clear();
            return View();
        }
    }
}
