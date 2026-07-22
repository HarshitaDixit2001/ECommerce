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
    [Route("wallet"), Authorize]
    public class WalletController : BaseController
    {

        #region Constructors  
        private readonly IAddServices _addSvc;
        private readonly IGetServices _getSvc;
        public WalletController(IConfiguration config, IAddServices addServices, IGetServices getSvc, IMemoryCache cache)
         : base(config, addServices, getSvc, cache)
        {
            _addSvc = addServices;
            _getSvc = getSvc;
        }
        #endregion


        #region  Company Wallet Transaction
        [HttpGet("admin-company-wallet-tran")]
        public async Task<IActionResult> AdminCompanyWalletTran()
        {
            if (RoleWiseLogin("admin_role") is IActionResult rolecheck) return rolecheck;
            return View();
        }


        [HttpGet("user-company-wallet-tran")]
        public async Task<IActionResult> User_Company_Wallet_Tran()
        {
            if (CheckAndLogout() is IActionResult logout) return logout;
            return View();
        }


        [HttpPost("get-user-company-wallet-passbook")]
        public async Task<IActionResult> GetUserCompanyWalletPassbook([FromForm] UserWalletPassbookFilter req)
        {
            if (CheckAndLogout() is IActionResult logout) return logout;
            try
            {
                var result = await _getSvc.UserCompanyWalletPassbook(req);
                return Ok(new { success = true, token_valid = true, data = result, message = "" });
            }
            catch { return StatusCode(500, new { success = false, token_valid = true, data = "", message = "Internal server error" }); }
        }
        #endregion


        #region Payout Wallet Transaction

        [HttpGet("admin-payout-wallet-tran")]
        public async Task<IActionResult> AdminPayoutWalletTran()
        {
            if (RoleWiseLogin("admin_role") is IActionResult rolecheck) return rolecheck;
            return View();
        }

        [HttpGet("user-payout-wallet-tran")]
        public async Task<IActionResult> User_Payout_Wallet_Tran()
        {
            if (CheckAndLogout() is IActionResult logout) return logout;
            return View();
        }

        [HttpPost("get-user-payout-wallet-passbook")]
        public async Task<IActionResult> GetUserPayoutWalletPassbook([FromForm] UserPayoutWalletPassbookFilter req)
        {
            if (CheckAndLogout() is IActionResult logout) return logout;
            try
            {
                var result = await _getSvc.UserPayoutWalletPassbook(req);
                return Ok(new { success = true, token_valid = true, data = result, message = "" });
            }
            catch { return StatusCode(500, new { success = false, token_valid = true, data = "", message = "Internal server error" }); }
        }


        #endregion


        #region user Company Wallet Request  
        [HttpGet("user-company-wallet-request")]
        public async Task<IActionResult> UserCompanyWalletRequest()
        {
            if (CheckAndLogout() is IActionResult logout) return logout;

            var search_obj = new dropdown_request_param { mst_key = "GET_BANK_LIST" };
            var BankList = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
            ViewBag.BankList = new SelectList(BankList ?? new List<dropdown_response>(), "Value", "Text");

            search_obj = new dropdown_request_param { mst_key = "GET_ADMIN_BANK_LIST" };
            var AdminBankList = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
            ViewBag.AdminBankList = new SelectList(AdminBankList ?? new List<dropdown_response>(), "Value", "Text");

            return View();
        }


        [HttpPost("user-company-wallet-request")]
        public async Task<IActionResult> UserCompanyWalletRequest([FromForm] UserCompanyWalletRequestDto req)
        {
            if (req.status == 1)
            {
                if (RoleWiseLogin("admin_role") is IActionResult rolecheck) return rolecheck;
            }
            if (CheckAndLogout() is IActionResult logout) return logout;
            try
            {
                var result = await _addSvc.AddUpdateUserCompanyWalletRequest(req);
                return Ok(new { success = result.error == "0", message = result.json_result });
            }
            catch { return StatusCode(500, new { success = false, token_valid = true, data = "", message = "Internal server error" }); }
        }


        [HttpPost("get-user-company-wallet-request-list")]
        public async Task<IActionResult> UserCompanyWalletRequestList([FromForm] UserCompanyWalletRequestListFilter req)
        {
            if (CheckAndLogout() is IActionResult logout) return logout;
            try
            {
                var result = await _getSvc.GetUserCompanyWalletRequest(req);
                return Ok(new { success = true, token_valid = true, data = result, message = "" });
            }
            catch { return StatusCode(500, new { success = false, token_valid = true, data = "", message = "Internal server error" }); }
        }

        #endregion


        #region Admin Company Wallet Request  
        [HttpGet("admin-company-wallet-request")]
        public async Task<IActionResult> AdminCompanyWalletRequest()
        {
            if (RoleWiseLogin("admin_role") is IActionResult rolecheck) return rolecheck;
            return View();
        }


        [HttpPost("admin-company-wallet-request")]
        public async Task<IActionResult> AdminCompanyWalletRequest([FromForm] UserCompanyWalletRequestDto req)
        {
            if (CheckAndLogout() is IActionResult logout) return logout;
            try
            {
                var result = await _addSvc.AddUpdateUserCompanyWalletRequest(req);
                return Ok(new { success = result.error == "0", message = result.json_result });
            }
            catch { return StatusCode(500, new { success = false, token_valid = true, data = "", message = "Internal server error" }); }
        }
        #endregion


        #region Cashback History  
        [HttpGet("cashback-history")]
        public async Task<IActionResult> CashbackHistory()
        {
            if (CheckAndLogout() is IActionResult logout) return logout;
            return View();
        }


        [HttpPost("get-cashback-history")]
        public async Task<IActionResult> GetCashbackHistory([FromForm] UserCompanyWalletRequestDto req)
        {
            if (CheckAndLogout() is IActionResult logout) return logout;
            try
            {
                var result = await _addSvc.AddUpdateUserCompanyWalletRequest(req);
                return Ok(new { success = result.error == "0", message = result.json_result });
            }
            catch { return StatusCode(500, new { success = false, token_valid = true, data = "", message = "Internal server error" }); }
        }
        #endregion


        #region Coupons
        [HttpGet("user-coupons")]
        public async Task<IActionResult> UserCoupons()
        {
            if (CheckAndLogout() is IActionResult logout) return logout;
            return View();
        }


        [HttpPost("get-user-coupons")]
        public async Task<IActionResult> GetUserCoupons([FromForm] UserCompanyWalletRequestDto req)
        {
            if (CheckAndLogout() is IActionResult logout) return logout;
            try
            {
                var result = await _addSvc.AddUpdateUserCompanyWalletRequest(req);
                return Ok(new { success = result.error == "0", message = result.json_result });
            }
            catch { return StatusCode(500, new { success = false, token_valid = true, data = "", message = "Internal server error" }); }
        }
        #endregion


    }
}
