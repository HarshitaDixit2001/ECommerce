using AppModels; 
using Microsoft.AspNetCore.Mvc;
using QRCoder;

namespace WebCode.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpiController : ControllerBase
    {
        [HttpGet("qr")]
        public IActionResult GenerateQr(decimal amount = 1)
        { 
            string txnNote = "Order Payment";
            // ✅ Proper UPI URL (encoded)
            string upiString = $"upi://pay?pa={Const.COMP_UPI_ID}&pn={Uri.EscapeDataString(Const.COMP_UPI_NAME)}&am={amount}&cu=INR&tn={Uri.EscapeDataString(txnNote)}";
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                var qrData = qrGenerator.CreateQrCode(upiString, QRCodeGenerator.ECCLevel.Q);
                var qrCode = new PngByteQRCode(qrData);
                byte[] qrImage = qrCode.GetGraphic(20);
                return File(qrImage, "image/png");
            }
        }
    }
}
