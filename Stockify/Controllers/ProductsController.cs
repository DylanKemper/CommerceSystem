using Microsoft.AspNetCore.Mvc;
using Stockify.Models;
using Stockify.Services;

namespace Stockify.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        // GET /Products
        [HttpGet]
        public IActionResult Index()
        {
            return View(_productService.GetAllProducts());
        }

        // GET /Products/Edit/5
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var product = _productService.GetProductById(id);
            if (product is null) return NotFound();
            return View(product);
        }

        // POST /Products/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product product)
        {
            if (!ModelState.IsValid) return View(product);

            if (!_productService.UpdateProduct(product)) return NotFound();

            TempData["Message"] = $"{product.Name} updated.";
            return RedirectToAction(nameof(Index));
        }

        // POST /Products/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var product = _productService.GetProductById(id);
            if (product is null) return NotFound();

            _productService.DeleteProduct(id);

            TempData["Message"] = $"{product.Name} deleted.";
            return RedirectToAction(nameof(Index));
        }

        // GET /Products/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View(new Product());
        }

        // POST /Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {
            if (!ModelState.IsValid) return View(product);

            _productService.Add(product);

            TempData["Message"] = $"{product.Name} added.";
            return RedirectToAction(nameof(Index));
        }
    }
}