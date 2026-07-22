using AppModels;
using AppServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Security.Claims;
using Microsoft.Extensions.Caching.Memory;
namespace AppWeb.Controllers
{
    [Route("vendor"), Authorize]
    public class VendorController : BaseController
    {
        #region Constructors  
        private readonly IAddServices _addSvc;
        private readonly IGetServices _getSvc;
        public VendorController(IConfiguration config, IAddServices addServices,
            IGetServices getSvc, IMemoryCache cache) : base(config, addServices, getSvc, cache)
        {
            _addSvc = addServices;
            _getSvc = getSvc;
        }
        #endregion

        
        #region vendor  
        [HttpGet("vendor-list")]
        public async Task<IActionResult> VendorList()
        {
            if (RoleWiseLogin("admin_role") is IActionResult rolecheck) return rolecheck;
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;

            return View();
        }


        [HttpGet("vendor-add/{id?}")]
        public async Task<IActionResult> VendorAdd(int? id = 0)
        {
            if (RoleWiseLogin("admin_role") is IActionResult rolecheck) return rolecheck;
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;

            var model = new VendorDto();
            if (id > 0)
            {
                guest_report_request_param req_obj = new guest_report_request_param();
                req_obj.mst_key = "GET_VENDOR_LIST";
                req_obj.id = id;
                db_common_res json = await _getSvc.master_reports(req_obj);
                if (json?.json_result != null)
                {
                    var array_data = JsonConvert.DeserializeObject<List<Vendor_db>>(json?.json_result);
                    var first = array_data.FirstOrDefault();

                    //var first = JsonConvert.DeserializeObject<Vendor_db>(json?.json_result);
                    if (first != null)
                    {
                        model = new VendorDto()
                        {
                            vendor_id = first.vendor_id,
                            vendor_name = first.vendor_name,
                            display_name = first.display_name,
                            email_id = first.email_id,
                            phone_no = first.phone_no,
                            mobile_id = first.mobile_id,
                            //source_supply = first.source_supply,
                            state_id = first.state_id,
                            skype_name = first.skype_name,
                            designation = first.designation,
                            department = first.department,
                            website = first.website,
                            status = first.status
                        };

                    }
                }
            }

            var search_obj = new dropdown_request_param { mst_key = "GET_STATE_LIST" };
            var StateList = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
            ViewBag.StateList = new SelectList(StateList ?? new List<dropdown_response>(), "Value", "Text", model.state_id);

            return View(model);
        }


        [HttpPost("vendor-add/{id?}")]
        public async Task<IActionResult> VendorAdd(VendorDto req)
        {
            if (RoleWiseLogin("admin_role") is IActionResult rolecheck) return rolecheck;
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;

            var search_obj = new dropdown_request_param { mst_key = "GET_STATE_LIST" };
            if (!ModelState.IsValid)
            {
                var StateList = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
                ViewBag.StateList = new SelectList(StateList ?? new List<dropdown_response>(), "Value", "Text", req.state_id);
                return View(req);
            }
            db_common_res result = await _addSvc.AddVendor(req);
            if (result.error == "0")
            {
                if (req.vendor_id == null)
                    TempData["Success"] = "✅ Your details have been saved successfully.";
                else
                    TempData["Success"] = "✅ Your details have been updated successfully.";

                return RedirectToAction("vendor-list", "vendor");
            }
            TempData["Error"] = result.json_result;


            search_obj = new dropdown_request_param { mst_key = "GET_STATE_LIST" };
            var StateList_2 = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
            ViewBag.StateList = new SelectList(StateList_2 ?? new List<dropdown_response>(), "Value", "Text", req.state_id);
            return View(new VendorDto());
        }
        #endregion


        #region generate-vendor-po
        [HttpGet("generate-vendor-po")]
        public async Task<IActionResult> GenarateVendorPO()
        {
            if (RoleWiseLogin("admin_role") is IActionResult rolecheck) return rolecheck;
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;

            var model = new AdminPurchaseDto();

            var search_obj = new dropdown_request_param { mst_key = "GET_VENDOR_LIST" };
            var VendorList = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
            ViewBag.VendorList = new SelectList(VendorList ?? new List<dropdown_response>(), "Value", "Text");

            search_obj = new dropdown_request_param { mst_key = "GET_VENDOR_PURCHASE_PRODUCT_LIST" };
            var ProductList = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
            ViewBag.ProductList = new SelectList(ProductList ?? new List<dropdown_response>(), "Value", "Text");

            return View(model);
        }


        [HttpPost("generate-vendor-po")]
        public async Task<IActionResult> GenarateVendorPO([FromForm] AdminPurchaseDto req)
        {
            if (RoleWiseLogin("admin_role") is IActionResult rolecheck) return rolecheck;
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;


            var model = new AdminPurchaseDto();
            var search_obj = new dropdown_request_param { mst_key = "GET_VENDOR_LIST" };
            if (!ModelState.IsValid)
            {
                var VendorList = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
                ViewBag.VendorList = new SelectList(VendorList ?? new List<dropdown_response>(), "Value", "Text");

                search_obj = new dropdown_request_param { mst_key = "GET_VENDOR_PURCHASE_PRODUCT_LIST" };
                var ProductList = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
                ViewBag.ProductList = new SelectList(ProductList ?? new List<dropdown_response>(), "Value", "Text");

                return View(req);
            }
            var uid = User.FindFirst(ClaimTypes.Sid)?.Value;
            req.cart_id = Const.VENDOR_PURCHASE_ORDER;
            req.buyer_uid = int.Parse(uid);

            req.delivery_date = ParseDate(req.delivery_date);
            req.doe = ParseDate(req.doe);

            db_common_res result = await _addSvc.GenarateVendorPO(req);
            if (result.error == "0")
            {
                model.invoice_no = string.Empty;
                model.vendor_id = 0;
                TempData["Success"] = "✅ Your purchase order has been generated. Your order number is : " + result.json_result;
            }
            else
            {
                TempData["Error"] = result.json_result;
            }
            search_obj = new dropdown_request_param { mst_key = "GET_VENDOR_LIST" };
            var VendorList_2 = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
            ViewBag.VendorList = new SelectList(VendorList_2 ?? new List<dropdown_response>(), "Value", "Text");

            search_obj = new dropdown_request_param { mst_key = "GET_VENDOR_PURCHASE_PRODUCT_LIST" };
            var ProductList_2 = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
            ViewBag.ProductList = new SelectList(ProductList_2 ?? new List<dropdown_response>(), "Value", "Text");

            return View(model);
        }
        #endregion


        #region vendor-purchase-order-list
        [HttpGet("vendor-po-list")]
        public async Task<IActionResult> VendorPOList()
        {
            if (RoleWiseLogin("admin_role") is IActionResult rolecheck) return rolecheck;
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;

            var model = new AdminPurchaseDto();

            var search_obj = new dropdown_request_param { mst_key = "GET_VENDOR_LIST" };
            var VendorList = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
            ViewBag.VendorList = new SelectList(VendorList ?? new List<dropdown_response>(), "Value", "Text");
             
            return View();
        }


        [HttpPost("get-vendor-po-list")]
        public async Task<IActionResult> GetVendorPOList([FromBody] AdminVendorPurchaseFilter req)
        {
            if (RoleWiseLogin("admin_role") is IActionResult rolecheck) return rolecheck;
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;
            try
            { 
                var result = await _getSvc.VendorPOList(req);
                return Ok(new { success = true, token_valid = true, data = result, message = "" });
            }
            catch (Exception ex) { return StatusCode(500, new { success = false, token_valid = true, data ="", total_count = 0, message = "Internal server error" }); }
        } 
        #endregion


        #region vendor-received-purchase-order
        [HttpGet("vendor-received-po/{id?}")]
        public async Task<IActionResult> VendorReceivePO(int? id = 0)
        {
            if (RoleWiseLogin("admin_role") is IActionResult rolecheck) return rolecheck;
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;
  
            var model = new AdminPurchaseReceiveDto();
            model.purchase_id = id;

            var search_obj = new dropdown_request_param { mst_key = "GET_VENDOR_PO" };
            var VendorPOList = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
            ViewBag.VendorPOList = new SelectList(VendorPOList ?? new List<dropdown_response>(), "Value", "Text", model.purchase_id);
 
            return View(model);
        }



        [HttpPost("vendor-received-po/{id?}")]
        public async Task<IActionResult> VendorReceivePO(int? id, [FromForm] AdminPurchaseReceiveDto req)
        {
            if (RoleWiseLogin("admin_role") is IActionResult rolecheck) return rolecheck;
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;

            var model = new AdminPurchaseReceiveDto();
            var search_obj = new dropdown_request_param { mst_key = "GET_VENDOR_PO" };
            if (!ModelState.IsValid)
            { 
                var VendorPOList = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
                ViewBag.VendorPOList = new SelectList(VendorPOList ?? new List<dropdown_response>(), "Value", "Text", req.purchase_id);
                return View(req);
            }
            var uid = User.FindFirst(ClaimTypes.Sid)?.Value;
            req.buyer_uid = int.Parse(uid);
            req.vendor_inv_date = ParseDate(req.vendor_inv_date.ToString());

            db_common_res result = await _addSvc.ReceivePO_FromVendor(req);
            if (result.error == "0")
            { 
                TempData["Success"] = "✅ Your purchase order has been generated. Your order number is : " + result.json_result;
                return RedirectToAction("vendor-po-list", "vendor");
                //  <li><a asp-controller="vendor" asp-action="vendor-po-list">Purchase Order List</a></li>
            }
            else
            {
                TempData["Error"] = result.json_result;
            }
  
            var VendorPOList_2 = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
            ViewBag.VendorPOList = new SelectList(VendorPOList_2 ?? new List<dropdown_response>(), "Value", "Text", req.purchase_id);
            return View(req);
        }
        #endregion

    }
}
