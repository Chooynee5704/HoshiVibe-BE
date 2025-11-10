using System.Security.Cryptography;
using System.Text;

namespace HoshiVibe.Service
{
    public class VnPayServiceSigner
    {
        public static string BuildRawToSign(IDictionary<string, string> dict)
            => string.Join("&", dict
                .Where(kv => !string.IsNullOrWhiteSpace(kv.Value))
                .Where(kv => kv.Key != "vnp_SecureHash" && kv.Key != "vnp_SecureHashType")
                .OrderBy(kv => kv.Key, StringComparer.Ordinal)
                .Select(kv =>
                    $"{Uri.EscapeDataString(kv.Key)}={Uri.EscapeDataString(kv.Value)}"));

        // 3.2: Chuỗi ENCODE để gửi: key/value URL-encode, sort A→Z
        public static string BuildEncodedQuery(IDictionary<string, string> dict)
            => string.Join("&", dict
                .Where(kv => !string.IsNullOrEmpty(kv.Value))
                .OrderBy(kv => kv.Key, StringComparer.Ordinal)
                .Select(kv => $"{Uri.EscapeDataString(kv.Key)}={Uri.EscapeDataString(kv.Value)}"));

        // 3.3: HMAC SHA512
        public static string HmacSHA512(string key, string data)
        {
            using var h = new HMACSHA512(Encoding.UTF8.GetBytes(key));
            var hash = h.ComputeHash(Encoding.UTF8.GetBytes(data));
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }
    }
}
