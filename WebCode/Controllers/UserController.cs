using AppModels;
using AppServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering; 
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json; 
using System.Data;
using System.Security.Claims;
namespace AppWeb.Controllers
{
    [Route("user"), Authorize]
    public class UserController : BaseController
    {

        #region Constructors  
        private readonly IAddServices _addSvc;
        private readonly IGetServices _getSvc;
        public UserController(IConfiguration config, IAddServices addServices,
            IGetServices getSvc, IMemoryCache cache) : base(config, addServices, getSvc, cache)
        {
            _addSvc = addServices;
            _getSvc = getSvc;
        }
        #endregion



        #region update-user-kyc 
        [HttpPost("update-user-kyc")]
        public async Task<IActionResult> UpdateUserKyc([FromForm] UpdateUserKycDto req)
        {
            if (CheckAndLogout() is IActionResult logout) return logout;
            try
            {
                var result = await _addSvc.UpdateUserKyc(req);
                return Ok(new { success = result.error == "0", message = result.json_result });
            }
            catch { return StatusCode(500, new { success = false, token_valid = true, data = "", message = "Internal server error" }); }
        }
        #endregion



        #region self-user-profile 
        [HttpGet("user-profile/{user_id?}")]
        public async Task<IActionResult> user_profile(string? user_id)
        {
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;

            var model = new AddUpdateUserRequest();
            user_id=User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!string.IsNullOrEmpty(user_id))
            {
                var req_obj = new UserListBasicDetailFilter() { user_id = user_id };

                db_common_res json = await _getSvc.UserDetail(req_obj);
                if (json?.json_result != null)
                {
                    var first = JsonConvert.DeserializeObject<AddUpdateUserRequest>(json?.json_result);
                    if (first != null)
                    {
                        model = new AddUpdateUserRequest()
                        {
                            uid = first.uid,
                            role_id = first.role_id,
                            sub_role_id = first.sub_role_id,
                            sponsor_user_id = first.sponsor_user_id,
                            user_id = first.user_id,
                            name = first.name,
                            com_name = first.com_name,
                            mobile = first.mobile,
                            email_id = first.email_id,

                            gender = first.gender,
                            dob = first.dob,
                            address = first.address,
                            state_id = first.state_id,
                            dist_id = first.dist_id,
                            city = first.city,
                            pin_code = first.pin_code,
                            nom_name = first.nom_name,
                            nom_rela = first.nom_rela,
                            bank_id = first.bank_id,
                            branch = first.branch,
                            account_no = first.account_no,
                            ac_type = first.ac_type,
                            ifsc = first.ifsc,
                            bank_img = first.bank_img,
                            bank_status = first.bank_status,
                            pan_no = first.pan_no,
                            pan_img = first.pan_img,
                            pan_status = first.pan_status,
                            aadhar_no = first.aadhar_no,
                            aadhar_img_1 = first.aadhar_img_1,
                            aadhar_img_2 = first.aadhar_img_2,
                            aadhar_status = first.aadhar_status,
                            pf_img = first.pf_img,
                            tin_no = first.tin_no,
                            cin_no = first.cin_no,
                            gst_no = first.gst_no,
                            gst_img = first.gst_img,
                            gst_status = first.gst_status
                        };
                    }
                    if (model.sub_role_id == 1)
                        return RedirectToAction("logout", "home");
                }
            }


            var search_obj = new dropdown_request_param { mst_key = "GET_STATE_LIST" };
            var StateList = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
            ViewBag.StateList = new SelectList(StateList ?? new List<dropdown_response>(), "Value", "Text", model.state_id);

            search_obj = new dropdown_request_param { mst_key = "GET_DISTRICT_LIST", id = model.state_id };
            var DistrictList = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
            ViewBag.DistrictList = new SelectList(DistrictList ?? new List<dropdown_response>(), "Value", "Text", model.dist_id);

            search_obj = new dropdown_request_param { mst_key = "GET_BANK_LIST" };
            var BankList = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
            ViewBag.BankList = new SelectList(BankList ?? new List<dropdown_response>(), "Value", "Text", model.bank_id);

            return View(model);
        }


        [HttpPost("user-profile/{user_id?}")]
        public async Task<IActionResult> user_profile([FromForm] AddUpdateUserRequest req)
        {
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;

           
            var search_obj = new dropdown_request_param { mst_key = "GET_STATE_LIST" };
            if (!ModelState.IsValid)
            {
                var StateList = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
                ViewBag.StateList = new SelectList(StateList ?? new List<dropdown_response>(), "Value", "Text", req.state_id);

                search_obj = new dropdown_request_param { mst_key = "GET_DISTRICT_LIST", id = req.state_id };
                var DistrictList = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
                ViewBag.DistrictList = new SelectList(DistrictList ?? new List<dropdown_response>(), "Value", "Text", req.dist_id);

                search_obj = new dropdown_request_param { mst_key = "GET_BANK_LIST" };
                var BankList = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
                ViewBag.BankList = new SelectList(BankList ?? new List<dropdown_response>(), "Value", "Text", req.bank_id);

                // Collect all errors with <br/> line break
                TempData["Error"] = string.Join("<br/>", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));

                return View(req);
            }

            req.login_uid = int.TryParse(User.FindFirst(System.Security.Claims.ClaimTypes.Sid)?.Value, out var uid) ? uid : 0;
            req.role_id = 1;
            req.submit_type = "SELF";
            req.dob = ParseDate(req.dob);
            // 🔹 save data
            db_common_res result = await _addSvc.AddUser(req);
            if (result.error == "0")
            {
                if (req.user_id == "")
                    TempData["Success"] = "✅ Your details have been saved successfully. Your user Id is :" + result.json_result;
                else
                    TempData["Success"] = "✅ Your details have been updated successfully.";

                return RedirectToAction("user-profile", "user");
            }
            TempData["Error"] = result.json_result;

            search_obj = new dropdown_request_param { mst_key = "GET_STATE_LIST" };
            var StateList_2 = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
            ViewBag.StateList = new SelectList(StateList_2 ?? new List<dropdown_response>(), "Value", "Text", req.state_id);

            search_obj = new dropdown_request_param { mst_key = "GET_DISTRICT_LIST", id = req.state_id };
            var DistrictList_2 = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
            ViewBag.DistrictList = new SelectList(DistrictList_2 ?? new List<dropdown_response>(), "Value", "Text", req.dist_id);

            search_obj = new dropdown_request_param { mst_key = "GET_BANK_LIST" };
            var BankList_1 = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
            ViewBag.BankList = new SelectList(BankList_1 ?? new List<dropdown_response>(), "Value", "Text", req.bank_id);

            return View(new AddUpdateUserRequest());
        }
        #endregion



        #region admin-sysuser-list   
        [HttpGet("admin-sysuser-list")]
        public async Task<IActionResult> admin_sysuser_list()
        {
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;
            return View();
        }

        [HttpPost("get-admin-sysuser-list")]
        public async Task<IActionResult> GetAdminSysUserList([FromBody] UserListFilter req)
        {
            var tokenCheck = TokenValidate();
            if (!tokenCheck.IsValid)
                return Ok(new { success = false, token_valid = false, message = "Invalid or expired token.", data = "" });

            var result = await _getSvc.UserList(req);
            return Ok(new { success = true, token_valid = true, data = result, message = "" });
        }
        [HttpGet("admin-add-sysuser/{user_id?}")]
        public async Task<IActionResult> admin_add_sysuser(string? user_id, string? submit_type = "")
        {
            if (RoleWiseLogin("admin_role") is IActionResult rolecheck) return rolecheck;
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;


            var model = new AddUpdateUserRequest();
            model.submit_type = "ADD";
            if (!string.IsNullOrEmpty(user_id))
            {
                var req_obj = new UserListBasicDetailFilter() { user_id = user_id };

                db_common_res json = await _getSvc.UserDetail(req_obj);
                if (json?.json_result != null)
                {
                    var first = JsonConvert.DeserializeObject<AddUpdateUserRequest>(json?.json_result);
                    if (first != null)
                    {
                        model = new AddUpdateUserRequest()
                        {
                            uid = first.uid,
                            role_id = first.role_id,
                            sub_role_id = first.sub_role_id,
                            sponsor_user_id = first.sponsor_user_id,
                            user_id = first.user_id,
                            name = first.name,
                            com_name = first.com_name,

                            mobile = first.mobile,
                            email_id = first.email_id,
                            gender = first.gender,
                            dob = first.dob,
                            address = first.address,
                            state_id = first.state_id,
                            dist_id = first.dist_id,
                            city = first.city,
                            pin_code = first.pin_code,
                            nom_name = first.nom_name,
                            nom_rela = first.nom_rela,
                            bank_id = first.bank_id,
                            branch = first.branch,
                            account_no = first.account_no,
                            ac_type = first.ac_type,
                            ifsc = first.ifsc,
                            bank_img = first.bank_img,
                            bank_status = first.bank_status,
                            pan_no = first.pan_no,
                            pan_img = first.pan_img,
                            pan_status = first.pan_status,
                            aadhar_no = first.aadhar_no,
                            aadhar_img_1 = first.aadhar_img_1,
                            aadhar_img_2 = first.aadhar_img_2,
                            aadhar_status = first.aadhar_status,
                            pf_img = first.pf_img,
                            tin_no = first.tin_no,
                            cin_no = first.cin_no,
                            gst_no = first.gst_no,
                            gst_img = first.gst_img,
                            gst_status = first.gst_status
                        };
                    }
                    if (model.sub_role_id == 1)
                        return RedirectToAction("logout", "home");
                }
            }


            var search_obj = new dropdown_request_param { mst_key = "GET_STATE_LIST" };
            var StateList = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
            ViewBag.StateList = new SelectList(StateList ?? new List<dropdown_response>(), "Value", "Text", model.state_id);

            search_obj = new dropdown_request_param { mst_key = "GET_DISTRICT_LIST", id = model.state_id };
            var DistrictList = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
            ViewBag.DistrictList = new SelectList(DistrictList ?? new List<dropdown_response>(), "Value", "Text", model.dist_id);

            return View(model);
        }


        [HttpPost("admin-add-sysuser/{user_id?}")]
        public async Task<IActionResult> admin_add_sysuser([FromForm] AddUpdateUserRequest req)
        {
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;

            var search_obj = new dropdown_request_param { mst_key = "GET_STATE_LIST" };
            if (!ModelState.IsValid)
            {
                var StateList = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
                ViewBag.StateList = new SelectList(StateList ?? new List<dropdown_response>(), "Value", "Text", req.state_id);

                search_obj = new dropdown_request_param { mst_key = "GET_DISTRICT_LIST", id = req.state_id };
                var DistrictList = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
                ViewBag.DistrictList = new SelectList(DistrictList ?? new List<dropdown_response>(), "Value", "Text", req.dist_id);

                // Collect all errors with <br/> line break
                TempData["Error"] = string.Join("<br/>", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));

                return View(req);
            }

            req.login_uid = int.TryParse(User.FindFirst(System.Security.Claims.ClaimTypes.Sid)?.Value, out var uid) ? uid : 0;
            req.role_id = 1;
            req.dob = ParseDate(req.dob);
            if (req.uid == null)
            {
                string? error = req.password == null ? "Please enter password.!!"
                              : req.tran_password == null ? "Please enter transaction password.!!"
                              : null;
                if (error != null) { TempData["Error"] = error; return View(req); }
            }
            req.submit_type = "ADMIN";
            // 🔹 save data
            db_common_res result = await _addSvc.AddUser(req);
            if (result.error == "0")
            {
                if (req.uid == null)
                {
                    // Send WhatsApp Message start
                    string queryString = string.Join("&", new[]
                    {
                         ("divine_name", req.name ?? string.Empty),
                         ("divine_whatsapp_no", "91" + req.mobile)
                    }.Select(kvp => $"{kvp.Item1}={kvp.Item2}"));
                    var sent_result = await new WhatsAppService().SendWhatsAppAsync($"{Const.WA_URL_SALES_MAN_CREATE}?{queryString}");
                    // Send WhatsApp Message End 

                    TempData["Success"] = "✅ Your details have been saved successfully. Your user Id is :" + result.json_result;
                }
                else
                    TempData["Success"] = "✅ Your details have been updated successfully.";
               
                return RedirectToAction("admin-user-list", "user");
            }
            TempData["Error"] = result.json_result;

            search_obj = new dropdown_request_param { mst_key = "GET_STATE_LIST" };
            var StateList_2 = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
            ViewBag.StateList = new SelectList(StateList_2 ?? new List<dropdown_response>(), "Value", "Text", req.state_id);

            search_obj = new dropdown_request_param { mst_key = "GET_DISTRICT_LIST", id = req.state_id };
            var DistrictList_2 = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
            ViewBag.DistrictList = new SelectList(DistrictList_2 ?? new List<dropdown_response>(), "Value", "Text", req.dist_id);

            return View(new AddUpdateUserRequest());
        }
        #endregion



        #region admin-franchise-list  
        [HttpGet("admin-franchise-list")]
        public async Task<IActionResult> admin_franchise_list()
        {
            if (RoleWiseLogin("admin_role") is IActionResult rolecheck) return rolecheck;
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;

            var check = CheckAndLogout();
            if (check != null) return check;
            return View();
        }

        [HttpPost("get-admin-franchise-list")]
        public async Task<IActionResult> GetFranchiseList([FromBody] UserListFilter req)
        {
            var tokenCheck = TokenValidate();
            if (!tokenCheck.IsValid)
                return Ok(new { success = false, token_valid = false, message = "Invalid or expired token.", data = "" });

            var result = await _getSvc.UserList(req);
            return Ok(new { success = true, token_valid = true, data = result });
        }


        [HttpGet("admin-add-franchise/{user_id?}")]
        public async Task<IActionResult> admin_add_franchise(string? user_id)
        {
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;

            var model = new AddUpdateUserRequest();

            if (!string.IsNullOrEmpty(user_id))
            {
                var req_obj = new UserListBasicDetailFilter() { user_id = user_id };

                db_common_res json = await _getSvc.UserDetail(req_obj);
                if (json?.json_result != null)
                {
                    var first = JsonConvert.DeserializeObject<AddUpdateUserRequest>(json?.json_result);
                    if (first != null)
                    {
                        model = new AddUpdateUserRequest()
                        {
                            uid = first.uid,
                            role_id = first.role_id,
                            sub_role_id = first.sub_role_id,
                            sponsor_user_id = first.sponsor_user_id,
                            user_id = first.user_id,
                            name = first.name,
                            com_name = first.com_name,
                            mobile = first.mobile,
                            email_id = first.email_id,
                            gender = first.gender,
                            dob = first.dob,
                            address = first.address,
                            state_id = first.state_id,
                            dist_id = first.dist_id,
                            city = first.city,
                            pin_code = first.pin_code,
                            nom_name = first.nom_name,
                            nom_rela = first.nom_rela,
                            bank_id = first.bank_id,
                            branch = first.branch,
                            account_no = first.account_no,
                            ac_type = first.ac_type,
                            ifsc = first.ifsc,
                            bank_img = first.bank_img,
                            bank_status = first.bank_status,
                            pan_no = first.pan_no,
                            pan_img = first.pan_img,
                            pan_status = first.pan_status,
                            aadhar_no = first.aadhar_no,
                            aadhar_img_1 = first.aadhar_img_1,
                            aadhar_img_2 = first.aadhar_img_2,
                            aadhar_status = first.aadhar_status,
                            pf_img = first.pf_img,
                            tin_no = first.tin_no,
                            cin_no = first.cin_no,
                            gst_no = first.gst_no,
                            gst_img = first.gst_img,
                            gst_status = first.gst_status
                        };
                    }
                    if (model.sub_role_id == 1)
                        return RedirectToAction("logout", "home");
                }
            }

            var search_obj = new dropdown_request_param { mst_key = "GET_STATE_LIST" };
            var StateList = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
            ViewBag.StateList = new SelectList(StateList ?? new List<dropdown_response>(), "Value", "Text", model.state_id);

            search_obj = new dropdown_request_param { mst_key = "GET_DISTRICT_LIST", id = model.state_id };
            var DistrictList = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
            ViewBag.DistrictList = new SelectList(DistrictList ?? new List<dropdown_response>(), "Value", "Text", model.dist_id);

            return View(model);
        }


        [HttpPost("admin-add-franchise/{user_id?}")]
        public async Task<IActionResult> admin_add_franchise([FromForm] AddUpdateUserRequest req)
        {
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;

            var search_obj = new dropdown_request_param { mst_key = "GET_STATE_LIST" };
            if (!ModelState.IsValid)
            {
                var StateList = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
                ViewBag.StateList = new SelectList(StateList ?? new List<dropdown_response>(), "Value", "Text", req.state_id);

                search_obj = new dropdown_request_param { mst_key = "GET_DISTRICT_LIST", id = req.state_id };
                var DistrictList = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
                ViewBag.DistrictList = new SelectList(DistrictList ?? new List<dropdown_response>(), "Value", "Text", req.dist_id);

                TempData["Error"] = string.Join("<br/>", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return View(req);
            }

            req.login_uid = int.TryParse(User.FindFirst(System.Security.Claims.ClaimTypes.Sid)?.Value, out var uid) ? uid : 0;
            //req.role_id = 2;
            req.dob = ParseDate(req.dob);
            req.submit_type = "ADMIN";
            //req.password = "abc@123";
            //req.tran_password = "abc@123";
            // 🔹 save data
            db_common_res result = await _addSvc.AddUser(req);
            if (result.error == "0")
            {
                if (req.uid == null)
                {
                    // Send WhatsApp Message start
                    //string queryString = string.Join("&", new[]
                    //{
                    //     ("divine_name", req.name ?? string.Empty),
                    //     ("divine_whatsapp_no", "91" + req.mobile)
                    //}.Select(kvp => $"{kvp.Item1}={kvp.Item2}"));
                    //var sent_result = await new WhatsAppService().SendWhatsAppAsync($"{Const.WA_URL_PARTY_CREATE}?{queryString}");
                    // Send WhatsApp Message End 
                    TempData["Success"] = "✅ Your details have been saved successfully. Your user Id is :" + result.json_result;
                }
                else
                    TempData["Success"] = "✅ Your details have been updated successfully.";

                return RedirectToAction("admin-franchise-list", "user");
            }
            TempData["Error"] = result.json_result;

            search_obj = new dropdown_request_param { mst_key = "GET_STATE_LIST" };
            var StateList_2 = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
            ViewBag.StateList = new SelectList(StateList_2 ?? new List<dropdown_response>(), "Value", "Text", req.state_id);

            search_obj = new dropdown_request_param { mst_key = "GET_DISTRICT_LIST", id = req.state_id };
            var DistrictList_2 = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
            ViewBag.DistrictList = new SelectList(DistrictList_2 ?? new List<dropdown_response>(), "Value", "Text", req.dist_id);

            return View(new AddUpdateUserRequest());
        }

        #endregion



        #region admin-user-list  
        [HttpGet("admin-user-list")]
        public async Task<IActionResult> admin_user_list()
        {
            if (RoleWiseLogin("admin_role") is IActionResult rolecheck) return rolecheck;
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;

            return View();
        }

        [HttpPost("get-admin-user-list")]
        public async Task<IActionResult> GetUserList([FromBody] UserListFilter req)
        {
            var tokenCheck = TokenValidate();
            if (!tokenCheck.IsValid)
                return Ok(new { success = false, token_valid = false, message = "Invalid or expired token.", data = "" });

            var result = await _getSvc.UserList(req);
            return Ok(new { success = true, token_valid = true, data = result });
        }


        [HttpGet("admin-add-user/{user_id?}")]
        public async Task<IActionResult> admin_add_user(string? user_id)
        {
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;

            var model = new AddUpdateUserRequest();

            if (!string.IsNullOrEmpty(user_id))
            {
                var req_obj = new UserListBasicDetailFilter() { user_id = user_id };

                db_common_res json = await _getSvc.UserDetail(req_obj);
                if (json?.json_result != null)
                {
                    var first = JsonConvert.DeserializeObject<AddUpdateUserRequest>(json?.json_result);
                    if (first != null)
                    {
                        model = new AddUpdateUserRequest()
                        {
                            uid = first.uid,
                            role_id = first.role_id,
                            sub_role_id = first.sub_role_id,
                            sponsor_user_id = first.sponsor_user_id,
                            user_id = first.user_id,
                            name = first.name,
                            com_name = first.com_name,
                            mobile = first.mobile,
                            email_id = first.email_id,
                            gender = first.gender,
                            dob = first.dob,
                            address = first.address,
                            state_id = first.state_id,
                            dist_id = first.dist_id,
                            city = first.city,
                            pin_code = first.pin_code,
                            nom_name = first.nom_name,
                            nom_rela = first.nom_rela,
                            bank_id = first.bank_id,
                            branch = first.branch,
                            account_no = first.account_no,
                            ac_type = first.ac_type,
                            ifsc = first.ifsc,
                            bank_img = first.bank_img,
                            bank_status = first.bank_status,
                            pan_no = first.pan_no,
                            pan_img = first.pan_img,
                            pan_status = first.pan_status,
                            aadhar_no = first.aadhar_no,
                            aadhar_img_1 = first.aadhar_img_1,
                            aadhar_img_2 = first.aadhar_img_2,
                            aadhar_status = first.aadhar_status,
                            pf_img = first.pf_img,
                            tin_no = first.tin_no,
                            cin_no = first.cin_no,
                            gst_no = first.gst_no,
                            gst_img = first.gst_img,
                            gst_status = first.gst_status
                        };
                    }
                    if (model.sub_role_id == 1)
                        return RedirectToAction("logout", "home");
                }
            }

            var search_obj = new dropdown_request_param { mst_key = "GET_STATE_LIST" };
            var StateList = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
            ViewBag.StateList = new SelectList(StateList ?? new List<dropdown_response>(), "Value", "Text", model.state_id);

            search_obj = new dropdown_request_param { mst_key = "GET_DISTRICT_LIST", id = model.state_id };
            var DistrictList = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
            ViewBag.DistrictList = new SelectList(DistrictList ?? new List<dropdown_response>(), "Value", "Text", model.dist_id);

            return View(model);
        }


        [HttpPost("admin-add-user/{user_id?}")]
        public async Task<IActionResult> admin_add_user([FromForm] AddUpdateUserRequest req)
        {
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;

            var search_obj = new dropdown_request_param { mst_key = "GET_STATE_LIST" };
            if (!ModelState.IsValid)
            {
                var StateList = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
                ViewBag.StateList = new SelectList(StateList ?? new List<dropdown_response>(), "Value", "Text", req.state_id);

                search_obj = new dropdown_request_param { mst_key = "GET_DISTRICT_LIST", id = req.state_id };
                var DistrictList = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
                ViewBag.DistrictList = new SelectList(DistrictList ?? new List<dropdown_response>(), "Value", "Text", req.dist_id);

                TempData["Error"] = string.Join("<br/>", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return View(req);
            }

            req.login_uid = int.TryParse(User.FindFirst(ClaimTypes.Sid)?.Value, out var uid) ? uid : 0;
            
            req.dob = ParseDate(req.dob);
            req.submit_type = "ADMIN"; 
            // 🔹 save data
            db_common_res result = await _addSvc.AddUser(req);
            if (result.error == "0")
            {
                if (req.uid == null)
                {
                    // Send WhatsApp Message start
                    //string queryString = string.Join("&", new[]
                    //{
                    //     ("divine_name", req.name ?? string.Empty),
                    //     ("divine_whatsapp_no", "91" + req.mobile)
                    //}.Select(kvp => $"{kvp.Item1}={kvp.Item2}"));
                    //var sent_result = await new WhatsAppService().SendWhatsAppAsync($"{Const.WA_URL_PARTY_CREATE}?{queryString}");
                    // Send WhatsApp Message End 
                    TempData["Success"] = "✅ Your details have been saved successfully. Your user Id is :" + result.json_result;
                }
                else
                    TempData["Success"] = "✅ Your details have been updated successfully.";

                //return RedirectToAction("admin-franchise-list", "user");
            }
            TempData["Error"] = result.json_result;

            search_obj = new dropdown_request_param { mst_key = "GET_STATE_LIST" };
            var StateList_2 = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
            ViewBag.StateList = new SelectList(StateList_2 ?? new List<dropdown_response>(), "Value", "Text", req.state_id);

            search_obj = new dropdown_request_param { mst_key = "GET_DISTRICT_LIST", id = req.state_id };
            var DistrictList_2 = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
            ViewBag.DistrictList = new SelectList(DistrictList_2 ?? new List<dropdown_response>(), "Value", "Text", req.dist_id);

            return View(new AddUpdateUserRequest());
        }
        #endregion


 
        #region Change_Sponsor 
        [HttpGet("change-sponsor")]
        public async Task<IActionResult> Change_Sponsor()
        {
            if (RoleWiseLogin("admin_role") is IActionResult rolecheck) return rolecheck;
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;

            var model = new AddUpdateUserRequest();

            var uid = User.FindFirst(System.Security.Claims.ClaimTypes.Sid)?.Value;

            var search_obj = new dropdown_request_param { mst_key = "GET_All_USER", id = Convert.ToInt32(3) };
            var BuyerList = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
            ViewBag.BuyerList = new SelectList(BuyerList ?? new List<dropdown_response>(), "Value", "Text");

            return View(model);
        }


        [HttpGet("sponsor-update")]
        public async Task<IActionResult> Change_Sponsor(int? user_type, int? uid, int? sponsor_uid, string? user_id)
        {
            if (RoleWiseLogin("admin_role") is IActionResult rolecheck) return rolecheck;
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;
            try
            {
                var req = new ChangeSponsor()
                {
                    user_type = user_type,
                    uid = uid,
                    sponsor_uid = sponsor_uid,
                    user_id = user_id,
                    login_user_id = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value,
                };

                db_common_res result = await _addSvc.ChangeSponsor(req);
                return Ok(new { success = result.error == "0", token_valid = true, data = "", message = result.json_result });
            }
            catch (Exception er) { return Ok(new { success = false, token_valid = true, data = "", message = "Internal server error" }); }
        }
        #endregion


        #region Change_Password
        [HttpGet("change-password")]
        public async Task<IActionResult> Change_Password()
        {
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;
            return View();
        }


        [HttpGet("update-password")]
        public async Task<IActionResult> Change_Password(string? old_password, string? password)
        {
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;
            try
            {
                var req = new ChangePassword()
                {
                    old_password = old_password,
                    password = password,
                    uid = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid)?.Value),
                };

                db_common_res result = await _addSvc.ChangePassword(req);
                return Ok(new { success = result.error == "0", token_valid = true, data = "", message = result.json_result });
            }
            catch (Exception er) { return Ok(new { success = false, token_valid = true, data = "", message = "Internal server error" }); }
        }
        #endregion


        #region welcomeletter
        [HttpGet("welcome-letter")]
        public async Task<IActionResult> welcomeletter()
        {
            if (CheckAndLogout() is IActionResult logincheck) return logincheck;
            return View();
        }

        #endregion


        
    }
}
