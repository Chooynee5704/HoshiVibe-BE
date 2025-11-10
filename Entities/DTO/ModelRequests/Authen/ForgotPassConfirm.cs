namespace HoshiVibe.Entity.DTO.ModelRequests.Authen
{
    public class ForgotPassConfirm
    {
        public required string Email { get; set; }
        public required string Account { get; set; }
        public required string VerificationCode { get; set; }
        public required string NewPassword { get; set; }
    }
}
