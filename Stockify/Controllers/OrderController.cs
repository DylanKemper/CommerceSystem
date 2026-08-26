using Microsoft.AspNetCore.Mvc;
using Stockify.Services;
using Stockify.Models;

namespace Stockify.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Order order)
        {
            if (ModelState.IsValid)
            {
                _orderService.Add(order);
                return RedirectToAction("Index");
            }
            return View(order);
        }


        public IActionResult Edit(int id)
        {
            var order = _orderService.GetById(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        [HttpPost]
        public IActionResult Edit(int id, Order order)
        {
            if (ModelState.IsValid)
            {
                _orderService.Update(order);
                return RedirectToAction("Index");
            }
            return View(order);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _orderService.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
