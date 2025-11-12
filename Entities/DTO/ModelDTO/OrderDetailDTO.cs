using HoshiVibe.Entities.Models.Base;
using HoshiVibe.Entity.DTO.ModelDTO;
using HoshiVibe.Entities.DTO.ModelRequests.CustomDesign;

namespace HoshiVibe.Entity.Model
{
    public class OrderDetailDTO
    {
        public Guid OrderDetailId { get; set; }
        public string? OrderId { get; set; }
        
        public Guid? ProductId { get; set; }
        public Guid? CProductId { get; set; }
        public Guid? CustomDesignId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalPrice => UnitPrice * Quantity * (1 - Discount);

        // Include product details for cart display
        public ProductDTO? Product { get; set; }

        // Include custom design details when the order detail refers to a custom design
        public CustomDesignDTO? CustomDesign { get; set; }
    }
}

