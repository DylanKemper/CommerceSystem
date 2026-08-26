using Microsoft.AspNetCore.Mvc;
using Stockify.Services;
using Stockify.Models;

namespace Stockify.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public IActionResult Index()
        {
            return View(_customerService.GetAll());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Customer customer)
        {
            if (ModelState.IsValid)
            {
                _customerService.Add(customer);
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        public IActionResult Edit(int id)
        {
            var customer = _customerService.GetById(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        [HttpPost]
        public IActionResult Edit(int id, Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return View(customer);      // Return to the Edit view with the current customer data
            }
            _customerService.Update(customer);
            return RedirectToAction("Index");   // Redirect to the Index view after successful update
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _customerService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}