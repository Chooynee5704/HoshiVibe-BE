namespace HoshiVibe.Entities.DTO.ModelRequests.OderProcess
{
    public class UpdateShippingStatusDTO
    {
        public required string ShippingStatus { get; set; } // Pending, Shipping, Delivered, PickedUp
    }
}
