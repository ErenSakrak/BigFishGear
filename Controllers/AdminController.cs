using Microsoft.AspNetCore.Mvc;
using bigSales.Models;
using bigSales.Services;
using Microsoft.AspNetCore.Authorization;
using bigSales.Data;

namespace bigSales.Controllers
{
    [Authorize(Roles = "1")] 
    public class AdminController : Controller
    {
        private readonly ProductService _productService;
        private readonly CategoryService _categoryService;
        private readonly ApplicationDbContext _context;

        public AdminController(ProductService productService, CategoryService categoryService, ApplicationDbContext context)
        {
            _productService = productService;
            _categoryService = categoryService;
            _context = context;
        }

        public IActionResult Index()
        {
            var products = _productService.GetAllProducts();

            return View(products);
        }

        public IActionResult List()
        {
            var products = _productService.GetAllProductsWithCategory();
            return View(products);
        }


        public IActionResult Details(int id)
        {
            var product = _productService.GetProductWithCategoryById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        public IActionResult Create()
        {
            var categories = _categoryService.GetAllCategories(); 
            ViewBag.Categories = categories;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product, IFormFile Image)
        {
            if (!ModelState.IsValid)
            {
                
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Hata: {error.ErrorMessage}");
                }

                ViewBag.Categories = _categoryService.GetAllCategories(); 
                return View(product); 
            }

            
            if (Image != null && Image.Length > 0)
            {
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + Image.FileName;
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await Image.CopyToAsync(fileStream);
                }

                product.ImagePath = "/images/" + uniqueFileName;
            }

            try
            {
                _productService.AddProduct(product);  
                return RedirectToAction("List", "Admin");  
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during product creation: {ex.Message}");
                ViewBag.Categories = _categoryService.GetAllCategories();
                return View(product);  
            }
        }



        public IActionResult Edit(int id)
        {
            var product = _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }

            ViewBag.Categories = _categoryService.GetAllCategories();
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product product, IFormFile Image)
        {
            if (!ModelState.IsValid)
            {
                
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Hata: {error.ErrorMessage}");
                }

                ViewBag.Categories = _categoryService.GetAllCategories(); 
                return View(product); 
            }

            try
            {
                if (Image != null && Image.Length > 0)
                {
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + Image.FileName;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await Image.CopyToAsync(fileStream);
                    }

                    product.ImagePath = "/images/" + uniqueFileName;
                }
                else
                {
                    
                    var existingProduct = _productService.GetProductById(product.Id);
                    product.ImagePath = existingProduct?.ImagePath;
                }

                _productService.UpdateProduct(product); 

                TempData["SuccessMessage"] = "Ürün başarıyla güncellendi!"; 
                return RedirectToAction("List", "Admin"); 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during product update: {ex.Message}");
                ViewBag.Categories = _categoryService.GetAllCategories();
                return View(product); 
            }
        }


        public IActionResult Delete(int id)
        {
            var product = _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            _productService.DeleteProduct(id);
            return RedirectToAction("List", "Admin");
        }

    }
}
