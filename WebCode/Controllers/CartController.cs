using AppModels;
using AppServices; 
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using WebCode.Data;
using WebCode.DataService;
namespace AppWeb.Controllers
{
    [Route("cart"), Authorize]
    public class CartController : BaseController
    {

        #region Constructors  
        private readonly IAddServices _addSvc;
        private readonly IGetServices _getSvc;
        public CartController(IConfiguration config, IAddServices addServices,
            IGetServices getSvc, IMemoryCache cache) : base(config, addServices, getSvc, cache)
        {
            _addSvc = addServices;
            _getSvc = getSvc;
        }
        #endregion


        #region cart-data
        [HttpGet("add-cart-data")]
        public async Task<IActionResult> AddCartData(int? cart_id = 0, int? uid = 0,
            decimal? rate = 0, int? prod_type = 0, int? pid = 0, int? cid = 0, int? qty = 0)
        {
            if (prod_type == 3)
                if (RoleWiseLogin("admin_sub_role") is IActionResult rolecheck) return rolecheck;
            try
            {
                var req = new add_cart_data()
                {
                    cart_id = cart_id,
                    seller_uid = 0,
                    uid = uid,
                    pid = pid,
                    prod_type = prod_type,
                    qty = qty,
                    rate = rate,
                    cid = cid
                };
                db_common_res result = await _addSvc.AddCartData(req);
                return Ok(new { success = result.error == "0", token_valid = true, data = result.json_result, message = "" });
            }
            catch (Exception ex) { return StatusCode(500, new { success = false, token_valid = true, message = "Internal server error", details = ex.Message }); }
        }


        [HttpGet("get-cart-data")]
        public async Task<IActionResult> GetCartData(int? cart_id = 0, int? uid = 0)
        {
            try
            {
                var req = new filter_cart_data()
                {
                    cart_id = cart_id,
                    uid = uid
                };
                db_common_res result = await _addSvc.GetCartData(req);
                if (result == null)
                    return Ok(new { success = false, token_valid = true, data = "[]", message = "" });
                return Ok(new { success = result.error == "", token_valid = true, data = result.json_result, message = "" });
            }
            catch (Exception ex) { return StatusCode(500, new { success = false, token_valid = true, message = "Internal server error", details = ex.Message }); }
        }


        [HttpGet("get-cart-scheme-data")]
        public async Task<IActionResult> GetCartSchemeData(int? cart_id = 0, int? uid = 0)
        {
            try
            {
                var req = new filter_cart_data()
                {
                    cart_id = cart_id,
                    uid = uid
                };
                db_common_res result = await _addSvc.GetCartSchemeData(req);
                if (result == null)
                    return Ok(new { success = false, token_valid = true, data = "[]", message = "" });

                return Ok(new { success = result.error == "", token_valid = true, data = result.json_result, message = "" });
            }
            catch (Exception ex) { return StatusCode(500, new { success = false, token_valid = true, message = "Internal server error", details = ex.Message }); }
        }


        [HttpGet("delete-cart-data")]
        public async Task<IActionResult> DeletCartData(int? id = 0)
        {
            try
            {
                delete_cart_data req = new delete_cart_data();
                req.id = id;
                db_common_res result = await _addSvc.DeletCartData(req);
                return Ok(new { success = result.error == "0", token_valid = true, data = result.json_result, message = "" });
            }
            catch (Exception ex) { return StatusCode(500, new { success = false, token_valid = true, message = "Internal server error", details = ex.Message }); }
        }




        [HttpPost("update-invoice-detail-id")]
        public async Task<IActionResult> update_invoice_detail_id([FromBody] update_invoice_detail_id req)
        {
            try
            {
                db_common_res result = await _addSvc.UpdateInvoiceDetailId(req);
                return Ok(new { success = result.error == "0", token_valid = true, data = result.json_result, message = "" });
            }
            catch (Exception ex) { return StatusCode(500, new { success = false, token_valid = true, data = "", message = ex.Message }); }
        }


        [HttpGet("get-count-cart-data")]
        public async Task<IActionResult> GetCountCartData(int? cart_id, int? uid)
        {
            try
            {
                var req = new filter_cart_data { cart_id = cart_id, uid = uid };
                var db_result = await _addSvc.GetCountCartData(req);
                return Ok(new { success = db_result.error == "0" ? true : false, data = db_result.json_result, message = "" });
            }
            catch (Exception ex) { return StatusCode(500, new { success = false, data = 0, message = "Internal server error" }); }
        }


        #endregion


        #region view-cart
        [HttpGet("view-cart")]
        public async Task<IActionResult> ViewCart()
        {
            var check = CheckAndLogout();
            if (check != null) return check;

            var model = new GenerateOrderDto();
            var search_obj = new dropdown_request_param { mst_key = "GET_STATE_LIST" };
            var StateList = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
            ViewBag.StateList = new SelectList(StateList ?? new List<dropdown_response>(), "Value", "Text");

            ViewBag.DistrictList = new SelectList(new List<DropdownItem>(), "Value", "Text");

            return View(model);
        }



        [HttpPost("view-cart")]
        public async Task<IActionResult> ViewCart([FromForm] GenerateOrderDto req)
        {
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;

            var uid = User.FindFirst(System.Security.Claims.ClaimTypes.Sid)?.Value;
            var user_id = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, message = string.Join("\n", errors) });
            }
            req.cart_id = 3;
            req.seller_uid = Convert.ToInt32(Const.DEFAULT_SALLER);
            req.user_id = user_id;
            req.buyer_uid = Convert.ToInt32(uid);
            req.checkdate = ParseDate(req.checkdate);
            // 🔹 save data
            db_order_res result = await _addSvc.GenerateUserOrder(req);
            return Ok(new { success = result.error == "0", inv_gen = result.inv_gen, message = result.json_result });
        }



        [HttpPost("add-shipping-address")]
        public async Task<IActionResult> AddShippingAddress([FromForm] ShippingAddressDto req)
        {
            try
            { 
                db_common_res result = await _addSvc.AddShippingAddress(req);
                return Ok(new { success = result.error == "0", token_valid = true, data = result.json_result, message = "" });
            }
            catch (Exception ex) { return StatusCode(500, new { success = false, token_valid = true, message = "Internal server error", details = ex.Message }); }
        }

        [HttpGet("get-address-list")]
        public async Task<IActionResult> GetAddress(int? uid = 0)
        {
            try
            {
                var req = new AddressFilter()
                { 
                    uid = uid,
                    said=0
                };
                db_common_res result = await _addSvc.GetAddressList(req);
                if (result == null)
                    return Ok(new { success = false, token_valid = true, data = "[]", message = "" });
                return Ok(new { success = result.error == "", token_valid = true, data = result.json_result, message = "" });
            }
            catch (Exception ex) { return StatusCode(500, new { success = false, token_valid = true, message = "Internal server error", details = ex.Message }); }
        }
         

        private readonly OcrService _ocr = new OcrService();
        private readonly PaymentParser _parser = new PaymentParser();

        [HttpPost("upload-slip")]
        public async Task<IActionResult> UploadSlip(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File not uploaded");

            var uploads = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
            if (!Directory.Exists(uploads))
                Directory.CreateDirectory(uploads);

            var filePath = Path.Combine(uploads, file.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            // ✅ OCR
            var text = _ocr.ExtractText(filePath);
            // ✅ Parse Data
            var result = _parser.Parse(text);
            return Ok(result);
        }
        #endregion



        #region ord-inv-success
        [HttpGet("ord-inv-success")]
        public async Task<IActionResult> Ord_Inv_Success(int? inv_gen=null, string? ord_inv_no=null)
        {
            var model = new GetOrderInvModel();

            if (!string.IsNullOrEmpty(ord_inv_no))
            {
                var req_obj = new GetOrderInvFilter() { inv_gen = inv_gen, ord_inv_no= ord_inv_no };

                db_common_res json = await _getSvc.GetOrdInvSuccess(req_obj);
                if (json?.json_result != null)
                {
                    var first = JsonConvert.DeserializeObject<GetOrderInvModel>(json?.json_result);
                    if (first != null)
                    {
                        model = new GetOrderInvModel()
                        {
                            inv_gen = inv_gen,
                            ord_inv_id = first.ord_inv_id,
                            ord_inv_no = first.ord_inv_no,
                            pay_mode = first.pay_mode,
                            doe = first.doe,
                            t_amount = first.t_amount
                        };
                    } 
                }
            }



            //var search_obj = new dropdown_request_param { mst_key = "GET_STATE_LIST" };
            //var StateList = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
            //ViewBag.StateList = new SelectList(StateList ?? new List<dropdown_response>(), "Value", "Text");
             

            return View(model);
        }
        #endregion


        #region wish-list
        [HttpGet("wish-list")]
        public async Task<IActionResult> Wishlist()
        { 
            return View();
        }
        #endregion


    }
}
