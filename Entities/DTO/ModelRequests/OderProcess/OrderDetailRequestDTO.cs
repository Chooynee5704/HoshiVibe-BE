using System.ComponentModel.DataAnnotations;

namespace HoshiVibe.Entities.DTO.ModelRequests.OderProcess
{
    public class OrderDetailRequestDTO
    {
        // OrderId is optional - if not provided, will use user's pending order or create new one
        public string? OrderId { get; set; }

        public Guid? ProductId { get; set; }
        public Guid? CProduct_Id { get; set; }
        public Guid? CustomDesign_Id { get; set; }
        
        [Range(1, int.MaxValue, ErrorMessage = "Quantity phải lớn hơn 0")]
        public int Quantity { get; set; }
        
        [Range(0, double.MaxValue, ErrorMessage = "UnitPrice phải lớn hơn hoặc bằng 0")]
        public decimal UnitPrice { get; set; }
        
        [Range(0, 1, ErrorMessage = "Discount phải từ 0 đến 1")]
        public decimal Discount { get; set; }
        
        public decimal TotalPrice => UnitPrice * Quantity * (1 - Discount);
    }
}
