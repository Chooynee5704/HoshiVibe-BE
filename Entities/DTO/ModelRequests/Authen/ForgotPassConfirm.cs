namespace HoshiVibe.Entity.DTO.ModelRequests.Authen
{
    public class ForgotPassConfirm
    {
        public required string Identifier { get; set; }
        public required string VerificationCode { get; set; }
        public required string NewPassword { get; set; }
    }
}
