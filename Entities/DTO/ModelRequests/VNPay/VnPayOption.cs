namespace HoshiVibe.Entities.DTO.ModelRequests.VNPay
{
    public class VnPayOption
    {
        public string TmnCode { get; set; } = "";
        public string HashSecret { get; set; } = "";
        public string PaymentUrl { get; set; } = "";
        public string ReturnUrl { get; set; } = "";
        public string IpnUrl { get; set; } = "";
        public string FrontendSuccessUrl { get; set; } = "";
        public string FrontendFailUrl { get; set; } = "";
        public string Locale { get; set; } = "vn";
        public string CurrCode { get; set; } = "VND";
        public string Version { get; set; } = "2.1.0";
        public string Command { get; set; } = "pay";
    }
}
