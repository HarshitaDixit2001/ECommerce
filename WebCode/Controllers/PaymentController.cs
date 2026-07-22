using AppModels;
using AppServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
namespace WebCode.Controllers
{
    [Route("payment")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        #region Constructors  
        private readonly IAddServices _addSvc;
        public PaymentController(IAddServices addServices)
        {
            _addSvc = addServices;
        }
        #endregion


        [HttpPost("receive")]
        public async Task<IActionResult> Receive([FromBody] SmsPaymentReceive model, [FromHeader(Name = "x-api-key")] string apiKey)
        {
            try
            {
                // ✅ STEP 1: Validate API Key
                if (apiKey != "master_b7F9kQ2zX4Lm8P1rV6sT0wY3A9cD5E7hJ2K8N4uM6Z1xR0qP")
                {
                    return Unauthorized(new { success = false, message = "Invalid API Key" });
                }

                // ✅ STEP 2: Validate Model
                if (model == null || string.IsNullOrWhiteSpace(model.refId))
                {
                    return BadRequest(new { success = false, message = "Invalid Data" });
                }

                // ✅ STEP 3: Save raw JSON (logging)
                string jsonBody = JsonConvert.SerializeObject(model);

                if (!Directory.Exists(Const.IMAGEPATH))
                    Directory.CreateDirectory(Const.IMAGEPATH);

                string fileName = $"PaymentTracking_{DateTime.Now:yyyyMMddHHmmssfff}.json";
                string filePath = Path.Combine(Const.IMAGEPATH, fileName);

                await System.IO.File.WriteAllTextAsync(filePath, jsonBody);

                // ✅ STEP 4: Process Data
                await _addSvc.ProcessSmsPayment(model);

                // ✅ SUCCESS RESPONSE
                return Ok(new { success = true, message = "Payment received successfully" });
            }
            catch (Exception ex)
            {
                try
                {
                    // ✅ Error logging
                    if (!Directory.Exists(Const.IMAGEPATH))
                        Directory.CreateDirectory(Const.IMAGEPATH);

                    string errorFile = Path.Combine(Const.IMAGEPATH, "webhook_error.txt");

                    await System.IO.File.AppendAllTextAsync(errorFile, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} : {ex}\n");
                }
                catch
                {
                    // Avoid crash if logging fails
                }
                // ❗ IMPORTANT: Always return response
                return StatusCode(500, new { success = false, message = "Internal Server Error", error = ex.Message });
            }
        }

         

        [HttpGet("status")]
        public IActionResult Status()
        {
            return Ok("Webhook API Working");
        }

    }
}
