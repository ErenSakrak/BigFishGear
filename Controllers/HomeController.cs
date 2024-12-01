using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using bigSales.Models;
using bigSales.Services;

namespace bigSales.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ProductService _productService;
        


        public HomeController(ILogger<HomeController> logger, ProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        public IActionResult Index()
        {

            var products = _productService.GetAllProducts();
            return View(products);
        }

        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
