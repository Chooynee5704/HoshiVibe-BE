namespace HoshiVibe.Entity.DTO.ModelRequests.Authen
{
    public class ForgotPassRq
    {
        public required string  Account { get; set; }
        public required string  Email { get; set; }
    }
}
