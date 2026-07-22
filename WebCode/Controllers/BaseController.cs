using AppModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims; 
using Microsoft.AspNetCore.Mvc.Rendering;
using AppServices;
using Microsoft.Extensions.Caching.Memory;
namespace AppWeb.Controllers
{
    public class BaseController : Controller
    {
         
        #region Constructors  
        private readonly IGetServices _getSvc;
        private readonly IAddServices _addSvc;
        protected readonly IConfiguration _config;
        private readonly IMemoryCache _cache;
        public BaseController(IConfiguration config, IAddServices addServices, IGetServices getSvc, IMemoryCache cache)
        {
            _config = config;
            _getSvc = getSvc;
            _addSvc = addServices;
            _cache = cache;
        }
        #endregion


        protected (bool IsValid, IActionResult Ok) TokenValidate()
        {
            if (Request == null || !Request.Headers.ContainsKey("Authorization"))
            {
                return (false, Ok(new { success = false, token_valid = false, message = "Missing HTTP request or Authorization header.", data = "" }));
            }

            var authHeader = Request.Headers["Authorization"].ToString();
            if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                return (false, Ok(new { success = false, token_valid = false, message = "Invalid Authorization header format.", data = "" }));
            }

            var token = authHeader.Replace("Bearer ", "").Trim();
            var validator = new JwtTokenValidator(_config); // You should implement this
            var isValid = validator.Validate(token);

            if (!isValid)
            {
                return (false, Ok(new { success = false, token_valid = false, message = "Invalid or expired token.", data = "" }));
            }
            return (true, null);
        }


 
        protected IActionResult? CheckAndLogout() =>
        string.IsNullOrEmpty(User.FindFirst(ClaimTypes.NameIdentifier)?.Value)
        ? Redirect(Const.LOGOUT) : null;


        protected IActionResult? RoleWiseLogin(string role) =>
        User.FindFirst("role_name")?.Value != role ? Redirect(Const.LOGOUT) : null;

      
 
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            try
            {
                // ✅ Try to get from cache
                if (!_cache.TryGetValue("CategoryList", out List<dropdown_response> categories))
                {
                    // Not in cache → fetch from DB
                    var search_obj = new dropdown_request_param { mst_key = "GET_CATEGORY_LIST" };
                    categories = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;

                    // ✅ Store in cache (e.g., 30 minutes)
                    _cache.Set("CategoryList", categories, TimeSpan.FromMinutes(1440));
                }

                ViewBag.Categories = new SelectList(categories ?? new List<dropdown_response>(), "Value", "Text");

                // Token available globally
                ViewBag.Token = User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.SerialNumber)?.Value;
            }
            catch
            {
                ViewBag.Categories = new SelectList(new List<dropdown_response>(), "Value", "Text");
            }
            GetNum();
            await next();
        }



        public static string? ParseDate(string? dateStr)
        {
            if (string.IsNullOrWhiteSpace(dateStr)) return null;

            dateStr = dateStr.Split(' ')[0]; // Removes time part if present

            return DateTime.TryParseExact(dateStr, "dd-MM-yyyy",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out var dt)
                ? dt.ToString("MM/dd/yyyy")
                : null;
        }



        protected void GetNum()
        {
            Random rnd = new Random();
            int num1 = rnd.Next(1, 10);
            int num2 = rnd.Next(1, 10);

            ViewBag.Num1 = num1;
            ViewBag.Num2 = num2;
            ViewBag.Sum = num1 + num2;
        }



    }
}
