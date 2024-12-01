using bigSales.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace bigSales.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }


        public IActionResult Rods()
        {
            var products = _context.Products
                .Include(p => p.Category)
                .Where(p => p.CategoryId == 1)
                .ToList();

            return View(products);
        }


        public IActionResult Reels()
        {
            var products = _context.Products
                .Include(p => p.Category)
                .Where(p => p.CategoryId == 2)
                .ToList();

            return View(products);
        }


        public IActionResult Lures()
        {
            var products = _context.Products
                .Include(p => p.Category)
                .Where(p => p.CategoryId == 3)
                .ToList();

            return View(products);
        }
    }
}
