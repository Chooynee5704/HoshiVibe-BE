using HoshiVibe.Entities.Models.Base;

namespace HoshiVibe.Entities.DTO.ModelRequests.OderProcess
{
    public class CartRequestDTO
    {
        public Guid User_Id { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    }
}
