using AppModels;
using AppServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
namespace AppWeb.Controllers
{
    [Route("common"), Authorize]
    public class CommonAddController : BaseController
    {
      
        #region Constructors
            private readonly IAddServices _addSvc;
            private readonly IGetServices _getSvc;

            public CommonAddController(
                IConfiguration config,
                IAddServices addSvc,
                IGetServices getSvc, IMemoryCache cache
            ) : base(config, addSvc, getSvc, cache) // ✅ Pass all required dependencies to BaseController
            {
                _addSvc = addSvc;
                _getSvc = getSvc;
            }
            #endregion
       

        #region add-size
        [HttpPost("add-size")]
        public async Task<IActionResult> AddSize([FromForm] SizeDto req)
        {
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;
            try
            {
                db_common_res result = await _addSvc.AddSize(req);
                return Ok(new { success = result.error == "0", token_valid = true, data = "", message = result.json_result});
            }
            catch { return Ok(new { success =false, token_valid = true, data = "", message = "Internal server error" }); }
        }
        #endregion


        #region add-batch
        [HttpPost("add-batch")]
        public async Task<IActionResult> AddBatch([FromForm] BatchDto req)
        {
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;
            try
            {
                db_common_res result = await _addSvc.AddBatch(req);
                return Ok(new { success = result.error == "0", token_valid = true, data = "", message = result.json_result });
            }
            catch { return Ok(new { success = false, token_valid = true, data = "", message = "Internal server error" }); }
        }
        #endregion

         
        #region add-category
        [HttpPost("add-category")]
        public async Task<IActionResult> AddCategory(CategoryDto req)
        {
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;
            try
            {
                db_common_res result = await _addSvc.AddCategory(req);
                return Ok(new { success = result.error == "0", token_valid = true, data = "", message = result.json_result });
            }
            catch { return Ok(new { success = false, token_valid = true, data = "", message = "Internal server error" }); }
        }
        #endregion
         
    }
}
