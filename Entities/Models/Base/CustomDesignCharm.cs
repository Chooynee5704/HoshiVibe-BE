namespace HoshiVibe.Entities.Models.Base
{
    public class CustomDesignCharm
    {
        public Guid CustomDesignCharm_Id { get; set; }
        public Guid CustomDesign_Id { get; set; }
        public Guid CProduct_Id { get; set; } // Reference to charm (CustomProduct)
        
        // Navigation properties
        public CustomDesign? CustomDesign { get; set; }
        public CustomProduct? Charm { get; set; }
    }
}
