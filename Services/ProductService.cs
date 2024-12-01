using bigSales.Data;
using bigSales.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace bigSales.Services
{
    public class ProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public List<Product> GetAllProducts()
        {
            return _context.Products.ToList();
        }


        public List<Product> GetAllProductsWithCategory()
        {
            return _context.Products
                           .Include(p => p.Category) 
                           .ToList();
        }


        public Product? GetProductWithCategoryById(int id)
        {
            return _context.Products
                           .Include(p => p.Category) 
                           .FirstOrDefault(p => p.Id == id);
        }


        public void AddProduct(Product product)
        {
            try
            {
                if (product == null)
                    throw new ArgumentNullException(nameof(product));

                _context.Products.Add(product);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }




        public Product? GetProductById(int id)
        {
            return _context.Products.Find(id);
        }


        public void UpdateProduct(Product product)
        {
            var existingProduct = _context.Products.FirstOrDefault(p => p.Id == product.Id);
            if (existingProduct != null)
            {
                existingProduct.Name = product.Name;
                existingProduct.Description = product.Description;
                existingProduct.Price = product.Price;
                existingProduct.CategoryId = product.CategoryId;
                existingProduct.ImagePath = product.ImagePath;

                _context.SaveChanges();
            }
        }


        public void DeleteProduct(int id)
        {
            var product = _context.Products.Find(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
        }
    }
}
