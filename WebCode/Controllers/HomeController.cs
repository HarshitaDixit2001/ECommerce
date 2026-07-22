using AppModels;
using AppServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;
namespace AppWeb.Controllers
{

    [Route("home")]
    public class HomeController : BaseController
    {
        #region Constructors
        private readonly IAuthServices _authServices;
        private readonly ILogger<HomeController> _logger;
        private readonly IAddServices _addSvc;
        private readonly IGetServices _getSvc;
        public HomeController(
            IConfiguration config,
            ILogger<HomeController> logger,
            IAuthServices authServices,
            IAddServices addServices,
            IGetServices getSvc, IMemoryCache cache
        ) : base(config, addServices, getSvc, cache) // ✅ Correctly pass dependencies to BaseController
        {
            _authServices = authServices;
            _logger = logger;
            _addSvc = addServices;
            _getSvc = getSvc;
        }
        #endregion


        #region Error   
        public IActionResult Error()
        {
            var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();

            if (exceptionFeature != null)
            {
                var ex = exceptionFeature.Error;

                _logger.LogError(ex, "Unhandled Exception");

                //if (_env.IsDevelopment())
                //{
                // Show full error in development
                ViewBag.ErrorMessage = ex.Message;
                ViewBag.StackTrace = ex.StackTrace;
                //}
                //else
                //{
                //    // Hide in production
                //    ViewBag.ErrorMessage = "An unexpected error occurred. Please try again later.";
                //}
            }

            return View();
        }
        #endregion


        #region Index  
        [HttpGet("/")]
        public IActionResult Index()
        {
            if (User.FindFirst("role_name")?.Value == "admin_role")
            {
                return RedirectToAction("dashboard", "Secure");
            }
            return View();
        }
        #endregion


        #region AddUser 
        [HttpGet("addUser")]
        public async Task<IActionResult> AddUser()
        {
            var search_obj = new dropdown_request_param { mst_key = "GET_STATE_LIST" };
            var StateList = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
            ViewBag.StateList = new SelectList(StateList ?? new List<dropdown_response>(), "Value", "Text");

            search_obj = new dropdown_request_param { mst_key = "GET_DISTRICT_LIST", id = 1 };
            var DistrictList = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
            ViewBag.DistrictList = new SelectList(DistrictList ?? new List<dropdown_response>(), "Value", "Text");

            search_obj = new dropdown_request_param { mst_key = "GET_BANK_LIST" };
            var BankList = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
            ViewBag.BankList = new SelectList(BankList ?? new List<dropdown_response>(), "Value", "Text");

            return View();
        }


        [HttpPost("addUser")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUser([FromForm] AddUpdateUserRequest req)
        {
            req.user_id = req.mobile;
            req.dob = ParseDate(req.dob);
            req.sub_role_name = "user_sub_role";
            req.submit_type = "ROOT";
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, message = string.Join("\n", errors) });
            }

            db_common_res result = await _addSvc.AddUser(req);
            if (result.error == "0")
            {
                return Json(new { success = true, message = result.json_result });
            }
            else
            {
                return Json(new { success = false, message = result.json_result });
            }
        }
        #endregion


        #region Login 
        [HttpGet("login")]
        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest req)
        {
            try
            {
                GetNum();
                if (!ModelState.IsValid)
                {
                    TempData["Error"] = "Error from model validation.!!";
                    return View(req);
                }
                ;

                var result = await _authServices.GetUserLogin(req);
                var successProp = result?.GetType().GetProperty("success")?.GetValue(result, null);
                if (successProp is bool success && success)
                {
                    var role_name = result?.GetType().GetProperty("role_name")?.GetValue(result)?.ToString();
                    if (role_name == "admin_role")
                    {
                        return RedirectToAction("dashboard", "Secure");
                    }
                    if (role_name == "user_role")
                    {
                        return RedirectToAction("user-dashboard", "secure");
                    }
                }
                TempData["Error"] = result?.GetType().GetProperty("message")?.GetValue(result, null)?.ToString() ?? "Login failed";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return View(req);
        }

        [HttpPost("MasterLogin")]
        public async Task<string> MasterLogin([FromBody] LoginRequest req)
        {
            try
            {
                var result = await _authServices.GetUserLogin(req);
                var successProp = result?.GetType().GetProperty("success")?.GetValue(result, null);
                if (successProp is bool success && success)
                {
                    return "1";
                }
                return result?.GetType().GetProperty("message")?.GetValue(result, null)?.ToString() ?? "Login failed";

            }
            catch (Exception ex) { return ex.Message; }
            return null;
        }
        #endregion


        #region Logout
        [HttpGet("Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("login", "home");
        }

        [HttpGet("AccessDenied")]
        public async Task<IActionResult> AccessDenied()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return View();
        }
        #endregion


        #region Contact
        [HttpGet("contact")]
        public async Task<IActionResult> Contact()
        {
            return View();
        }
        #endregion


        #region AboutUs
        [HttpGet("aboutus")]
        public async Task<IActionResult> AboutUs()
        {
            return View();
        }
        #endregion


        #region Thanks
        [HttpGet("thanks")]
        public async Task<IActionResult> Thanks(string? user_id)
        {
            return View();
        }
        #endregion


        #region Products
        //[HttpGet("products")]
        //public async Task<IActionResult> Products(int? cat_id, int? sub_cat_id, string? cat_name, string? sub_cat_name)
        //{
        //    ViewBag.cat_id = cat_id;
        //    ViewBag.sub_cat_id = sub_cat_id;
        //    ViewBag.cat_name = cat_name;
        //    ViewBag.sub_cat_name = sub_cat_name;

        //    var req = new get_root_product
        //    {
        //        seller_uid = Const.DEFAULT_SALLER,
        //        uid = 0,
        //        cat_id = cat_id,
        //        sub_cat_id = sub_cat_id,
        //        fetch_type = "ROOT"  // Can be changed to "BY_CATEGORY" or "BY_SUBCATEGORY" if needed
        //    };
        //    try
        //    {
        //        var products = await _getSvc.GetRootProductList(req) ?? new List<ProductViewModel>();
        //        return View(products);
        //    }
        //    catch (Exception ex)
        //    {
        //        return View(new List<ProductViewModel>()); // Return empty list on error
        //    }
        //}
         
        #endregion

         

        //#region Product Detail  
        //[HttpGet("proddetail")]
        //public async Task<IActionResult> ProdDetail(int? pid, int? size_id, int? color_id)
        //{

        //    get_root_product_detail req = new get_root_product_detail();
        //    req.seller_uid = Const.DEFAULT_SALLER;
        //    req.pid = pid;
        //    req.size_id = size_id;
        //    req.color_id = color_id;

        //    var model = await _getSvc.ProdDetail(req);
        //    model.ImageList = new List<string>();
        //    var images = new[] { model.img1, model.img2, model.img3, model.img4, model.img5, model.img6 };
        //    foreach (var img in images)
        //    {
        //        if (!string.IsNullOrWhiteSpace(img))
        //        {
        //            model.ImageList.Add(img.Trim());
        //        }
        //    }


        //    model.Features = new List<string>();
        //    if (model.t1 != "")
        //    {
        //        model.Features.Add(model.t1);
        //        model.Features.Add(model.d1);
        //    }
        //    if (model.t2 != "")
        //    {
        //        model.Features.Add(model.t2);
        //        model.Features.Add(model.d2);
        //    }
        //    if (model.t3 != "")
        //    {
        //        model.Features.Add(model.t3);
        //        model.Features.Add(model.d3);
        //    }
        //    if (model.t4 != "")
        //    {
        //        model.Features.Add(model.t4);
        //        model.Features.Add(model.d4);
        //    }
        //    if (model.t5 != "")
        //    {
        //        model.Features.Add(model.t5);
        //        model.Features.Add(model.d5);
        //    }
        //    return View(model);
        //}

        //#endregion


        #region Portfolio
        [HttpGet("portfolio")]
        public IActionResult Portfolio()
        {
            return View();
        }
        #endregion


        #region PortfolioDetail
        [HttpGet("portfoliodetail")]
        public IActionResult PortfolioDetail()
        {
            return View();
        }
        #endregion


        #region Blog
        [HttpGet("blog")]
        public IActionResult Blog()
        {
            return View();
        }
        #endregion


        #region News
        [HttpGet("news")]
        public IActionResult News()
        {
            return View();
        }
        #endregion
         

        #region FAQ
        [HttpGet("faq")]
        public IActionResult FAQ()
        {
            return View();
        }
        #endregion


        #region Privacy Policy
        [HttpGet("privacy-policy")]
        public IActionResult PrivacyPolicy()
        {
            return View();
        }
        #endregion


        #region Terms & Conditions
        [HttpGet("terms-conditions")]
        public IActionResult TermsConditions()
        {
            return View();
        }
        #endregion


        #region Refund Policy
        [HttpGet("refund-policy")]
        public IActionResult RefundPolicy()
        {
            return View();
        }
        #endregion


        #region Shipping Policy
        [HttpGet("shipping-policy")]
        public IActionResult ShippingPolicy()
        {
            return View();
        }
        #endregion

         
        #region EnquirySave 
        [HttpPost("enquirysave")]
        public async Task<IActionResult> EnquirySave([FromForm] EnquiryDto req)
        {
            try
            {
                var result = await _addSvc.SaveEnquiry(req);
                return Ok(new { success = result.error == "0", message = result.json_result });
            }
            catch { return StatusCode(500, new { success = false, token_valid = true, data = "", message = "Internal server error" }); }
        }
        #endregion


        #region Referral-Link
        [HttpGet("referral-link")]
        public IActionResult ReferralLink()
        {
            return View();
        }
        #endregion


        #region Refer & Earn
        [HttpGet("refer-earn")]
        public IActionResult ReferEarn()
        {
            return View();
        }
        #endregion


        #region Referral History
        [HttpGet("referral-history")]
        public IActionResult ReferralHistory()
        {
            return View();
        }
        #endregion


        #region Support Tickets
        [HttpGet("support-tickets")]
        public IActionResult SupportTickets()
        {
            return View();
        }
        #endregion


        #region Raise Complaint
        [HttpGet("raise-complaint")]
        public IActionResult RaiseComplaint()
        {
            return View();
        }
        #endregion

    }
}
