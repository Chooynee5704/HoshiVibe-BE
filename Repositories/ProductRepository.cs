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

        public Product? GetProductById(Guid? id)
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

public ICollection<Product> GetProposedProducts(string destiny)
{
    // Chuẩn hóa chữ thường để tránh sai khi so sánh
    string lowerDestiny = destiny.ToLower();

    // Xác định danh sách mệnh tương sinh / tương hợp
    List<string> relatedDestinies = new();

    switch (lowerDestiny)
    {
        case "kim":
            relatedDestinies.AddRange(new[] { "kim", "thủy" });
            break;
        case "mộc":
            relatedDestinies.AddRange(new[] { "mộc", "hỏa" });
            break;
        case "thủy":
            relatedDestinies.AddRange(new[] { "thủy", "mộc" });
            break;
        case "hỏa":
            relatedDestinies.AddRange(new[] { "hỏa", "thổ" });
            break;
        case "thổ":
            relatedDestinies.AddRange(new[] { "thổ", "kim" });
            break;
        default:
            // nếu không có mệnh hợp thì chỉ tìm theo chính mệnh
            relatedDestinies.Add(lowerDestiny);
            break;
    }

    // Lọc các sản phẩm theo mệnh tương hợp / tương sinh
    return _context.Products
        .Where(p => p.Destiny != null && relatedDestinies.Contains(p.Destiny.ToLower()))
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
