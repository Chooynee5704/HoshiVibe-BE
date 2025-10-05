using System.ComponentModel.DataAnnotations;

namespace HoshiVibe.Entities.DTO.ModelRequests.User
{
    public class UpdateUserDTO
    {
        [Required]
        public Guid User_Id { get; set; }

        [Required]
        public  required string Password { get; set; }
        [Required]
        public required string Email {  get; set; }

    }
}
