using bigSales.Data;
using bigSales.Models;
using System.Collections.Generic;
using System.Linq;

namespace bigSales.Services
{
    public class CategoryService
    {
        private readonly ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Category> GetAllCategories()
        {
            var categories = _context.Categories.ToList();
            if (categories == null || !categories.Any())
            {

                throw new InvalidOperationException("Kategoriler yüklenemedi.");
            }
            return categories;
        }

    }
}
