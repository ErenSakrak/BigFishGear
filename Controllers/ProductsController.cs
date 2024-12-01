using Microsoft.AspNetCore.Mvc;
using bigSales.Models;
using bigSales.Services;
using Microsoft.AspNetCore.Authorization;

namespace bigSales.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ProductService _productService;

        public ProductsController(ProductService productService)
        {
            _productService = productService;
        }

        public IActionResult ProductsList()
        {
            var products = _productService.GetAllProducts();
            return View(products);
        }

        public IActionResult AllProducts()
        {
            var products = _productService.GetAllProducts();
            return View(products);
        }

        public IActionResult Details()
        {
            return View();
        }


    }
}
