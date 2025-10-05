using HoshiVibe.DB;
using HoshiVibe.Entities.Models.Base;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace HoshiVibe.Repositories
{
    public class ProductRepository
    {
        private readonly DataContext _context;
        public ProductRepository(DataContext context)
        {
            _context = context;
        }

        public Product? GetProductById(Guid id)
        {
            return _context.Products.FirstOrDefault(u => u.Product_Id == id);
        }
        public bool ProductExists(Guid id)
        {
            return _context.Products.Any(u => u.Product_Id == id);
        }
        public ICollection<Product> Search(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                // Nếu query trống -> trả về toàn bộ danh sách
                return _context.Products
                    .OrderBy(p => p.Price)
                    .ToList();
            }
            return _context.Products
                .Where(p => 
                        p.Description.ToLower().Contains(keyword.ToLower()) || 
                        p.Category.ToLower().Contains(keyword.ToLower()) || 
                        p.Name.ToLower().Contains(keyword.ToLower()))
                .OrderBy(p => p.Price)
                .ToList();
        }
        public bool CreateProduct(Product product)
        {
            _context.Products.Add(product);
            return Save();
        }
        public bool UpdateProduct(Product product)
        {
            _context.Products.Update(product);
            return Save();
        }

        public bool DeleteProduct(Product product)
        {
            _context.Products.Remove(product);
            return Save();
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }

    }
}
