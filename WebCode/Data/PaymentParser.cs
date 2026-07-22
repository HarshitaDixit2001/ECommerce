using AppModels;
using System.Text.RegularExpressions;
namespace WebCode.Data
{
    public class PaymentParser
    { 
        public PaymentResult Parse(string text)
        {
            var result = new PaymentResult
            {
                utr_fullText = text
            };

            if (string.IsNullOrWhiteSpace(text))
                return result;

            text = text.Replace("\n", " ").Replace("\r", " ");

            // =========================
            // ✅ 1. AMOUNT (Strong)
            // =========================
            var amountPatterns = new[]
            {
                @"(?:Rs\.?|INR|₹)\s?(\d{1,3}(?:,\d{3})*(?:\.\d{1,2})?)",
                @"Amount\s*[:\-]?\s*(\d+(?:\.\d{1,2})?)",
                @"Received\s*(\d+(?:\.\d{1,2})?)",
                @"\b(\d{2,6})\b" // fallback
            };

            foreach (var pattern in amountPatterns)
            {
                var match = Regex.Match(text, pattern, RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    result.utr_amount = match.Groups[1].Value.Replace(",", "");
                    break;
                }
            }



            //// =========================
            //// ✅ 2. REF ID / UTR (Strong)
            //// =========================
            var utrMatch = Regex.Match(text,
                @"(?:UPI\s*Ref(?:erence)?\s*(?:No)?|UTR)[\s:\-]*([0-9]{12})",
                RegexOptions.IgnoreCase);

            if (utrMatch.Success)
            {
                result.utr_no = utrMatch.Groups[1].Value;
            }
            else
            {
                // fallback (only 12 digit allowed)
                var fallbackUtr = Regex.Match(text, @"\b([0-9]{12})\b");
                result.utr_no = fallbackUtr.Success ? fallbackUtr.Groups[1].Value : "";
            } 

            // =========================
            // ✅ 3. DATE TIME (Flexible)
            // =========================
            var datePatterns = new[]
            {
                @"\d{1,2}[:.]\d{2}\s?(AM|PM)\s?,?\s?\d{1,2}\s?[A-Za-z]{3,}\s?\d{4}", // 11:27 AM, 17 Apr 2026
                @"\d{1,2}[-/]\d{1,2}[-/]\d{2,4}\s?\d{1,2}:\d{2}",
                @"\d{1,2}\s?[A-Za-z]{3,}\s?\d{4}"
            };

            foreach (var pattern in datePatterns)
            {
                var match = Regex.Match(text, pattern, RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    result.utr_doe = match.Value;
                    break;
                }
            }

            // =========================
            // ✅ 4. SENDER NAME (NEW 🔥)
            // =========================
            var senderMatch = Regex.Match(text, @"From\s+([A-Za-z\s]+)", RegexOptions.IgnoreCase);
            result.utr_senderName = senderMatch.Success ? senderMatch.Groups[1].Value.Trim() : "";

            // =========================
            // ✅ 5. UPI ID (NEW 🔥)
            // =========================
            var upiMatch = Regex.Match(text, @"([a-zA-Z0-9.\-_]+@[a-zA-Z]+)", RegexOptions.IgnoreCase);
            result.utr_upiId = upiMatch.Success ? upiMatch.Value : "";

            return result;
        }
    }
}
