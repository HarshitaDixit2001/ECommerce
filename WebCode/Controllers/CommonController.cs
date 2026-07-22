using AppModels;
using AppServices; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
namespace AppWeb.Controllers
{
    [Route("common")]
    public class CommonController : BaseController
    {

        #region Constructors   
        private readonly IGetServices _getSvc;
        public CommonController(IConfiguration config, IAddServices addServices,
            IGetServices getSvc, IMemoryCache cache) : base(config, addServices, getSvc, cache)
        { 
            _getSvc = getSvc;
        }
        #endregion


        #region master-reports  
        [HttpGet("master-reports")]
        public async Task<IActionResult> master_reports(int? id = 0, int? sub_id=0, string? mst_key=null, 
        string? user_id=null, bool? mst_key_detail = false)
        {
            guest_report_request_param obj = new guest_report_request_param();
            obj.mst_key = mst_key;
            obj.mst_key_detail = mst_key_detail;
            obj.id = id;
            obj.sub_id = sub_id;
            obj.user_id = user_id;
            db_common_res result = await _getSvc.master_reports(obj);
            return Ok(new { success = result.error == "", token_valid = true, data = result.json_result, message = ""});
        }
        #endregion


        #region get-master-dropdown
        [HttpGet("get-master-dropdown")]
        public async Task<IActionResult> get_master_dropdown(int? id = 0, string mst_key = "")
        {
            dropdown_request_param obj = new dropdown_request_param();
            obj.mst_key = mst_key;
            obj.id = id;
            var response = await _getSvc.master_dropdown(obj);
            return Ok(new { data = response });
        }

        #endregion


        #region master-update-status  
        [HttpGet("master-update-status")]
        public async Task<IActionResult> master_update_status(string? mst_key = null, string? col_name = null, int ? id = 0)
        {
            update_table_row_status_master_param obj = new update_table_row_status_master_param();
            obj.mst_key = mst_key;
            obj.col_name = col_name;
            obj.id = id;
            db_common_res result = await _getSvc.master_update_status(obj);
            return Ok(new { success = result.error == "0", token_valid = true, data = result.json_result, message = "" });
        }
        #endregion


        #region master-delete-table-row  
        [HttpGet("master-delete-table-row")]
        public async Task<IActionResult> master_delete_table_row(int? id = 0, string? mst_key = null)
        {
            delete_table_row_master_param obj = new delete_table_row_master_param();
            obj.mst_key = mst_key; 
            obj.id = id; 
            db_common_res result = await _getSvc.master_delete_table_row(obj);
            if (!string.IsNullOrWhiteSpace(result.json_result))
            {
                string filePath = Path.Combine(Const.IMAGEPATH,result.json_result.Replace("/", "\\"));
                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);
            } 
            return Ok(new { success = result.error == "0", token_valid = true, data = result.json_result, message = "" });
        }
        #endregion

    }
}
