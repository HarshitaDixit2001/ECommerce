using AppModels;
using AppServices;
using AppWeb.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;

namespace webCode.Controllers
{
    [Route("payout"), Authorize]
    public class PayoutController : BaseController
    {

        #region Constructors  
        private readonly IAddServices _addSvc;
        private readonly IGetServices _getSvc;
        public PayoutController(IConfiguration config, IAddServices addServices, IGetServices getSvc, IMemoryCache cache)
         : base(config, addServices, getSvc, cache)
        {
            _addSvc = addServices;
            _getSvc = getSvc;
        }
        #endregion




        #region Repurchase Payout
        [HttpGet("repurchase-payout")]
        public async Task<IActionResult> Repurchase_Payout()
        {
            if (RoleWiseLogin("admin_role") is IActionResult rolecheck) return rolecheck;
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;

            return View();
        }

        [HttpPost("repurchase-payout")]
        public async Task<IActionResult> Repurchase_Payout([FromForm] MakePayoutData req)
        {
            if (RoleWiseLogin("admin_role") is IActionResult rolecheck) return rolecheck;
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;

            if (!ModelState.IsValid)
            {
                var allErrors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                // Optional: Join multiple errors into one message, or just take the first
                TempData["Error"] = string.Join("/n", allErrors);
                return View(req);
            }

            req.from_date = ParseDate(req.from_date);
            req.to_date = ParseDate(req.to_date);
            // 🔹 save data
            db_common_res result = await _addSvc.GenerateMatrixPayout(req);
            if (result.error == "0")
            {
                TempData["Success"] = result.json_result;
                return RedirectToAction("repurchase-payout-dispatch", "payout");
            }
            TempData["Error"] = result.json_result;
            return View();
        }
        #endregion


        #region Repurchase Payout Period
        [HttpGet("repurchase-payout-period")]
        public async Task<IActionResult> Repurchase_Payout_Period()
        {
            if (RoleWiseLogin("admin_role") is IActionResult rolecheck) return rolecheck;
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;


            return View();
        }

        [HttpPost("get-repurchase-payout-period")]
        public async Task<IActionResult> Get_Repurchase_Payout_Period()
        {
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;
            try
            {
                var result = await _getSvc.Get_Repurchase_Payout_Period();
                return Ok(new { success = true, token_valid = true, data = result, message = "" });
            }
            catch
            {
                return Ok(new { success = false, token_valid = true, data = "", message = "" });
            }
        }
        #endregion


        #region Repurchase Payout Dispatch
        [HttpGet("repurchase-payout-dispatch")]
        public async Task<IActionResult> Repurchase_Payout_Dispatch()
        {
            if (RoleWiseLogin("admin_role") is IActionResult rolecheck) return rolecheck;
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;

            var search_obj = new dropdown_request_param { mst_key = "GET_MATRIX_PAYOUT" };
            var MatrixPayoutList = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
            var maxPayoutNo = MatrixPayoutList?.Max(x => Convert.ToInt32(x.Value));

            ViewBag.MatrixPayoutList = new SelectList(
                MatrixPayoutList ?? new List<dropdown_response>(),
                "Value",
                "Text",
                maxPayoutNo // ✅ pre-select last payout no
            );

            return View();
        }


        [HttpPost("get-repurchase-payout-dispatch")]
        public async Task<IActionResult> Get_Repurchase_Payout_Dispatch([FromBody] MatrixPayoutDispatchListFilter req)
        {
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;
            try
            {
                var result = await _getSvc.Repurchase_Payout_Dispatch(req);
                return Ok(new { success = true, token_valid = true, data = result, message = "" });
            }
            catch
            {
                return Ok(new { success = false, token_valid = true, data = "", message = "" });
            }
        }
        #endregion


        #region Repurchase Payout Dispatch User
        [HttpGet("repurchase-payout-dispatch-user")]
        public async Task<IActionResult> Repurchase_Payout_Dispatch_User()
        {
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;

            return View();
        }
        #endregion



        #region UserIncomeList
        [HttpGet("user-income-list")]
        public async Task<IActionResult> UserIncomeList()
        {
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;

            var uid = User.FindFirst(ClaimTypes.Sid)?.Value;

            var search_obj = new dropdown_request_param { mst_key = "GET_All_USER", id = 3 };
            var SallerList = (await _getSvc.master_dropdown(search_obj) as List<dropdown_response>)?
                                .Where(x => x.Value != Convert.ToInt32(uid)).ToList();
            ViewBag.SallerList = new SelectList(SallerList ?? new List<dropdown_response>(), "Value", "Text");

            return View();
        }


        [HttpPost("get-user-income-list")]
        public async Task<IActionResult> GetUserIncomeList([FromBody] UserIncomeListFilter req)
        {
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;
            try
            {
                var result = await _getSvc.UserIncomeList(req);
                return Ok(new { success = true, token_valid = true, data = result, message = "" });
            }
            catch
            {
                return Ok(new { success = false, token_valid = true, data = "", message = "" });
            }
        }
        #endregion
 
    }
}
