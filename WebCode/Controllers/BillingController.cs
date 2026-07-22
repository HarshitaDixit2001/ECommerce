using AppModels;
using AppServices;
using Microsoft.AspNetCore.Authorization; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Microsoft.Extensions.Caching.Memory;
namespace AppWeb.Controllers
{
    [Route("billing"), Authorize]
    public class BillingController : BaseController
    {

        #region Constructors  
        private readonly IAddServices _addSvc;
        private readonly IGetServices _getSvc;
        public BillingController(IConfiguration config, IAddServices addServices, IGetServices getSvc, IMemoryCache cache)
         : base(config, addServices, getSvc, cache)  
        {
            _addSvc = addServices;
            _getSvc = getSvc;
        }
        #endregion

 

        #region GenerateFranInvPO
        [HttpGet("generate-fran-order/{buyer_uid?}")]
        public async Task<IActionResult> GenerateFranOrder(int? buyer_uid = 0)
        {
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;

            var model = new GenerateInvoiceDto();
             
            var search_obj = new dropdown_request_param { mst_key = "GET_STATE_LIST" };
            var StateList = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
            ViewBag.StateList = new SelectList(StateList ?? new List<dropdown_response>(), "Value", "Text");


            search_obj = new dropdown_request_param { mst_key = "GET_All_USER", 
                id = Convert.ToInt32(2) };
            var BuyerList = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
            ViewBag.BuyerList = new SelectList(BuyerList ?? new List<dropdown_response>(), "Value", "Text");
        
            return View(model);
        }


        [HttpPost("generate-fran-order/{buyer_uid?}")]
        public async Task<IActionResult> GenerateFranOrder([FromForm] GenerateInvoiceDto req, int? buyer_uid = 0)
        {
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;

 
            var search_obj = new dropdown_request_param { mst_key = "GET_STATE_LIST" };
            if (!ModelState.IsValid)
            {
                var allErrors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                // Optional: Join multiple errors into one message, or just take the first
                TempData["Error"] = string.Join("/n", allErrors);

                var StateList = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
                ViewBag.StateList = new SelectList(StateList ?? new List<dropdown_response>(), "Value", "Text", req.state_id);

                search_obj = new dropdown_request_param { mst_key = "GET_DISTRICT_LIST", id = Convert.ToInt32(req.state_id) };
                var DistrictList = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
                ViewBag.DistrictList = new SelectList(DistrictList ?? new List<dropdown_response>(), "Value", "Text", req.dist_id);

                search_obj = new dropdown_request_param { mst_key = "GET_All_USER", id = Convert.ToInt32(2) };
                var BuyerList = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
                ViewBag.BuyerList = new SelectList(BuyerList ?? new List<dropdown_response>(), "Value", "Text", buyer_uid);
  
                return View(req);
            }

            var uid = User.FindFirst(System.Security.Claims.ClaimTypes.Sid)?.Value;
            req.user_id = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            req.seller_uid = Convert.ToInt32(uid);
            req.buyer_uid = req.buyer_uid;
             
            // 🔹 save data
            db_common_res result = await _addSvc.GenarateFranchiseOrderInvoice(req);
            if (result.error == "0")
            {
                var order_no = result.json_result;
                int order_id = 0;
                var seller_mobile = "";

                // Send WhatsApp Message start
                var req_obj = new guest_report_request_param { mst_key = "GET_ORDER_DETAILS", invoice_no = order_no };
                db_common_res json = await _getSvc.master_reports(req_obj);
                if (!string.IsNullOrWhiteSpace(json.json_result))
                {
                    var first = JsonConvert.DeserializeObject<List<OrderDetails_DB>>(json.json_result)?.FirstOrDefault();
                    if (first != null)
                    {
                        order_id = first.order_id ?? 0;
                        seller_mobile = first.seller_mobile ?? string.Empty;
                         
                    }
                }
                // End Send WhatsApp Message
                

                TempData["Success"] =
                 "✅ Your order has been generated. Your order number is : " + order_no +
                 "<br> <br> <br> <a href='" + Const.URL_ORDER + order_id.ToString() + "' target='_blank' style='color:blue; font-weight:bold;'>🖨 Print Order</a>" +
                 " | <a href='" + Const.URL_ORDER + order_id.ToString() + "' onclick=\"navigator.clipboard.writeText(this.href); alert('Link copied!'); return false;\" style='color:green; font-weight:bold;'>📋 Copy Link</a>";

                //return RedirectToAction("generate-order", "billing");
            }
            TempData["Error"] = result.json_result;

            search_obj = new dropdown_request_param { mst_key = "GET_STATE_LIST" };
            var StateList_2 = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
            ViewBag.StateList = new SelectList(StateList_2 ?? new List<dropdown_response>(), "Value", "Text", req.state_id);

            search_obj = new dropdown_request_param { mst_key = "GET_DISTRICT_LIST", id = Convert.ToInt32(req.state_id) };
            var DistrictList_2 = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
            ViewBag.DistrictList = new SelectList(DistrictList_2 ?? new List<dropdown_response>(), "Value", "Text", req.dist_id);

            search_obj = new dropdown_request_param { mst_key = "GET_All_USER", id = Convert.ToInt32(2) };
            var BuyerList_2 = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
            ViewBag.BuyerList = new SelectList(BuyerList_2 ?? new List<dropdown_response>(), "Value", "Text", buyer_uid);
             
            return View(req);
        }
        #endregion



        #region FranInvoiceList 
        [HttpGet("fran-invoice-list")]
        public async Task<IActionResult> FranInvoiceList()
        {
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;
            return View();
        }


        [HttpGet("admin-fran-invoice-list")]
        public async Task<IActionResult> AdminFranInvoiceList()
        {
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;
            return View();
        }


        [HttpPost("get-fran-invoice-list")]
        public async Task<IActionResult> GetFranInvoiceList([FromBody] InvoiceListFilter req)
        {
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;
            try
            {
                var result = await _getSvc.FranInvoiceList(req);
                return Ok(new { success = true, token_valid = true, data = result, message = "" });
            }
            catch
            {
                return StatusCode(500, new { success = false, token_valid = true, data = "", total_count = 0, message = "Internal server error" });
            }
        }
        #endregion



        #region FranOrderList
        [HttpGet("fran-made-order-list")]
        public async Task<IActionResult> FranMadeOrderList()
        {
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;
            return View();
        }

        [HttpGet("fran-received-order-list")]
        public async Task<IActionResult> FranReceviedOrderList()
        {
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;
            return View();
        }


        [HttpGet("admin-fran-order-list")]
        public async Task<IActionResult> AdminFranOrderList()
        {
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;
            return View();
        }


        [HttpPost("get-fran-order-list")]
        public async Task<IActionResult> GetFranOrderList([FromBody] OrderListFilter req)
        {
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;
            try
            {
                var result = await _getSvc.FranOrderList(req);
                return Ok(new { success = true, token_valid = true, data = result, message = "" });
            }
            catch (Exception ex) { return StatusCode(500, new { success = false, token_valid = true, data = "", total_count = 0, message = "Internal server error" }); }
        }
        #endregion




        #region GenerateUserOrder
        [HttpGet("generate-user-order")]
        public async Task<IActionResult> GenerateUserOrder()
        {
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;
             
            var model = new GenerateOrderDto();
              
            var search_obj = new dropdown_request_param { mst_key = "GET_STATE_LIST" };
            var StateList = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
            ViewBag.StateList = new SelectList(StateList ?? new List<dropdown_response>(), "Value", "Text");

            search_obj = new dropdown_request_param { mst_key = "GET_All_PRODUCTS" };
            var ProductList = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
            ViewBag.ProductList = new SelectList(ProductList ?? new List<dropdown_response>(), "Value", "Text");

            return View(model);
        }


        [HttpPost("generate-user-order")]
        public async Task<IActionResult> GenerateUserOrder([FromForm] GenerateOrderDto req)
        {
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;

            var uid = User.FindFirst(System.Security.Claims.ClaimTypes.Sid)?.Value;
            var user_id = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            var search_obj = new dropdown_request_param { mst_key = "GET_STATE_LIST" };
            if (!ModelState.IsValid)
            {
                var allErrors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                // Optional: Join multiple errors into one message, or just take the first
                TempData["Error"] = string.Join("/n", allErrors);
 
                var StateList = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
                ViewBag.StateList = new SelectList(StateList ?? new List<dropdown_response>(), "Value", "Text", req.state_id);

                search_obj = new dropdown_request_param { mst_key = "GET_DISTRICT_LIST", id = Convert.ToInt32(req.state_id) };
                var DistrictList = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
                ViewBag.DistrictList = new SelectList(DistrictList ?? new List<dropdown_response>(), "Value", "Text", req.dist_id);
 
                return View(req);
            }
            
            req.seller_uid = Convert.ToInt32(Const.DEFAULT_SALLER); 
            req.user_id = user_id;
            req.buyer_uid = Convert.ToInt32(uid);

            // 🔹 save data
            db_order_res result = await _addSvc.GenerateUserOrder(req);
            if (result.error == "0")
            {
                //var invoice_no = result.json_result;
                //int inv_id = 0;
                //var seller_mobile = "";

                //// Send WhatsApp Message start
                //var req_obj = new guest_report_request_param { mst_key = "GET_INVOICE_DETAILS", invoice_no = invoice_no };
                //db_common_res json = await _getSvc.master_reports(req_obj);
                //if (!string.IsNullOrWhiteSpace(json.json_result))
                //{
                //    var first = JsonConvert.DeserializeObject<List<InvoiceDetails_DB>>(json.json_result)?.FirstOrDefault();
                //    if (first != null)
                //    {
                //        inv_id = first.inv_id ?? 0;
                //        seller_mobile = first.seller_mobile ?? string.Empty;

                //    }
                //}
                // End Send WhatsApp Message
                if (result?.error == "0")
                {
                    if (req.pay_id == 9 || req.pay_id == 10)//(9, 10) Wallet Payment Mode 
                        TempData["Success"] = "Your invoice has been generated. Your invoice number is : " + result?.json_result;
                    else if (req.pay_id > 0 || req.pay_id <= 8)// Cash, UPI, and Banking Payment Mode 
                        TempData["Success"] = "Your purchase order has been generated. Your order  number is : " + result?.json_result;
                }
                else
                {
                    TempData["Error"] = result?.json_result ?? Const.ERROR_HEADER;
                }
                 
                return RedirectToAction("generate-user-order", "billing");
            }
            TempData["Error"] = result.json_result;

            search_obj = new dropdown_request_param { mst_key = "GET_STATE_LIST" };
            var StateList_2 = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
            ViewBag.StateList = new SelectList(StateList_2 ?? new List<dropdown_response>(), "Value", "Text", req.state_id);

            search_obj = new dropdown_request_param { mst_key = "GET_DISTRICT_LIST", id = Convert.ToInt32(req.state_id) };
            var DistrictList_2 = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
            ViewBag.DistrictList = new SelectList(DistrictList_2 ?? new List<dropdown_response>(), "Value", "Text", req.dist_id);
 
            return View(req);
        }
        #endregion




        #region user-order-list
        [HttpGet("user-order-list")]
        public async Task<IActionResult> UserOrderList()
        {
            var check = CheckAndLogout();
            if (check != null) return check;
             
            return View();
        }

        [HttpGet("admin-user-order-list")]
        public async Task<IActionResult> AdminUserOrderList()
        {
            var check = CheckAndLogout();
            if (check != null) return check;

            return View();
        }


        [HttpPost("get-user-order-list")]
        public async Task<IActionResult> GetUserOrderList([FromBody] UserOrderListFilter req)
        {
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;
            try
            {
                var result = await _getSvc.UserOrderList(req);
                return Ok(new { success = true, token_valid = true, data = result, message = "" });
            }
            catch (Exception ex) { return StatusCode(500, new { success = false, token_valid = true, data = "", total_count = 0, message = "Internal server error" }); }
        }

        #endregion



        #region user-invoice-list
        [HttpGet("user-invoice-list")]
        public async Task<IActionResult> UserInvoiceList()
        {
            var check = CheckAndLogout();
            if (check != null) return check;
             
            return View();
        }

        [HttpGet("fran-user-invoice-list")]
        public async Task<IActionResult> FranUserInvoiceList()
        {
            var check = CheckAndLogout();
            if (check != null) return check;

            return View();
        }

        [HttpGet("admin-user-invoice-list")]
        public async Task<IActionResult> AdminUserInvoiceList()
        {
            var check = CheckAndLogout();
            if (check != null) return check;

            return View();
        }


        [HttpPost("get-user-invoice-list")]
        public async Task<IActionResult> GetUserInvoiceList([FromBody] UserInvoiceListFilter req)
        {
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;
            try
            {
                var result = await _getSvc.UserInvoiceList(req);
                return Ok(new { success = true, token_valid = true, data = result, message = "" });
            }
            catch
            {
                return StatusCode(500, new { success = false, token_valid = true, data = "", total_count = 0, message = "Internal server error" });
            }
        }

        #endregion



       


        #region GenerateUserInvoice
        [HttpGet("generate-user-invoice/{order_id?}")]
        public async Task<IActionResult> GenerateUserInvoice(int? order_id)
        {
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;

            var uid = User.FindFirst(System.Security.Claims.ClaimTypes.Sid)?.Value;
            var model = new GenerateInvoiceDto();
            model.order_id = order_id;
            if (order_id != null)
            {
                var req_obj = new UpdateOrderDetailFilter() { order_id = order_id };
                db_common_res json = await _getSvc.UserUpdateOrderDetail(req_obj);
                if (json?.json_result != null)
                {
                    var list = JsonConvert.DeserializeObject<List<OrderDetailFromDB>>(json.json_result);
                    var first = list?.FirstOrDefault();
                    if (first != null)
                    {
                        model = new GenerateInvoiceDto
                        {
                            order_no = first.order_no,
                            buyer_uid = first.buyer_uid,
                            seller_uid = first.seller_uid,
                            inv_type = first.inv_type
                        };
                    }
                } 
            }


            var search_obj = new dropdown_request_param { mst_key = "GET_STATE_LIST" };
            var StateList = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
            ViewBag.StateList = new SelectList(StateList ?? new List<dropdown_response>(), "Value", "Text");
             
            search_obj = new dropdown_request_param { mst_key = "GET_All_USER", id = 3 };
            var BuyerList = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
            ViewBag.BuyerList = new SelectList(BuyerList ?? new List<dropdown_response>(), "Value", "Text");

            search_obj = new dropdown_request_param { mst_key = "GET_All_PRODUCTS"};
            var ProductList = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
            ViewBag.ProductList = new SelectList(ProductList ?? new List<dropdown_response>(), "Value", "Text");
             
            return View(model);
        }


        [HttpPost("generate-user-invoice/{order_id?}")]
        public async Task<IActionResult> GenerateUserInvoice([FromForm] GenerateInvoiceDto req, int? order_id)
        {
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;
             
            var uid = User.FindFirst(System.Security.Claims.ClaimTypes.Sid)?.Value;
            var user_id = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            var search_obj = new dropdown_request_param { mst_key = "GET_STATE_LIST" };
            if (!ModelState.IsValid)
            {
                var allErrors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                // Optional: Join multiple errors into one message, or just take the first
                TempData["Error"] = string.Join("/n", allErrors);


                var StateList = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
                ViewBag.StateList = new SelectList(StateList ?? new List<dropdown_response>(), "Value", "Text", req.state_id);

                search_obj = new dropdown_request_param { mst_key = "GET_DISTRICT_LIST", id = Convert.ToInt32(req.state_id) };
                var DistrictList = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
                ViewBag.DistrictList = new SelectList(DistrictList ?? new List<dropdown_response>(), "Value", "Text", req.dist_id);

                search_obj = new dropdown_request_param { mst_key = "GET_All_USER", id = 3 };
                var BuyerList = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
                ViewBag.BuyerList = new SelectList(BuyerList ?? new List<dropdown_response>(), "Value", "Text", req.buyer_uid);
                
                return View(req);
            }


            req.seller_uid = Convert.ToInt32(uid);
            req.order_id = order_id;
            req.user_id = user_id; 
            req.doe=ParseDate(req.doe);

            // 🔹 save data
            db_common_res result = await _addSvc.GenarateUserInvoice(req);
            if (result.error == "0")
            { 
                var invoice_no = result.json_result;
                int inv_id = 0;
                var seller_mobile = "";

                // Send WhatsApp Message start
                var req_obj = new guest_report_request_param { mst_key = "GET_INVOICE_DETAILS", invoice_no = invoice_no };
                db_common_res json = await _getSvc.master_reports(req_obj);
                if (!string.IsNullOrWhiteSpace(json.json_result))
                {
                    var first = JsonConvert.DeserializeObject<List<InvoiceDetails_DB>>(json.json_result)?.FirstOrDefault();
                    if (first != null)
                    {
                        inv_id = first.inv_id ?? 0;
                        seller_mobile = first.seller_mobile ?? string.Empty;
                         
                    }
                }
                // End Send WhatsApp Message


                TempData["Success"] =
                "✅ Your invoice has been generated. <br> Your invoice number is : " + invoice_no +
                "<br> <br> <br> <a href='" + Const.URL_INVOICE + inv_id.ToString() + "' target='_blank' style='color:#0d6efd; font-weight:bold;'>🖨 Print Invoice</a>" +
                " | <a href='" + Const.URL_INVOICE + inv_id.ToString() + "' onclick=\"navigator.clipboard.writeText(this.href); alert('Link copied!'); return false;\" style='color:#6f42c1; font-weight:bold;'>📋 Copy Link</a>";

                return RedirectToAction("generate-user-invoice", "billing");
            }
            TempData["Error"] = result.json_result;

            search_obj = new dropdown_request_param { mst_key = "GET_STATE_LIST" };
            var StateList_2 = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
            ViewBag.StateList = new SelectList(StateList_2 ?? new List<dropdown_response>(), "Value", "Text", req.state_id);

            search_obj = new dropdown_request_param { mst_key = "GET_DISTRICT_LIST", id = Convert.ToInt32(req.state_id) };
            var DistrictList_2 = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
            ViewBag.DistrictList = new SelectList(DistrictList_2 ?? new List<dropdown_response>(), "Value", "Text", req.dist_id);


            search_obj = new dropdown_request_param { mst_key = "GET_All_USER", id = 3 };
            var BuyerList_2 = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
            ViewBag.BuyerList = new SelectList(BuyerList_2 ?? new List<dropdown_response>(), "Value", "Text", req.buyer_uid);
 
            return View(req);
        }
        #endregion



        #region FranchiseCancelInvoice
        [HttpGet("fran-cancel-invoice")]
        public async Task<IActionResult> FranCancelInvoice(int? inv_id)
        {
            if (RoleWiseLogin("admin_sub_role") is IActionResult rolecheck) return rolecheck;
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;
            try
            {
                var req = new CancelInvoiceFilter() {
                    inv_id = inv_id,
                    login_user_id = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                };
                db_common_res result = await _addSvc.FranCancelInvoice(req);
                return Ok(new { success = result.error == "0", token_valid = true, data = "", message = result.json_result });
            }
            catch { return StatusCode(500, new { success = false, token_valid = true, data = "", total_count = 0, message = "Internal server error" }); }
        }
        #endregion



        #region UserCancelInvoice
        [HttpGet("user-cancel-invoice")]
        public async Task<IActionResult> UserCancelInvoice(int? inv_id)
        {
            if (RoleWiseLogin("admin_sub_role") is IActionResult rolecheck) return rolecheck;
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;
            try
            {
                var req = new CancelInvoiceFilter()
                {
                    inv_id = inv_id,
                    login_user_id = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                };
                db_common_res result = await _addSvc.UserCancelInvoice(req);
                return Ok(new { success = result.error == "0", token_valid = true, data = "", message = result.json_result });
            }
            catch { return StatusCode(500, new { success = false, token_valid = true, data = "", total_count = 0, message = "Internal server error" }); }
        }
        #endregion



        #region Commision-history-And-Dispatch


        [HttpGet("get-commision-history/{inv_id?}")]
        public async Task<IActionResult> GetCommisionHistory(int? inv_id)
        {
            if (RoleWiseLogin("admin_sub_role") is IActionResult rolecheck) return rolecheck;
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;
            try
            {
                var req = new InvoiceIdRequestModel() { inv_id = inv_id };
                db_common_res result = await _getSvc.GetInvoiceProductsByOrderId(req);
                return Ok(new { success = result.error == "0", token_valid = true, data = result.json_result, total_count = 0, message = "" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, token_valid = true, data = "", total_count = 0, message = "Internal server error" });
            }
        }



        [HttpGet("invoice-update/{inv_id?}")]
        public async Task<IActionResult> InvoiceUpdate(int? inv_id = 0)
        {
            if (RoleWiseLogin("admin_sub_role") is IActionResult rolecheck) return rolecheck;
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;

            var model = new UpdateInvoiceDto();
            var uid = User.FindFirst(System.Security.Claims.ClaimTypes.Sid)?.Value;

            if (inv_id > 0)
            {
                var req = new InvoiceIdRequestModel { inv_id = inv_id };
                db_common_res result = await _getSvc.GetInvoiceBuyerSellerDetail(req);
                if (!string.IsNullOrWhiteSpace(result?.json_result))
                {
                    // Deserialize as a list
                    var list = JsonConvert.DeserializeObject<List<UpdateInvoiceDto>>(result.json_result);
                    var first = list?.FirstOrDefault(); // Pick the first item safely
                    if (first != null)
                    {
                        model = new UpdateInvoiceDto()
                        {
                            inv_id = first.inv_id,
                            invoice_no = first.invoice_no,
                            t_amount = first.t_amount,
                            receive_amount = first.receive_amount,
                            commission = first.commission,
                            doe = first.doe,

                            delivery_status = first.delivery_status,
                            transport = first.transport,
                            tracking = first.tracking,
                            dispatch_date = first.dispatch_date,
                            del_by = first.del_by,

                            lr_1 = first.lr_1,
                            lr_2 = first.lr_2,

                            remark = first.remark 
                        };
                    }
                }
            }

            return View(model);
        }


        [HttpPost("invoice-update/{inv_id?}")]
        public async Task<IActionResult> InvoiceUpdate([FromForm] UpdateInvoiceDto req, int? inv_id = 0)
        {
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;

            var uid = User.FindFirst(System.Security.Claims.ClaimTypes.Sid)?.Value;
            req.del_by = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            db_common_res result = await _addSvc.DispatchedProduct(req);
            if (result.error == "0")
            {
                TempData["Success"] = "✅ Your details have been saved successfully.";

                if (req.delivery_status == 1)
                {
                    // Send WhatsApp Message start
                    var req_obj = new guest_report_request_param { mst_key = "GET_INVOICE_DETAILS", invoice_no = req.invoice_no };
                    db_common_res json = await _getSvc.master_reports(req_obj);
                    if (!string.IsNullOrWhiteSpace(json.json_result))
                    {
                        var first = JsonConvert.DeserializeObject<List<InvoiceDetails_DB>>(json.json_result)?.FirstOrDefault();
                        if (first != null)
                        {
                            string queryString = string.Join("&", new[]
                            {
                            ("divine_name", first.name ?? string.Empty),
                            ("divine_date", first.doe ?? string.Empty),
                            ("divine_courier_receipt_no", first.transport ?? string.Empty),
                            ("divine_courier_name", first.tracking ?? string.Empty),
                            ("divine_lr_copy", Const.BASE_URL + first.lr_1),
                            ("divine_url", Const.URL_INVOICE + first.inv_id),
                            ("divine_whatsapp_no", "91" + first.mobile)
                        }.Select(kvp => $"{kvp.Item1}={kvp.Item2}"));

                            var sent_result = await new WhatsAppService().SendWhatsAppAsync($"{Const.WA_URL_DISPATCH}?{queryString}");
                        }
                    }
                    // Send WhatsApp Message End
                }
                 
            }
            TempData["Error"] = result.json_result;

            if (inv_id > 0)
            {
                var req_1 = new InvoiceIdRequestModel { inv_id = inv_id };
                db_common_res result_1 = await _getSvc.GetInvoiceBuyerSellerDetail(req_1);
                if (!string.IsNullOrWhiteSpace(result_1?.json_result))
                {
                    // Deserialize as a list
                    var list = JsonConvert.DeserializeObject<List<UpdateInvoiceDto>>(result_1.json_result);
                    var first = list?.FirstOrDefault(); // Pick the first item safely
                    if (first != null)
                    {
                        req = new UpdateInvoiceDto()
                        {
                            inv_id = first.inv_id,
                            invoice_no = first.invoice_no,
                            t_amount = first.t_amount,
                            receive_amount = first.receive_amount,
                            commission = first.commission,
                            doe = first.doe,

                            delivery_status = first.delivery_status,
                            transport = first.transport,
                            tracking = first.tracking,
                            dispatch_date = first.dispatch_date,
                            del_by = first.del_by,

                            lr_1 = first.lr_1,
                            lr_2 = first.lr_2,

                            remark = first.remark
                        };
                    }
                }
            }

            return View(req);
        }
        #endregion



        #region Receive Payment  

        [HttpGet("party-ledger")]
        public async Task<IActionResult> PartyLedger()
        {
            if (RoleWiseLogin("admin_sub_role") is IActionResult rolecheck) return rolecheck;
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;

            var uid = User.FindFirst(System.Security.Claims.ClaimTypes.Sid)?.Value;

            var search_obj = new dropdown_request_param { mst_key = "GET_All_USER", id = Convert.ToInt32(uid) };
            var BuyerList = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
            ViewBag.BuyerList = new SelectList(BuyerList ?? new List<dropdown_response>(), "Value", "Text");

            return View();
        }

        [HttpGet("add-partial-payment")]
        public async Task<IActionResult> GetPartialPayment(int? buyer_uid, int? voucher_id,
            decimal? receive_amount, string? particulars, string? voucher_no, string? doe)
        {
            if (RoleWiseLogin("admin_sub_role") is IActionResult rolecheck) return rolecheck;
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;
            try
            { 
                var req = new ReceivePaymentDto()
                {
                    buyer_uid = buyer_uid,
                    voucher_id = voucher_id,
                    receive_amount = receive_amount,
                    particulars = particulars,
                    voucher_no = voucher_no,
                    doe = ParseDate(doe),
                    login_user_id = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value,
                };

                db_common_res result = await _addSvc.ReceivedPayment(req);
                if (result.error == "0" && voucher_id == 12)
                {

                    // Send WhatsApp Message start
                    var req_obj = new guest_report_request_param { 
                        mst_key = "GET_BASIC_USER_DATA", 
                        id = buyer_uid,
                        user_id = null
                    };
                    db_common_res json = await _getSvc.master_reports(req_obj);
                    if (!string.IsNullOrWhiteSpace(json.json_result))
                    {
                        var first = JsonConvert.DeserializeObject<List<UserDetailsFromDB>>(json.json_result)?.FirstOrDefault();
                        if (first != null)
                        {
                            string company_due = decimal.TryParse(first.company_due, out var val) && val > 0 ? first.company_due : "0";

                            string queryString = string.Join("&", new[]
                            {
                                ("divine_name", first.name ?? string.Empty),
                                ("divine_amount",  receive_amount.ToString() ?? "0"),
                                ("divine_balance", company_due),
                                ("divine_whatsapp_no", "91" + first.mobile)
                            }.Select(kvp => $"{kvp.Item1}={kvp.Item2}"));

                            var sent_result = await new WhatsAppService().SendWhatsAppAsync($"{Const.WA_URL_PAYMENT_RECEIPT}?{queryString}");
                        }
                    }
                    // Send WhatsApp Message End 
                }
                 
                return Ok(new { success = result.error == "0", token_valid = true, data = "", message = result.json_result });
            }
            catch (Exception er) { return Ok(new { success = false, token_valid = true, data = "", message = "Internal server error" }); }
        }



        [HttpGet("get-payment-history")]
        public async Task<IActionResult> GetPaymentHistory(int? buyer_uid, string? from_date, string? to_date)
        {
            if (RoleWiseLogin("admin_sub_role") is IActionResult rolecheck) return rolecheck;
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;
            try
            {
                var req = new buyer_uid_filter() { buyer_uid = buyer_uid };
                db_common_res result = await _getSvc.GetPaymentHistory(req);
                return Ok(new { success = result.error == "0", token_valid = true, data = result.json_result, total_count = 0, message = "" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, token_valid = true, data = "", total_count = 0, message = "Internal server error" });
            }
        }
        #endregion



        #region FranchiseWalletTransaction
        [HttpGet("franchise-wallet-transaction")]
        public async Task<IActionResult> FranchiseWalletTransaction()
        {
            if (CheckAndLogout() is IActionResult logincheck)
                return logincheck;

            return View();
        }


        [HttpPost("get-wallet-transaction")]
        public async Task<IActionResult> GetWalletTransaction([FromBody] WalletTransactionFilter req)
        {
            if (CheckAndLogout() is IActionResult logout) return logout;
            try
            {
                var result = await _getSvc.FranWalletTransactionList(req);
                return Ok(new { success = true, token_valid = true, data = result, message = "" });
            }
            catch { return StatusCode(500, new { success = false, token_valid = true, data = "", message = "Internal server error" }); }
        }
        #endregion



        #region PartyWiseOutstanding

        [HttpGet("party-wise-outstanding")]
        public async Task<IActionResult> PartyWiseOutstanding()
        { 
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;

            var uid = User.FindFirst(System.Security.Claims.ClaimTypes.Sid)?.Value;

            var search_obj = new dropdown_request_param { mst_key = "GET_All_USER", id = Convert.ToInt32(uid) };
            var SallerList = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
            ViewBag.SallerList = new SelectList(SallerList ?? new List<dropdown_response>(), "Value", "Text", uid);
            
            return View();
        }



        [HttpGet("get-party-wise-outstanding")]
        public async Task<IActionResult> GetPartyWiseOutstanding(string from_date, string to_date, int? seller_uid)
        { 
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;
            try
            { 
                var req = new PartyWiseOutstandingFilter() {
                    from_date = from_date,
                    to_date = to_date,
                    uid = Convert.ToInt32(seller_uid)
                }; 
                var result = await _getSvc.PartyWiseOutstanding(req);
                return Ok(new { success = true, token_valid = true, data = result, message = "" });
            }
            catch { return StatusCode(500, new { success = false, token_valid = true, data = "", total_count = 0, message = "Internal server error" }); }
        }
        #endregion
 

        #region Profit&Loss-Report

        [HttpGet("profit-loss")]
        public async Task<IActionResult> ProfitLossReport()
        {
            if (RoleWiseLogin("admin_sub_role") is IActionResult rolecheck) return rolecheck;
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;

            var uid = User.FindFirst(System.Security.Claims.ClaimTypes.Sid)?.Value;

            var search_obj = new dropdown_request_param { mst_key = "GET_All_USER", id = Convert.ToInt32(uid) };
            var SallerList = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
            ViewBag.SallerList = new SelectList(SallerList ?? new List<dropdown_response>(), "Value", "Text", uid);


            return View();
        }


        [HttpGet("get-profit-loss")]
        public async Task<IActionResult> GetProfitLossReport(string? from_date, string? to_date, int? seller_uid)
        {
            if (RoleWiseLogin("admin_sub_role") is IActionResult rolecheck) return rolecheck;
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;
            try
            {
                var req = new ProfitLossFilter()
                {
                    from_date = from_date,
                    to_date = to_date,
                    uid = seller_uid
                };
                
                var result = await _getSvc.GetProfitLossReport(req); 
                var flatList = result.Select(r =>
                {
                    var flat = new Dictionary<string, object>
                    {
                        ["name"] = r.name
                    };

                    foreach (var kvp in r.month_qty)
                    {
                        flat[kvp.Key] = kvp.Value;
                    }

                    return flat;
                }).ToList(); // To materialize the list
                return Ok(new { success = true, token_valid = true, data = result });
            }
            catch
            {
                return StatusCode(500, new { success = false, token_valid = true, message = "Internal server error" });
            }
        }


        [HttpGet("profit-loss-delete")]
        public async Task<IActionResult> ProfitLossDelete(int? eid)
        {
            if (RoleWiseLogin("admin_sub_role") is IActionResult rolecheck) return rolecheck;
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;
            try
            {
                var req = new ExpensesFilter()
                {
                    eid = eid 
                };
                db_common_res result = await _addSvc.ProfitLossDelete(req);
                return Ok(new { success = result.error == "0", token_valid = true, data = "", message = result.json_result });
            }
            catch { return StatusCode(500, new { success = false, token_valid = true, data = "", total_count = 0, message = "Internal server error" }); }
        }

        #endregion





        #region Order Tracking 

        [HttpGet("order-tracking")]
        public async Task<IActionResult> OrderTracking()
        {
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;
            return View();
        }

        
        [HttpGet("get-order-tracking")]
        public async Task<IActionResult> GetOrderTracking(int? uid, string? from_date, string? to_date)
        {
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;
            return StatusCode(500, new { success = false, token_valid = true, data = "", total_count = 0, message = "Internal server error" });
        }
        #endregion

    }
}
