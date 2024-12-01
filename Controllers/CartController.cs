using Microsoft.AspNetCore.Mvc;
using bigSales.Models;
using bigSales.Services;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace bigSales.Controllers
{
    public class CartController : Controller
    {
        private readonly ProductService _productService;

        public CartController(ProductService productService)
        {
            _productService = productService;
        }

        private string GetCartSessionKey()
        {
            var username = User.Identity?.Name ?? "Guest";
            return $"Cart_{username}"; 
        }

        private List<Product> GetCartFromSession()
        {
            var cartJson = HttpContext.Session.GetString(GetCartSessionKey());
            if (string.IsNullOrEmpty(cartJson))
            {
                return new List<Product>();
            }

            var cartProducts = JsonSerializer.Deserialize<List<Product>>(cartJson);
            return cartProducts ?? new List<Product>(); 
        }

        private void SaveCartToSession(List<Product> cartProducts)
        {
            var cartJson = JsonSerializer.Serialize(cartProducts);
            HttpContext.Session.SetString(GetCartSessionKey(), cartJson);
        }

        public IActionResult Index()
        {
            var cartProducts = GetCartFromSession();
            return View(cartProducts);
        }

       
        public IActionResult Add(int productId)
        {
            var product = _productService.GetProductById(productId);
            if (product != null)
            {
                var cartProducts = GetCartFromSession();
                cartProducts.Add(product);
                SaveCartToSession(cartProducts);
            }
            return RedirectToAction("Index", "Cart"); 
        }

        public IActionResult Remove(int productId)
        {
            var cartProducts = GetCartFromSession();
            var product = cartProducts.Find(p => p.Id == productId);
            if (product != null)
            {
                cartProducts.Remove(product);
                SaveCartToSession(cartProducts);
            }
            return RedirectToAction("Index", "Cart");
        }
    }

}
