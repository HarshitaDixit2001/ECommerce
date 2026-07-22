using AppModels;
using AppServices;
using AppWeb.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc; 
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;

namespace WebCode.Controllers
{
     
    [Route("user-report"), Authorize]
    public class UserReportsController : BaseController
    {
        #region Constructors  
        private readonly IAddServices _addSvc;
        private readonly IGetServices _getSvc;
        public UserReportsController(IConfiguration config, IAddServices addServices,
            IGetServices getSvc, IMemoryCache cache) : base(config, addServices, getSvc, cache)
        {
            _addSvc = addServices;
            _getSvc = getSvc;
        }
        #endregion


        #region user-binary-tree 
        [HttpGet("user-binary-tree/{user_id?}")]
        public async Task<IActionResult> UserBinaryTree(string? user_id)
        {
            if (CheckAndLogout() is IActionResult logincheck)
                return logincheck;

            var login_user_id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            user_id ??= login_user_id;

            var rawData = await _getSvc.GetUserBinaryTree(new BinaryTeamFilter
            {
                login_user_id = login_user_id,
                user_id = user_id
            });

            var model = new BinaryTeamTreeViewModel
            {
                node1 = rawData.FirstOrDefault(x => x.node == 1),
                node2 = rawData.FirstOrDefault(x => x.node == 2),
                node3 = rawData.FirstOrDefault(x => x.node == 3),
                node4 = rawData.FirstOrDefault(x => x.node == 4),
                node5 = rawData.FirstOrDefault(x => x.node == 5),
                node6 = rawData.FirstOrDefault(x => x.node == 6),
                node7 = rawData.FirstOrDefault(x => x.node == 7)
            };
            return View(model);
        }
        #endregion


        #region user-binary-list  
        [HttpGet("user-binary-list")]
        public async Task<IActionResult> UserBinaryList()
        {
            var check = CheckAndLogout();
            if (check != null) return check;

            return View();
        }

        [HttpPost("get-binary-list")]
        public async Task<IActionResult> UserBinarylist([FromBody] UserBinaryFilter req)
        {
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;
            try
            {
                var result = await _getSvc.UserBinaryList(req);
                return Ok(new { success = true, token_valid = true, data = result, message = "" });
            }
            catch (Exception ex) { return StatusCode(500, new { success = false, token_valid = true, data = "", total_count = 0, message = "Internal server error" }); }
        }
        #endregion


        #region Matrix Tree View
        [HttpGet("matrix-tree-view")]
        public async Task<IActionResult> MatrixTreeView()
        {
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;
            return View();
        }


        [HttpGet("get-matrix-tree-view")]
        public async Task<IActionResult> GetMatrixTreeView(int? uid = 0, string? from_date = null, string? to_date = null)
        {
            try
            {
                var req = new matrix_tree_view_filter()
                {
                    uid = uid,
                    from_date = from_date,
                    to_date = to_date
                };
                db_common_res result = await _getSvc.GetMatrixTreeView(req);
                if (result == null)
                    return Ok(new { success = false, token_valid = true, data = "[]", message = "" });

                return Ok(new { success = result.error == "", token_valid = true, data = result.json_result, message = "" });
            }
            catch (Exception ex) { return StatusCode(500, new { success = false, token_valid = true, message = "Internal server error", details = ex.Message }); }
        }
        #endregion


        #region user-matrix-list
        [HttpGet("user-matrix-list")]
        public async Task<IActionResult> UserMatrixList()
        {
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;
            return View();
        }
        #endregion


        #region User-Matrix-list 
        [HttpPost("get-matrix-list")]
        public async Task<IActionResult> UserMatrixlist([FromBody] UserMatrixFilter req)
        {
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;
            try
            {
                var result = await _getSvc.UserMatrixList(req);
                return Ok(new { success = true, token_valid = true, data = result, message = "" });
            }
            catch (Exception ex) { return StatusCode(500, new { success = false, token_valid = true, data = "", total_count = 0, message = "Internal server error" }); }
        }
        #endregion



        #region user-reward-list
        [HttpGet("user-reward-list")]
        public async Task<IActionResult> UserRewardist()
        {
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;
            return View();
        }
        #endregion
    }
}
