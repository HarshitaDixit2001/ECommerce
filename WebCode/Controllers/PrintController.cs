using AppModels;
using AppServices; 
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.Extensions.Caching.Memory;
namespace AppWeb.Controllers
{
    [Route("print")]
    public class PrintController : BaseController
    {
        #region Constructors  
        private readonly IAddServices _addSvc;
        private readonly IGetServices _getSvc;
        public PrintController(IConfiguration config, IAddServices addServices,
            IGetServices getSvc, IMemoryCache cache) : base(config, addServices, getSvc, cache)
        {
            _addSvc = addServices;
            _getSvc = getSvc;
        }
        #endregion


        #region get-vendor-po-detail
        [HttpGet("get-vendor-po-detail")]
        public async Task<IActionResult> GetVendorPODetail(int? inv_id = 0)
        {
            try
            {
                inv_id_detail req = new inv_id_detail();
                req.inv_id = inv_id;
                db_common_res result = await _getSvc.GetVendorPODetail(req);
                return Ok(new { success = result.error == "", token_valid = true, data = result.json_result, message = "" });
            }
            catch (Exception ex) { return StatusCode(500, new { success = false, token_valid = true, data = "", message = ex.Message }); }
        }
        #endregion


        #region AdminPO
        [HttpGet("admin-po/{order_id}")]
        public async Task<IActionResult> AdminPO(int order_id)
        { 
            var model = new List<AdminOrderProductViewModel>();
            if (order_id > 0)
            {
                var req = new OrderIdRequestModel { order_id = order_id };
                db_common_res result = await _getSvc.GetOrderProductsByOrderId(req);
                if (result.json_result != null)
                {
                    model = JsonConvert.DeserializeObject<List<AdminOrderProductViewModel>>(result.json_result);
                }
            }
            return View(model);
        }



        [HttpGet("admin-po-seller-detail")]
        public async Task<IActionResult> AdminPO(int? order_id = 0)
        {
            if (order_id > 0)
            {
                var req = new OrderIdRequestModel { order_id = order_id };
                db_common_res result = await _getSvc.GetOrderBuyerSellerDetail(req);
                return Ok(new { success = result.error == "0", token_valid = true, data = result.json_result, message = "" });
            }
            return Ok(new { success = false, token_valid = true, data = "[]", message = "" });
        }
        #endregion



        #region UserPO
        [HttpGet("user-po/{order_id}")]
        public async Task<IActionResult> UserPO(int order_id)
        {
            var model = new List<UserOrderProductViewModel>();
            if (order_id > 0)
            {
                var req = new OrderIdRequestModel { order_id = order_id };
                db_common_res result = await _getSvc.GetUserOrderProductsByOrderId(req);
                if (result.json_result != null)
                {
                    model = JsonConvert.DeserializeObject<List<UserOrderProductViewModel>>(result.json_result);
                }
            }
            return View(model);
        }



        [HttpGet("user-po-seller-detail")]
        public async Task<IActionResult> UserPO(int? order_id = 0)
        {
            if (order_id > 0)
            {
                var req = new OrderIdRequestModel { order_id = order_id };
                db_common_res result = await _getSvc.GetUserOrderBuyerSellerDetail(req);
                return Ok(new { success = result.error == "0", token_valid = true, data = result.json_result, message = "" });
            }
            return Ok(new { success = false, token_valid = true, data = "[]", message = "" });
        }
        #endregion



        #region AdminInvoice
        [HttpGet("admin-invoice/{inv_id}")]
        public async Task<IActionResult> AdminInvoice(int inv_id)
        {
            var model = new List<AdminInvoiceProductViewModel>();
            if (inv_id > 0)
            {
                var req = new InvoiceIdRequestModel { inv_id = inv_id };
                db_common_res result = await _getSvc.GetInvoiceProductsByOrderId(req);
                if (result.json_result != null)
                {
                    model = JsonConvert.DeserializeObject<List<AdminInvoiceProductViewModel>>(result.json_result);
                }
            }
            return View(model);
        }



        [HttpGet("admin-invoice-seller-detail")]
        public async Task<IActionResult> AdminInvoice(int? inv_id = 0)
        {
            if (inv_id > 0)
            {
                var req = new InvoiceIdRequestModel { inv_id = inv_id };
                db_common_res result = await _getSvc.GetInvoiceBuyerSellerDetail(req);
                return Ok(new { success = result.error == "0", token_valid = true, data = result.json_result, message = "" });
            }
            return Ok(new { success = false, token_valid = true, data = "[]", message = "" });
        }
        #endregion


        #region get-vendor-po-detail
        [HttpGet("view-stock-detail/{uid}/{pid}")]
        public async Task<IActionResult> StockTran(int? uid, int? pid)
        {
            var model = new List<StockTranViewModel>();
            if (uid > 0 && pid > 0)
            {
                var req = new StockTranRequestModel { uid = uid, pid = pid };
                db_common_res result = await _getSvc.GetStockTranByUIdPID(req);
                if (result.json_result != null)
                {
                    model = JsonConvert.DeserializeObject<List<StockTranViewModel>>(result.json_result);
                }
            }
            return View(model);
        }
        #endregion


        #region VendorDetail
        [HttpGet("vendor-inv-detail/{purchase_id}")]
        public async Task<IActionResult> VendorInvDetail()
        {
            return View();
        }


        [HttpGet("vendor-po")]
        public async Task<IActionResult> VendorPODetail(int? purchase_id = 0)
        {
            if (purchase_id > 0)
            {
                var req = new VendorDetailRequestModel { purchase_id = purchase_id };
                db_common_res result = await _getSvc.GetVendorPODetailReport(req);
                return Ok(new { success = result.error == "0", token_valid = true, data = result.json_result, message = "" });
            }
            return Ok(new { success = false, token_valid = true, data = "[]", message = "" });
        }
         

        [HttpGet("vendor-product-detail")]
        public async Task<IActionResult> VendorProductDetails(int? purchase_id = 0)
        {
            if (purchase_id > 0)
            {
                var req = new VendorDetailRequestModel { purchase_id = purchase_id };
                db_common_res result = await _getSvc.GetVendorPOProductDetail(req);
                return Ok(new { success = result.error == "0", token_valid = true, data = result.json_result, message = "" });
            }
            return Ok(new { success = false, token_valid = true, data = "[]", message = "" });
        }
        #endregion


        #region EnquirySave
        [HttpPost("appointment-save")]
        public async Task<IActionResult> AppointmentSave([FromForm] EnquiryDto req)
        {
            try
            {
                req.visit_date = ParseDate(req.visit_date);
                var result = await _addSvc.SaveEnquiry(req);
                return Ok(new { success = result.error == "0", message = result.json_result });
            }
            catch { return StatusCode(500, new { success = false, token_valid = true, data = "", message = "Internal server error" }); }
        }
        #endregion
    }
}
