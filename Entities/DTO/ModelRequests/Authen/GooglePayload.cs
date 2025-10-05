namespace HoshiVibe.Entity.DTO.ModelRequests.Authen
{
    public class GooglePayload
    {
        public string Email { get; set; }
        public string Name { get; set; }

        public string Aud { get; set; } // Add this
    }
}
