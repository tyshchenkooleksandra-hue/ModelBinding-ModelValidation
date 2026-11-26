using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ProductsValidation.Models;
using ProductsValidation.Services;

namespace ProductsValidation.Controllers
{
    public class ProductsController : Controller
    {
        private readonly List<Product> myProducts;

        public ProductsController(Data data)
        {
            myProducts = data.Products;
        }

        public IActionResult Index()
        {
            return View(myProducts);
        }

        public IActionResult View(int id)
        {
            Product prod = myProducts.Find(p => p.Id == id);
            if (prod != null)
            {
                return View(prod);
            }

            return View("NotExists");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Product prod = myProducts.Find(p => p.Id == id);
            if (prod != null)
            {
                return View(prod);
            }

            return View("NotExists");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product product)
        {
            int index = myProducts.FindIndex(p => p.Id == product.Id);
            if (index < 0)
            {
                return View("NotExists");
            }

            if (!ModelState.IsValid)
            {
                return View(product);
            }

            myProducts[index] = product;
            return View("View", product);
        }

        [HttpGet]
        public IActionResult Create()
        {
            int nextId = myProducts.Count == 0 ? 1 : myProducts.Max(p => p.Id) + 1;
            return View(new Product() { Id = nextId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {
            if (myProducts.Any(p => p.Id == product.Id))
            {
                ModelState.AddModelError(nameof(Product.Id), "Product with this Id already exists");
            }

            if (!ModelState.IsValid)
            {
                return View(product);
            }

            myProducts.Add(product);
            return View("View", product);
        }

        [HttpGet]
        public IActionResult EditPrices(Product.Category type)
        {
            ViewBag.Category = type;
            List<ProductPrice> prices = myProducts
                .Where(p => p.Type == type)
                .Select(p => new ProductPrice { Id = p.Id, Name = p.Name, Price = p.Price })
                .ToList();
            return View(prices);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditPrices(Product.Category type, List<ProductPrice> prices)
        {
            // Id and Name are read-only: take them from storage, not from the form.
            foreach (ProductPrice item in prices)
            {
                Product prod = myProducts.Find(p => p.Id == item.Id && p.Type == type);
                item.Name = prod?.Name;
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Category = type;
                return View(prices);
            }

            foreach (ProductPrice item in prices)
            {
                Product prod = myProducts.Find(p => p.Id == item.Id && p.Type == type);
                if (prod != null)
                {
                    prod.Price = item.Price;
                }
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            myProducts.Remove(myProducts.Find(product => product.Id == id));
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
