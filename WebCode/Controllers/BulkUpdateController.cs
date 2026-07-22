using AppModels;
using AppServices;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Data;
using Microsoft.Extensions.Caching.Memory;
namespace AppWeb.Controllers
{
    [Route("bulk"), Authorize]
    public class BulkUpdateController : BaseController
    {

        #region Constructors  
        private readonly IAddServices _addSvc;
        private readonly IGetServices _getSvc;
        private readonly IDapperContext _db;
        public BulkUpdateController(IDapperContext dapperContext, IConfiguration config, IAddServices addServices,
            IGetServices getSvc, IMemoryCache cache) : base(config, addServices, getSvc, cache)
        {
            _addSvc = addServices;
            _getSvc = getSvc;
            _db = dapperContext;
        }
        #endregion

 

        #region SelectProductForCart 
        [HttpGet("select-product-for-cart")]
        public async Task<IActionResult> SelectProductForCart(int? cart_id = 0, int? buyer_uid = 0)
        {
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;

            int uid = Convert.ToInt32(User.FindFirst(System.Security.Claims.ClaimTypes.Sid)?.Value);

            var search_obj = new dropdown_request_param { mst_key = "GET_USER_DOWNLINE_FRANCHISE", id = uid };
            var UserList = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
            ViewBag.UserList = new SelectList(UserList ?? new List<dropdown_response>(), "Value", "Text", buyer_uid);


            if (User.FindFirst("SubRole")?.Value == "admin_sub_role")
            {
                ViewBag.CartList = new List<SelectListItem> { new() { Text = "🛒 Order Product", Value = "11", Selected = cart_id == 11 }, new() { Text = "📄 Invoice Product", Value = "10", Selected = cart_id == 10 } };
            }
            else
            {
                ViewBag.CartList = new List<SelectListItem> { new() { Text = "🛒 Order Product", Value = "11", Selected = cart_id == 11 } };
            }
            return View();
        }



        [HttpPost("select-product-for-cart")]
        public async Task<IActionResult> SelectProductForCart([FromBody] SelectProductForCart req)
        {
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;
            try
            {
                if (req == null || req.SelectProducts == null || !req.SelectProducts.Any())
                    return Ok(new { success = false, token_valid = true, data = "", message = "No products selected." });

                if (req.uid == 0)
                    return Ok(new { success = false, token_valid = true, data = "", message = "Please select party.!!" });

                // Prepare DataTable for TVP
                var dataTable = new DataTable();
                dataTable.Columns.Add("pid", typeof(int));

                foreach (var item in req.SelectProducts)
                {
                    if (item.pid.HasValue)
                        dataTable.Rows.Add(item.pid.Value);
                }

                // Set parameters
                var parameters = new DynamicParameters();
                parameters.Add("@uid", req.uid); // if required
                parameters.Add("@cart_id", req.cart_id); // if required
                parameters.Add("@SelectProducts", dataTable.AsTableValuedParameter("dbo.SelectProducts"));

                // Call stored procedure
                db_common_res result = await _db.ExecuteQueryFirstOrDefaultAsync<db_common_res>("sp_select_products_update_cart", parameters);
                if (result.error == "0")
                {
                    return Ok(new { success = true, token_valid = true, data = "", message = "" });
                }
                else
                {
                    //var search_obj = new dropdown_request_param { mst_key = "GET_PRODUCT_LIST" };
                    //var ProductList = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
                    //ViewBag.ProductList = ProductList ?? new List<dropdown_response>();

                    int uid = Convert.ToInt32(User.FindFirst(System.Security.Claims.ClaimTypes.Sid)?.Value);
                    var search_obj = new dropdown_request_param { mst_key = "GET_USER_ROLE_WISE" };
                    var UserList = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
                    ViewBag.UserList = new SelectList(UserList ?? new List<dropdown_response>(), "Value", "Text", uid);
                }
                return View(req);
            }
            catch (Exception ex) { return StatusCode(500, new { success = false, token_valid = true, message = ex.Message, data = "" }); }

        }
        #endregion

         

    }
}
