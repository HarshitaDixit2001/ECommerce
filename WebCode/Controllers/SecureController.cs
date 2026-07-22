using AppModels;
using AppServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Microsoft.Extensions.Caching.Memory;
namespace AppWeb.Controllers
{
    [Route("secure"), Authorize]
    public class SecureController : BaseController
    {
        #region Constructors  
        private readonly IAddServices _addSvc;
        private readonly IGetServices _getSvc;
        public SecureController(IConfiguration config, IAddServices addServices,
            IGetServices getSvc, IMemoryCache cache) : base(config, addServices, getSvc, cache)
        {
            _addSvc = addServices;
            _getSvc = getSvc;
        }
        #endregion


        #region Dashboard  
        [HttpGet("dashboard")]
        public async Task<IActionResult> Dashboard()
        {
            if (RoleWiseLogin("admin_role") is IActionResult rolecheck) return rolecheck;
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;

            //var uid = User.FindFirst(System.Security.Claims.ClaimTypes.Sid)?.Value;

            //var search_obj = new dropdown_request_param { mst_key = "GET_All_USER", id = Convert.ToInt32(uid) };
            //var SallerList = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
            //ViewBag.SallerList = new SelectList(SallerList ?? new List<dropdown_response>(), "Value", "Text", uid);
 
            return View();
        }


        [HttpGet("user-dashboard")]
        public async Task<IActionResult> UserDashboard()
        {
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;
             
            return View();
        }

        [HttpGet("get-user-dashboard")]
        public async Task<IActionResult> GetUserDashboard(int? uid)
        {
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;

            var req = new UserTarggetFilter()
            {
                uid = Convert.ToInt32(uid)
            };
            db_common_res result = await _getSvc.UserDashboard(req);
            return Ok(new { success = result.error == "0", token_valid = true, data = result.json_result, message = "" });
        }


        [HttpGet("sales-dashboard")]
        public async Task<IActionResult> SalesDashboard()
        {
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;

            //var uid = User.FindFirst(System.Security.Claims.ClaimTypes.Sid)?.Value;

            //var search_obj = new dropdown_request_param { mst_key = "GET_All_USER", id = Convert.ToInt32(uid) };
            //var SallerList = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
            //ViewBag.SallerList = new SelectList(SallerList ?? new List<dropdown_response>(), "Value", "Text", uid);


            return View();
        }






        [HttpGet("get-sales-dashboard")]
        public async Task<IActionResult> GetSalesDashboard(string? from_date, string? to_date, int? uid)
        {
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;

            var req = new SalesDashboardValueFilter()
            {
                uid = Convert.ToInt32(uid),
                from_date = from_date,
                to_date = to_date,
            };
            db_common_res result = await _getSvc.SalesDashboard(req);
            return Ok(new { success = result.error == "0", token_valid = true, data = result.json_result, message = "" });
        }

        #endregion


        #region Size
        [HttpGet("size")]
        public async Task<IActionResult> Size()
        {
            var check = CheckAndLogout();
            if (check != null) return check;
            return View();
        }
        #endregion


        #region batch
        [HttpGet("batch")]

        public async Task<IActionResult> Batch()
        {
            var check = CheckAndLogout();
            if (check != null) return check;
            return View();
        }
        #endregion


        #region Category
        [HttpGet("category")]
        public async Task<IActionResult> Category()
        {
            var check = CheckAndLogout();
            if (check != null) return check;
            return View();
        }
        #endregion


        #region product
        [HttpGet("product")]
        public async Task<IActionResult> ProductList()
        {
            var check = CheckAndLogout();
            if (check != null) return check;
            return View();
        }

        [HttpGet("product-add/{id?}")]
        public async Task<IActionResult> ProductAdd(int? id = 0)
        {
            var check = CheckAndLogout();
            if (check != null) return check;

            var model = new ProductView();
            if (id > 0)
            {
                guest_report_request_param req_obj = new guest_report_request_param();
                req_obj.mst_key = "GET_PRODUCT_LIST";
                req_obj.id = id;
                db_common_res json = await _getSvc.master_reports(req_obj);
                if (json?.json_result != null)
                {
                    var array_data = JsonConvert.DeserializeObject<List<ProductView>>(json?.json_result);
                    model = array_data.FirstOrDefault();
                }
            }

            var search_obj = new dropdown_request_param { mst_key = "GET_CATEGORY_LIST" };
            var catList = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
            ViewBag.CategoryList = new SelectList(catList ?? new List<dropdown_response>(), "Value", "Text", model.cat_id);

            search_obj = new dropdown_request_param { mst_key = "GET_BATCH_LIST" };
            var BatchList = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
            ViewBag.BatchList = new SelectList(BatchList ?? new List<dropdown_response>(), "Value", "Text", model.batch_id);

            // 

            search_obj = new dropdown_request_param { mst_key = "GET_LENGHT_UNIT" };
            var LenghtUnitList = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
            ViewBag.LenghtUnitList = new SelectList(LenghtUnitList ?? new List<dropdown_response>(), "Value", "Text", model.lenght_unit);

            return View(model);
        }


        [HttpPost("product-add/{id?}")]
        public async Task<IActionResult> ProductAdd([FromForm] ProductView req)
        {
            var check = CheckAndLogout();
            if (check != null) return check;

            var search_obj = new dropdown_request_param { mst_key = "GET_CATEGORY_LIST" };
            if (!ModelState.IsValid)
            {
                var allErrors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                TempData["Error"] = string.Join("/n", allErrors);

                var catList = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
                ViewBag.CategoryList = new SelectList(catList ?? new List<dropdown_response>(), "Value", "Text", req.cat_id);

                search_obj = new dropdown_request_param { mst_key = "GET_BATCH_LIST" };
                var BatchList = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
                ViewBag.BatchList = new SelectList(BatchList ?? new List<dropdown_response>(), "Value", "Text", req.batch_id);

                search_obj = new dropdown_request_param { mst_key = "GET_LENGHT_UNIT" };
                var LenghtUnitList = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
                ViewBag.LenghtUnitList = new SelectList(LenghtUnitList ?? new List<dropdown_response>(), "Value", "Text", req.lenght_unit);

                return View(req);
            }

            // 🔹  save data
            db_common_res result = await _addSvc.AddProduct(req);
            if (result.error == "0")
            {
                if (req.pid == null)
                    TempData["Success"] = "✅ Your details have been saved successfully.";
                else
                    TempData["Success"] = "✅ Your details have been updated successfully.";

                return RedirectToAction("product", "secure");
            }
            TempData["Error"] = result.json_result;

            search_obj = new dropdown_request_param { mst_key = "GET_CATEGORY_LIST" };
            var catList_2 = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
            ViewBag.CategoryList = new SelectList(catList_2 ?? new List<dropdown_response>(), "Value", "Text", req.cat_id);

            search_obj = new dropdown_request_param { mst_key = "GET_BATCH_LIST" };
            var BatchList_2 = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
            ViewBag.BatchList = new SelectList(BatchList_2 ?? new List<dropdown_response>(), "Value", "Text", req.batch_id);

            search_obj = new dropdown_request_param { mst_key = "GET_LENGHT_UNIT" };
            var LenghtUnitList_2 = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
            ViewBag.LenghtUnitList = new SelectList(LenghtUnitList_2 ?? new List<dropdown_response>(), "Value", "Text", req.lenght_unit);

            return View(new ProductView());
        }



        #endregion


        #region Product Ingredient
        [HttpGet("ingredient/{pid?}")]
        public async Task<IActionResult> Ingredient()
        { 
            return View();
        }

        [HttpPost("ingredient")]  
        public async Task<IActionResult> SaveProductIngredient([FromForm] IngredientDto req)
        {
            if (CheckAndLogout() is IActionResult logout) return logout;
            try
            { 
                var result = await _addSvc.AddIngredient(req);
                return Ok(new { success = result.error == "0", message = result.json_result });
            }
            catch { return StatusCode(500, new { success = false, token_valid = true, data = "", message = "Internal server error" }); }
        }
        #endregion

        #region StockList
        [HttpGet("stock-list")]
        public async Task<IActionResult> StockList()
        {
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;
            return View();
        }



        [HttpPost("get-stock-list")]
        public async Task<IActionResult> GetStockList()
        {
            //if (RoleWiseLogin("admin_sub_role") is IActionResult rolecheck) return rolecheck;
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;
            try
            {
                var uid = User.FindFirst(System.Security.Claims.ClaimTypes.Sid)?.Value;
                var req = new StockListFilter() { uid = Convert.ToInt32(uid) };

                var result = await _getSvc.StockList(req); // You must have this in your IAddServices implementation
                return Ok(new { success = true, token_valid = true, data = result, message = "" });
            }
            catch (Exception ex) { return StatusCode(500, new { success = false, token_valid = true, data = "", message = "Internal server error" }); }
        }

        #endregion


        #region MonthWiseProductQty

        [HttpGet("month-wise-product-qty")]
        public async Task<IActionResult> MonthWiseProductQty()
        {
            var check = CheckAndLogout();
            if (check != null) return check;

            var uid = User.FindFirst(System.Security.Claims.ClaimTypes.Sid)?.Value;

            var search_obj = new dropdown_request_param { mst_key = "GET_All_USER", id = Convert.ToInt32(uid) };
            var SallerList = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
            ViewBag.SallerList = new SelectList(SallerList ?? new List<dropdown_response>(), "Value", "Text");

            return View();
        }


        [HttpGet("get-month-wise-product-qty")]
        public async Task<IActionResult> GetMonthWiseProductQty(string? from_date, string? to_date,
            int? sub_role_id, int? seller_uid, int? prod_type, int? uid)
        {
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;
            try
            {
                var req = new MonthWiseProductQtyFilter()
                {
                    from_date = from_date,
                    to_date = to_date,
                    sub_role_id = sub_role_id,
                    seller_uid = seller_uid,
                    prod_type = prod_type,
                    uid = uid
                };


                var result = await _getSvc.GetMonthlyProductQty(req);

                var flatList = result.Select(r =>
                {
                    var flat = new Dictionary<string, object>
                    {
                        ["prod_code"] = r.prod_code,
                        ["prod_name"] = r.prod_name
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
        #endregion


        #region Sales&Collection

        [HttpGet("sales-collection")]
        public async Task<IActionResult> SalesCollection()
        {
            var check = CheckAndLogout();
            if (check != null) return check;

            var uid = User.FindFirst(System.Security.Claims.ClaimTypes.Sid)?.Value;

            var search_obj = new dropdown_request_param { mst_key = "GET_All_USER", id = Convert.ToInt32(uid) };
            var SallerList = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
            ViewBag.SallerList = new SelectList(SallerList ?? new List<dropdown_response>(), "Value", "Text");

            return View();
        }


        [HttpGet("get-sales-collection")]
        public async Task<IActionResult> GetSalesCollection(string? from_date, string? to_date,
            int? sub_role_id, int? mode_id, int? uid)
        {
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;
            try
            {
                var req = new SalesCollectionFilter()
                {
                    from_date = from_date,
                    to_date = to_date,
                    sub_role_id = sub_role_id,
                    mode_id = mode_id,
                    uid = uid
                };


                var result = await _getSvc.GetSalesCollection(req);

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


        [HttpGet("get-sales-collection-graph")]
        public async Task<IActionResult> GetSalesCollectioGraph(string? from_date, string? to_date, int? uid)
        {
            var check = CheckAndLogout();
            if (check != null) return check;

            var req = new SalesCollectionGraphFilter()
            {
                uid = Convert.ToInt32(uid),
                from_date = from_date,
                to_date = to_date,
            };
            db_common_res result = await _getSvc.GetSalesCollectionGraph(req);
            return Ok(new { success = result.error == "0", token_valid = true, data = result.json_result, message = "" });
        }




        [HttpGet("get-category-wise-graph")]
        public async Task<IActionResult> GetCategoryWiseGraph(string? from_date, string? to_date, int? uid)
        {
            var check = CheckAndLogout();
            if (check != null) return check;

            var req = new SalesCollectionGraphFilter()
            {
                uid = Convert.ToInt32(uid),
                from_date = from_date,
                to_date = to_date,
            };
            db_common_res result = await _getSvc.GetCategoryWiseGraph(req);
            return Ok(new { success = result.error == "0", token_valid = true, data = result.json_result, message = "" });
        }



        [HttpGet("get-product-wise-graph")]
        public async Task<IActionResult> GetProductWiseGraph(string? from_date, string? to_date, int? uid)
        {
            var check = CheckAndLogout();
            if (check != null) return check;

            var req = new SalesCollectionGraphFilter()
            {
                uid = Convert.ToInt32(uid),
                from_date = from_date,
                to_date = to_date,
            };
            db_common_res result = await _getSvc.GetProductWiseGraph(req);
            return Ok(new { success = result.error == "0", token_valid = true, data = result.json_result, message = "" });
        }

        #endregion


        #region Sales&Collection
        [HttpGet("sales-collection-commission")]
        public async Task<IActionResult> SalesCollectionCommission()
        {
            var check = CheckAndLogout();
            if (check != null) return check;

            var uid = User.FindFirst(System.Security.Claims.ClaimTypes.Sid)?.Value;

            var search_obj = new dropdown_request_param { mst_key = "GET_All_USER", id = Convert.ToInt32(uid) };
            var SallerList = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
            ViewBag.SallerList = new SelectList(SallerList ?? new List<dropdown_response>(), "Value", "Text");

            return View();
        }


        [HttpGet("get-sales-collection-commission")]
        public async Task<IActionResult> GetSalesCollectionCommission(string? from_date, string? to_date,
            int? sub_role_id, int? mode_id, int? uid)
        {
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;
            try
            {
                var req = new SalesCollectionCommisssionFilter()
                {
                    from_date = from_date,
                    to_date = to_date,
                    sub_role_id = sub_role_id,
                    mode_id = mode_id,
                    uid = uid
                };


                var result = await _getSvc.GetSalesCollectionCommission(req);

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
        #endregion


        #region EnquiryList
        [HttpGet("enquiry-list")]
        public async Task<IActionResult> EnquiryList()
        {
            if (RoleWiseLogin("admin_role") is IActionResult rolecheck) return rolecheck;
            return View();
        }
        #endregion

    }
}
