using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace AppModels
{
    public class UserByUIDFilter
    {
        public int? uid { get; set; }

    }

    public class AddressFilter
    {
        public int? uid { get; set; }
        public int? said { get; set; }
    }


    public class guest_report_request_param
    {
        public string? mst_key { get; set; } = string.Empty;     // Used to determine type of master data (e.g., 'product', 'batch', etc.)
        public bool? mst_key_detail { get; set; } = false;
        public string? from_date { get; set; } = string.Empty;   // Format: yyyy-MM-dd
        public string? to_date { get; set; } = string.Empty;     // Format: yyyy-MM-dd
        public string? user_id { get; set; } = string.Empty;
        public string? invoice_no { get; set; } = string.Empty;
        public int? id { get; set; } = 0;                        // Could be category_id, batch_id, or vendor_id
        public int? sub_id { get; set; } = 0;                    // Could be subcategory_id or size_id
        public string? name { get; set; } = string.Empty;        // Used for name-based filtering (e.g., product name or batch no.)
        public int? variant_id { get; set; } = 0;                // For filtering based on variant
        public int? color_id { get; set; } = 0;                  // For filtering based on color
        public bool? status { get; set; } = null;                    // Nullable, for active/inactive flag
        public int? page_record { get; set; } = 0;
        public int? page_no { get; set; } = 0;

    }

    public class update_table_row_status_master_param
    {
        public string? mst_key { get; set; } 
        public string? col_name { get; set; } 
        public int? id { get; set; } = 0;   
    }

    public class delete_table_row_master_param
    {
        public string? mst_key { get; set; } 
        public int? id { get; set; } = 0;
    }


    public class UserProductMappingFilter
    {
        public int? uid { get; set; }

    }

    public class matrix_tree_view_filter
    {
        public string? from_date { get; set; }
        public string? to_date { get; set; }
        public int? uid { get; set; }
    }
    public class ExpensesMappingFilter
    {
        public string? from_date { get; set; }
        public string? to_date { get; set; }
        public int? uid { get; set; }
        public int? ecid { get; set; }
    }

    public class UpdateUserKycDto
    {
        public int? uid { get; set; }
        public string? mst_key { get; set; }
        public int? status { get; set; }
    }
    public class UserListFilter
    {
        public string? from_date { get; set; }
        public string? to_date { get; set; }
        public int? uid { get; set; }
        public string? user_id { get; set; }
        public string? name { get; set; }
        public string? mobile { get; set; }
        public string? gst_no { get; set; }
        public int? status { get; set; }
        public int? role_id { get; set; }
        public int? @sub_role_id { get; set; }
    }

    public class dropdown_request_param
    {
        public string? mst_key { get; set; }     // Used to determine type of master data (e.g., 'product', 'batch', etc.)
        public int? id { get; set; } = 0;
    }

    public class filter_cart_data
    {
        public int? cart_id { get; set; }
        public int? uid { get; set; }
    }

    public class CategoryViewModel
    {
        public int? cat_id { get; set; }
        public string? cat_name { get; set; }
        public string? cat_desc { get; set; }
        public string? cat_img { get; set; }
        public bool status { get; set; }
        public string? prod_code { get; set; }
        public string? seo_url { get; set; }
        public string? cat_icon { get; set; }
    }
    public class billing_product_list_param
    {
        public int? inv_type { get; set; } = 0;
        public int? seller_uid { get; set; } = 0;
    }

    [Keyless]
    public class db_common_res
    {
        public string? error { get; set; }
        public string? json_result { get; set; }
    }

    [Keyless]
    public class db_order_res
    {
        public string? error { get; set; }
        public string? inv_gen { get; set; }
        public string? json_result { get; set; }
    }

    public class dropdown_response
    {
        public int? Value { get; set; }
        public string? Text { get; set; }
    }






    public class GenerateUserInvoiceDto
    {
        public int? is_old_inv { get; set; }
        public string? invoice_no { get; set; }
        public string? doe { get; set; }
        public int? cart_id { get; set; }
        public string? remark { get; set; }
        public int? order_id { get; set; }
        public decimal? receive_amount { get; set; }

        public int? inv_type { get; set; } = 0;
        public string? user_id { get; set; } = string.Empty;
        public int? buyer_uid { get; set; } = 0;
        public int? seller_uid { get; set; } = 0;
        public string? gst_no { get; set; } = string.Empty;
        public string? s_name { get; set; } = string.Empty;
        public string? s_mobile_no { get; set; } = string.Empty;
        public int? state_id { get; set; }
        public int? dist_id { get; set; }
        public string? s_city { get; set; } = string.Empty;
        public string? s_address { get; set; } = string.Empty;
        public string? s_pincode { get; set; } = string.Empty;
        public int? pay_id { get; set; } = 0;
        public string? bank_name { get; set; } = string.Empty;
        public string? check_no { get; set; } = string.Empty;
        public DateTime? checkdate { get; set; }
        public decimal? scheme { get; set; } = 0;
        public decimal? discount { get; set; } = 0;
        public decimal? del_charge { get; set; } = 0;
        public decimal? net_delivery_charge { get; set; } = 0;
        public decimal? adjust_amt { get; set; } = 0;
        public string? coupon { get; set; } = string.Empty;
        //public IFormFile? img { get; set; }
        public string? img_name { get; set; }
        public string? panel { get; set; }
        public string? order_no { get; set; }
        public string? appr_by { get; set; }
        public int? status { get; set; }
    }


    public class GenerateUserOrderDto
    {
        public int? cart_id { get; set; }
        public int? inv_type { get; set; } = 0;
        public string? user_id { get; set; } = string.Empty;
        public int? buyer_uid { get; set; } = 0;
        public int? seller_uid { get; set; } = 0;
        public string? gst_no { get; set; } = string.Empty;
        public string? s_name { get; set; } = string.Empty;
        public string? s_mobile_no { get; set; } = string.Empty;
        public int? state_id { get; set; }
        public int? dist_id { get; set; }
        public string? s_city { get; set; } = string.Empty;
        public string? s_address { get; set; } = string.Empty;
        public string? s_pincode { get; set; } = string.Empty;
        public int? pay_id { get; set; } = 0;
        public string? bank_name { get; set; } = string.Empty;
        public string? check_no { get; set; } = string.Empty;
        public string? checkdate { get; set; }
        public decimal? scheme { get; set; } = 0;
        public decimal? discount { get; set; } = 0;
        public decimal? del_charge { get; set; } = 0;
        public decimal? net_delivery_charge { get; set; } = 0;
        public decimal? adjust_amt { get; set; } = 0;
        public string? coupon { get; set; } = string.Empty;
        //public IFormFile? img { get; set; }
        public string? img_name { get; set; }
        public string? panel { get; set; }
        public string? appr_by { get; set; }
        public string? img_utr_no { get; set; }
        public string? img_utr_amount { get; set; }
        public string? img_utr_date { get; set; }
        public string? img_utr_full_text { get; set; }
        public string? img_utr_sender_name { get; set; }
        public string? img_utr_upi_id { get; set; }
        public int? said { get; set; }
    }


    public class SmsPaymentReceive
    {
        public decimal? amount { get; set; }
        public string? refId { get; set; }
        public string? rawSms { get; set; }
        public string? tranDate { get; set; }
        public string? sender { get; set; }
        public string? upiId { get; set; }
    }

    public class LoginRequest
    {
        [Required(ErrorMessage = "Please Enter User Name")]
        public string login_id { get; set; } = string.Empty;
        [Required(ErrorMessage = "Please Enter Password")]
        public string password { get; set; } = string.Empty;
        public int? Num1 { get; set; }
        public int? Num2 { get; set; }

        [Required(ErrorMessage = "Please Enter Captcha")]
        public int? Sum_input { get; set; }
    }

    public class dbLoginRequest
    {
        public string login_id { get; set; }
        public string password { get; set; }
    }

    public class ApiObjectResponse
    {
        public bool success { get; set; }
        public bool token_valid { get; set; }
        public string message { get; set; }
        public string data { get; set; }
    }

    public class LoginResponse
    {
        public bool success { get; set; }
        public string? token { get; set; }
        public string? message { get; set; }
        public LoginUserData data { get; set; } = new();
    }


    public class LoginUserData
    {
        public int? id { get; set; } = 0;
        public string? userId { get; set; } = "";
        public string? name { get; set; } = "";
        public string? role_name { get; set; } = "";
        public string? sub_role_name { get; set; } = "";
        public string? last_login { get; set; }

    }



    [Keyless]
    public class db_common_pageing_res
    {
        public string? error { get; set; }
        public string? json_result { get; set; }
        public int? total_count { get; set; }
    }


    [Keyless]
    public class db_login_res
    {
        public string? error { get; set; }
        public int? id { get; set; }
        public string? login_id { get; set; }
        public string? name { get; set; }
        public string? role_name { get; set; }
        public string? sub_role_name { get; set; }
        public int? status { get; set; }
        public string? last_login { get; set; }
        public string? pf_img { get; set; }
    }
    public class SizeDto
    {
        public int? size_id { get; set; }
        public string? size { get; set; } = string.Empty;
        public bool status { get; set; }
    }
    public class BatchDto
    {
        public int batch_id { get; set; }
        public string batch_no { get; set; } = string.Empty;
        public DateTime mfg_date { get; set; }
        public DateTime exp_date { get; set; }
        public bool status { get; set; }
    }
    public class CategoryDto
    {
        public int? cat_id { get; set; }
        public string? cat_name { get; set; }
        public string? cat_desc { get; set; }
        public string? cat_img { get; set; }
        public string? cat_icon { get; set; }
        public bool status { get; set; }
    }

    public class ApiResponse<T>
    {
        public bool success { get; set; }
        public bool token_valid { get; set; }
        public string message { get; set; }
        public T data { get; set; }
    }


    public class DropdownItem
    {
        public int Value { get; set; }
        public string Text { get; set; } = string.Empty;
    }

    public class DropdownResponse<T>
    {
        public T Data { get; set; } = default!;
    }



    public class Product_db
    {
        public int pid { get; set; }
        public string prod_code { get; set; }
        public string prod_name { get; set; }
        public string? prod_desc { get; set; }
        public int cat_id { get; set; }
        public int? sub_cat_id { get; set; }
        public string? hsncode { get; set; }
        public string? t1 { get; set; }
        public string? d1 { get; set; }
        public string? t2 { get; set; }
        public string? d2 { get; set; }
        public string? t3 { get; set; }
        public string? d3 { get; set; }
        public string? t4 { get; set; }
        public string? d4 { get; set; }
        public string? t5 { get; set; }
        public string? d5 { get; set; }

        public bool is_pkd { get; set; }
        public bool is_shop { get; set; }
        public bool is_arv { get; set; }
        public bool is_po { get; set; }
        public bool status { get; set; }


        public int batch_id { get; set; } = 0;
        public string? barcode { get; set; }
        public string? lenght { get; set; }
        public string? width { get; set; }
        public string? height { get; set; }
        public int? lenght_unit { get; set; }
        public string? weight { get; set; }
        public string? case_size { get; set; }
        public decimal? pur_price { get; set; } = 0;
        public decimal? mrp { get; set; } = 0;
        public decimal? asso_price { get; set; } = 0;
        public decimal? cust_price { get; set; } = 0;
        public decimal? fran_price { get; set; } = 0;
        public decimal? b_bv { get; set; } = 0;
        public decimal? m_bv { get; set; } = 0;
        public decimal? f_bv { get; set; } = 0;
        public decimal? gst { get; set; }

        public string? img_name_1 { get; set; }
        public string? img_name_2 { get; set; }
        public string? img_name_3 { get; set; }
        public string? img_name_4 { get; set; }
        public string? img_name_5 { get; set; }
        public string? img_name_6 { get; set; }

        public bool is_trending { get; set; }
        public bool is_best_selling { get; set; }
        public string? seo_title { get; set; }
        public string? seo_description { get; set; }
        public string? ingredient_title { get; set; }

    }

    public class SettingView
    {
        public string? user_val { get; set; }
    }


    public class ProductView
    {
        public int pid { get; set; }

        [Required(ErrorMessage = "SKU is required")]
        [Display(Name = "SKU")]
        public string prod_code { get; set; } = string.Empty;

        [Required(ErrorMessage = "Product name is required")]
        [Display(Name = "Product Name")]
        public string prod_name { get; set; } = string.Empty;

        [Display(Name = "Product Description")]
        public string prod_desc { get; set; } = string.Empty;

        [Required(ErrorMessage = "Category is required")]
        [Display(Name = "Category ID")]
        public int cat_id { get; set; }

        public string? cat_name { get; set; } = string.Empty;

        [Display(Name = "Subcategory ID")]
        public int? sub_cat_id { get; set; }

        //public string? sub_cat_name { get; set; } = string.Empty;

        [Required(ErrorMessage = "HSN Code is required")]
        [Display(Name = "HSN Code")]
        public string hsncode { get; set; } = string.Empty;


        [Display(Name = "Batch")]
        public int batch_id { get; set; } = 0;

        [MaxLength(50)]
        [Display(Name = "Barcode")]
        public string? barcode { get; set; }

        [Display(Name = "Length")]
        public string? lenght { get; set; }

        [Display(Name = "Width")]
        public string? width { get; set; }

        [Display(Name = "Height")]
        public string? height { get; set; }

        [Display(Name = "Length Unit")]
        public int? lenght_unit { get; set; }

        [Display(Name = "Weight")]
        public string? weight { get; set; }


        [Display(Name = "Case Size")]
        public string? case_size { get; set; }

        [Required(ErrorMessage = "Purchase price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Purchase price must be greater than 0")]
        [Display(Name = "Purchase Price")]
        public decimal? pur_price { get; set; } = 0;

        //[Required(ErrorMessage = "MRP is required")]
        //[Range(0.01, double.MaxValue, ErrorMessage = "MRP must be greater than 0")]
        [Display(Name = "MRP")]
        public decimal? mrp { get; set; } = 0;

        //[Required(ErrorMessage = "Associate Price is required")]
        //[Range(0.01, double.MaxValue, ErrorMessage = "Associate price must be greater than 0")]
        [Display(Name = "Sales Rate")]
        public decimal? asso_price { get; set; } = 0;

        //[Required(ErrorMessage = "Customer Price is required")]
        //[Range(0.01, double.MaxValue, ErrorMessage = "Customer price must be greater than 0")]
        [Display(Name = "Customer Price")]
        public decimal? cust_price { get; set; } = 0;

        [Required(ErrorMessage = "Sales Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Sales price must be greater than 0")]
        [Display(Name = "Sales Price")]
        public decimal? fran_price { get; set; } = 0;

        [Range(0, double.MaxValue, ErrorMessage = "Invalid " + Const.B_BV)]
        [Display(Name = Const.B_BV)]
        public decimal? b_bv { get; set; } = 0;

        [Range(0, double.MaxValue, ErrorMessage = "Invalid " + Const.B_BV)]
        [Display(Name = Const.M_BV)]
        public decimal? m_bv { get; set; } = 0;

        [Range(0, double.MaxValue, ErrorMessage = "Invalid " + Const.F_BV)]
        [Display(Name = Const.F_BV)]
        public decimal? f_bv { get; set; } = 0;

        [Display(Name = "GST Rate")]
        public decimal? gst { get; set; }


        [Display(Name = "Default Show")]
        public bool default_show { get; set; }


        [Display(Name = "Image 1")]
        public IFormFile? img1 { get; set; }


        [Display(Name = "Image 2")]
        public IFormFile? img2 { get; set; }


        [Display(Name = "Image 3")]
        public IFormFile? img3 { get; set; }


        [Display(Name = "Image 4")]
        public IFormFile? img4 { get; set; }


        [Display(Name = "Image 5")]
        public IFormFile? img5 { get; set; }


        [Display(Name = "Image 6")]
        public IFormFile? img6 { get; set; }

        public string? img_name_1 { get; set; }
        public string? img_name_2 { get; set; }
        public string? img_name_3 { get; set; }
        public string? img_name_4 { get; set; }
        public string? img_name_5 { get; set; }
        public string? img_name_6 { get; set; }

        public string? t1 { get; set; }
        public string? d1 { get; set; }
        public string? t2 { get; set; }
        public string? d2 { get; set; }
        public string? t3 { get; set; }
        public string? d3 { get; set; }
        public string? t4 { get; set; }
        public string? d4 { get; set; }
        public string? t5 { get; set; }
        public string? d5 { get; set; }

       

       

        

        [Display(Name = "Is Purchase")]
        public bool is_po { get; set; }
       


        [Display(Name = "Combo Pack")]
        public bool is_pkd { get; set; }

        [Display(Name = "Is Shop")]
        public bool is_shop { get; set; }

        [Display(Name = "New Arrivals")]
        public bool is_arv { get; set; }

        [Display(Name = "Trending")]
        public bool is_trending { get; set; }

        [Display(Name = "Best Selling")]
        public bool is_best_selling { get; set; }

        [Display(Name = "SEO Title")]
        public string? seo_title { get; set; }
        [Display(Name = "SEO Description")]
        public string? seo_description { get; set; }
        [Display(Name = "Ingredient Title")]
        public string? ingredient_title { get; set; }

        [Display(Name = "Active Product")]
        public bool status { get; set; }
    }


    public class IngredientDto
    {
        public int? ingredient_id { get; set; }
        public int? pid { get; set; }
        public string? ingredient_name { get; set; }
        public string? ingredient_desc { get; set; }
        public int? display_order { get; set; }
        public string? ingredient_img { get; set; }
        public IFormFile upload_ingredient_img { get; set; } 
    }

   
    public class DB_Ingredient
    {
        public int? ingredient_id { get; set; }
        public int? pid { get; set; }
        public string? ingredient_name { get; set; }
        public string? ingredient_desc { get; set; }
        public int? display_order { get; set; }
        public string? ingredient_img { get; set; }
         
    }

    public class Variant_db
    {
        public int variant_id { get; set; } = 0;
        public int pid { get; set; } = 0;
        public int batch_id { get; set; } = 0;
        public int size_id { get; set; } = 0;
        public int color_id { get; set; } = 0;
        public string barcode { get; set; } = string.Empty;
        public string? lenght { get; set; } = string.Empty;
        public string? width { get; set; } = string.Empty;
        public string? height { get; set; } = string.Empty;
        public string lenght_unit { get; set; } = string.Empty;
        public string? weight { get; set; } = string.Empty;
        public string? case_size { get; set; } = string.Empty;
        public decimal? pur_price { get; set; }
        public decimal? mrp { get; set; }
        public decimal? asso_price { get; set; }
        public decimal? cust_price { get; set; }
        public decimal? fran_price { get; set; }
        public decimal? b_bv { get; set; }
        public decimal? m_bv { get; set; }
        public decimal? f_bv { get; set; }
        public decimal? gst { get; set; }

        public bool status { get; set; }
        public bool default_show { get; set; }
        public string? img_name_1 { get; set; }
        public string? img_name_2 { get; set; }
        public string? img_name_3 { get; set; }
        public string? img_name_4 { get; set; }
        public string? img_name_5 { get; set; }
        public string? img_name_6 { get; set; }

    }



    public class VariantView
    {
        public int variant_id { get; set; } = 0;

        [Required(ErrorMessage = "Product is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Product is required")]
        public int pid { get; set; }

        [Display(Name = "Size")]
        public int? size_id { get; set; } = 0;

        [Display(Name = "Color")]
        public int? color_id { get; set; } = 0;



    }



    public class VendorDto
    {
        public int? vendor_id { get; set; }
        public string? vendor_name { get; set; }
        public string? display_name { get; set; }
        public string? email_id { get; set; }
        public string? phone_no { get; set; }
        public string? mobile_id { get; set; }
        public string? source_supply { get; set; }
        public int? state_id { get; set; }
        public string? skype_name { get; set; }
        public string? designation { get; set; }
        public string? department { get; set; }
        public string? website { get; set; }
        public bool status { get; set; }
    }

    public class Vendor_db
    {
        public int? vendor_id { get; set; }
        public string? vendor_name { get; set; }
        public string? display_name { get; set; }
        public string? email_id { get; set; }
        public string? phone_no { get; set; }
        public string? mobile_id { get; set; }
        public int? state_id { get; set; }
        public string? skype_name { get; set; }
        public string? designation { get; set; }
        public string? department { get; set; }
        public string? website { get; set; }
        public bool status { get; set; }
    }


    public class UserListBasicDetailFilter
    {
        public string? user_id { get; set; }
    }

    public class DoctorFilter
    {
        public int? duid { get; set; }
    }

    public class UpdateOrderDetailFilter
    {
        public int? order_id { get; set; }
    }

    public class DoctorModel
    {
        public int duid { get; set; } // Identity Primary Key
        public string? name { get; set; }
        public string? address { get; set; }
        public int? state_id { get; set; }
        public string? state { get; set; }
        public string? city { get; set; }
        public string? pin_code { get; set; }
        public string? mobile { get; set; }
        public string? com_name { get; set; }
        public int? uid_ref { get; set; }
        public decimal? minimum_sales_monthly { get; set; }
        public string? dob { get; set; }
        public bool status { get; set; }
        public List<ProductCheckboxViewModel> Products { get; set; } = new();
    }

    public class ProductCheckboxViewModel
    {
        public int pid { get; set; }
        public string prod_name { get; set; }
        public bool is_selected { get; set; }
    }


    public class Doctor_DB_Request
    {
        public int? duid { get; set; } // Identity Primary Key
        public string? name { get; set; }
        public string? address { get; set; }
        public int? state_id { get; set; }
        public string? state { get; set; }
        public string? city { get; set; }
        public string? pin_code { get; set; }
        public string? mobile { get; set; }
        public string? com_name { get; set; }
        public int? uid_ref { get; set; }
        public decimal? minimum_sales_monthly { get; set; }
        public string? dob { get; set; }
        public bool status { get; set; }
        public string? pid_str { get; set; }
    }


    public class DoctorView
    {
        public int? duid { get; set; } // Identity Primary Key
        public string? name { get; set; }
        public string? address { get; set; }
        public string? state { get; set; }
        public string? city { get; set; }
        public string? pin_code { get; set; }
        public string? mobile { get; set; }
        public string? com_name { get; set; }
        public string? sales_person { get; set; }
        public decimal? minimum_sales_monthly { get; set; }
        public string? dob { get; set; }
        public bool status { get; set; }
        public string? pid_str { get; set; }
    }

    public class DB_UserRequest
    {
        public int? uid { get; set; }
        public string? user_id { get; set; }
        public int? login_uid { get; set; }
        public int? role_id { get; set; }
        public int? sub_role_id { get; set; }
        public string? sub_role_name { get; set; }
        public string? submit_type { get; set; } = "ROOT";
        public string? sponsor_user_id { get; set; }
        public string? p_side { get; set; }
        public string? name { get; set; }
        public string? com_name { get; set; }
        public string? password { get; set; }
        public string? tran_password { get; set; }
        public string? dob { get; set; }
        public string? address { get; set; }
        public int? state_id { get; set; }
        public int? dist_id { get; set; }
        public string? city { get; set; }
        public string? pin_code { get; set; }


        public string? mobile { get; set; }
        public string? email_id { get; set; }
        public string? gender { get; set; }
        public string? pan_no { get; set; }
        public string? gst_no { get; set; }
        public string? aadhar_no { get; set; }
        public int? bank_id { get; set; }
        public string? branch { get; set; }
        public string? account_no { get; set; }
        public string? ac_type { get; set; }
        public string? ifsc { get; set; }

        public string? nom_name { get; set; }
        public string? nom_rela { get; set; }
        public string? aadhar_img_1 { get; set; }
        public string? aadhar_img_2 { get; set; }
        public string? pan_img { get; set; }
        public string? bank_img { get; set; }
        public string? gst_img { get; set; }
        public string? pf_img { get; set; }
    }



    public class AddUpdateUserRequest
    {
        public int? uid { get; set; }
        public int? login_uid { get; set; }
        public int? role_id { get; set; }
        public int? sub_role_id { get; set; }
        public string? sub_role_name { get; set; }
        public string? submit_type { get; set; } = "ROOT"; 
        public string? sponsor_user_id { get; set; }
        public string? p_side { get; set; }
        public string? com_name { get; set; }

        // Account Info
        [Display(Name = "User Login ID")]
        public string? user_id { get; set; }

        [Required(ErrorMessage = "User name is required."), StringLength(100), Display(Name = "User Name")]
        public string? name { get; set; }
        public string? password { get; set; }
        public string? tran_password { get; set; }

        [Display(Name = "Date Of Birth")]
        public string? dob { get; set; }

        // Contact & Address
        [StringLength(200), Display(Name = "Address")]
        public string? address { get; set; }

        [Display(Name = "State")]
        public int? state_id { get; set; }

        [Display(Name = "District")]
        public int? dist_id { get; set; }

        [Display(Name = "City")]
        public string? city { get; set; }

        [Display(Name = "PIN Code")]
        public string? pin_code { get; set; }

        //[Required, Phone, Display(Name = "Mobile Number")]
        [Required(ErrorMessage = "Mobile number is required and will be used as your User ID."), Display(Name = "Mobile Number")]
        public string? mobile { get; set; }

        [Display(Name = "Email ID")]
        public string? email_id { get; set; }

        [Display(Name = "Gender")]
        public string? gender { get; set; }

        // Bank Details
        [Display(Name = "Bank Name")]
        public int? bank_id { get; set; }

        [Display(Name = "Branch")]
        public string? branch { get; set; }

        [Display(Name = "Account Number")]
        public string? account_no { get; set; }

        [Display(Name = "Account Type")]
        public string? ac_type { get; set; }

        [Display(Name = "IFSC Code")]
        public string? ifsc { get; set; }

        [Display(Name = "Nominee Name")]
        public string? nom_name { get; set; }

        [Display(Name = "Nominee Relation")]
        public string? nom_rela { get; set; }


        [StringLength(10, MinimumLength = 10), Display(Name = "PAN Number")]
        public string? pan_no { get; set; }

        [Display(Name = "Pan Image")]
        public IFormFile? upload_pan { get; set; }
        public string? pan_img { get; set; }

        [Display(Name = "Bank Image")]
        public IFormFile? upload_bank { get; set; }
        public string? bank_img { get; set; }

        [Display(Name = "Aadhar Number")]
        public string? aadhar_no { get; set; }

        [Display(Name = "Aadhar Front")]
        public IFormFile? upload_aadhar_1 { get; set; }
        public string? aadhar_img_1 { get; set; }

        [Display(Name = "Aadhar Back")]
        public IFormFile? upload_aadhar_2 { get; set; }
        public string? aadhar_img_2 { get; set; }

        [Display(Name = "User Profile")]
        public IFormFile? upload_pf_img { get; set; }
        public string? pf_img { get; set; }

        [Display(Name = "GST Number")]
        public string? gst_no { get; set; }

        [Display(Name = "GST Image")]
        public IFormFile? upload_gst_img { get; set; }
        public string? gst_img { get; set; }

        [Display(Name = "TIN Number")]
        public string? tin_no { get; set; }

        [Display(Name = "CIN Number")]
        public string? cin_no { get; set; }

        
        public int? pan_status { get; set; }
        public int? bank_status { get; set; }
        public int? aadhar_status { get; set; }
        public int? gst_status { get; set; }

        public string? s_name { get; set; }
        public string? s_mobile_no { get; set; }
        

    }


    public class VendorPurchaseReceivedDto
    {
        public int? purchase_id { get; set; } = 0;
        [Required(ErrorMessage = "Invoice date is required.")]
        public DateTime? vendor_inv_date { get; set; } = DateTime.UtcNow.AddDays(0);
        [Required(ErrorMessage = "Invoice number is required.")]
        public string? vendor_inv_no { get; set; } = string.Empty;
        public int? buyer_uid { get; set; } = 0;
        public bool status { get; set; }
    }

    public class VendorPurchase_DB
    {
        public int? cart_id { get; set; } = 0;
        public int? vendor_id { get; set; } = 0;
        public string? delivery_date { get; set; }
        public string? doe { get; set; }
        public string? invoice_no { get; set; } = string.Empty;
        public int? buyer_uid { get; set; } = 0;
    }




    public class AdminPurchaseDto
    {
        public int? cart_id { get; set; } = 0;
        public int? vendor_id { get; set; } = 0;

        //[Required(ErrorMessage = "Delivery date is required.")]
        public string? delivery_date { get; set; }

        //[Required(ErrorMessage = "Purchase Order date is required.")]
        public string? doe { get; set; }

        //[Required(ErrorMessage = "Invoice number is required.")]
        public string? invoice_no { get; set; } = string.Empty;
        public int? buyer_uid { get; set; } = 0;
    }


    public class add_cart_data
    {
        public int? cart_id { get; set; }
        public int? uid { get; set; }
        public int? prod_type { get; set; }
        public int? seller_uid { get; set; }
        public int? pid { get; set; }
        public int? qty { get; set; }
        public decimal? rate { get; set; }
        public int? cid { get; set; }
    }


    public class update_invoice_detail_id
    {
        public string? tableName { get; set; }
        public string? ColumnName { get; set; }
        public int? inv_det_id { get; set; }
        public int? val { get; set; }
    }



    public class delete_cart_data
    {
        public int? id { get; set; }
    }
    public class get_cart_data
    {
        public int? cid { get; set; }
        public int? variant_id { get; set; }
        public string? prod_code { get; set; } = string.Empty;
        public string? prod_name { get; set; } = string.Empty;
        public string? case_size { get; set; } = string.Empty;
        public string? weight { get; set; } = string.Empty;
        public double? mrp { get; set; }
        public int? qty { get; set; }
        public double? price { get; set; }
        public double? gross { get; set; }
        public double? gst { get; set; }
        public double? gst_amt { get; set; }
        public double? t_amount { get; set; }
        public double? t_b_bv { get; set; }
        public double? t_m_bv { get; set; }
        public double? t_f_bv { get; set; }
    }

    public class VendorPurchaseOrderDto
    {
        public int purchase_id { get; set; }
        public string purchase_order_no { get; set; }
        public string invoice_no { get; set; }
        public string vendor_name { get; set; }

        public string buyer_user_id { get; set; }
        public string buyer_name { get; set; }

        public decimal t_gross { get; set; }
        public decimal t_tax_rs { get; set; }
        public decimal amount { get; set; }
        public decimal t_amount { get; set; }
        public decimal adjust_amt { get; set; }

        public byte status { get; set; }
        public DateTime doe { get; set; }
    }


    public class AdminVendorPurchaseFilter
    {
        public string? purchase_order_no { get; set; }
        public int? vendor_id { get; set; }
        public int? buyer_uid { get; set; }
        public int? status { get; set; }
        public string? from_date { get; set; }
        public string? to_date { get; set; }
        public int? in_page_no { get; set; }
        public int? in_page_record { get; set; }
    }

    public class AdminPurchaseReceiveDto
    {
        public int? purchase_id { get; set; }
        public string? vendor_inv_date { get; set; }
        public string? vendor_inv_no { get; set; }
        public int? buyer_uid { get; set; }
        public bool status { get; set; }
    }

    [Keyless]
    public class inv_id_detail
    {
        public int? inv_id { get; set; } = 0;
    }



    public class UserProductMappingDB
    {
        public int? upm_id { get; set; }
        public int? pid { get; set; }
        public int? uid { get; set; }
        public decimal? fran_price { get; set; }
        public decimal? final_rate { get; set; }
        public decimal? scheme_1 { get; set; }
        public decimal? scheme_2 { get; set; }
        public decimal? scheme_3 { get; set; }
        public decimal? scheme_4 { get; set; }
        public decimal? perc { get; set; }
        public int? prod_qty { get; set; }
        public int? offer_qty { get; set; }
        public int? is_gst_inclusive { get; set; }
    }

    public class SelectProductForCart
    {
        public int? cart_id { get; set; }
        public int? uid { get; set; }
        public List<SelectProduct>? SelectProducts { get; set; }
    }

    public class SelectProduct
    {
        public int? pid { get; set; }
    }
    public class UserProductMappingDto
    {
        public int? upm_id { get; set; }
        public int? pid { get; set; }
        public int? uid { get; set; }
        public string? cat_name { get; set; }
        public string? prod_code { get; set; }
        public string? prod_name { get; set; }
        public string? user_id { get; set; }
        public string? name { get; set; }
        public decimal? gst { get; set; }
        public decimal? fran_price { get; set; }
        public decimal? final_rate { get; set; }
        public decimal? scheme_1 { get; set; }
        public decimal? scheme_2 { get; set; }
        public decimal? scheme_3 { get; set; }
        public decimal? scheme_4 { get; set; }
        public int? prod_qty { get; set; }
        public int? offer_qty { get; set; }
        public int? is_gst_inclusive { get; set; }
        public decimal? perc { get; set; }

    }

    public class AddExpensesCategoryDto
    {
        public int? ecid { get; set; }
        public int? uid { get; set; }
        public string? exp_cat { get; set; }
        public decimal? amount { get; set; }
        public string? doe { get; set; }
        public string? remarks { get; set; }

    }

    public class ExpensesMappingDto
    {
        public int? eid { get; set; }
        public int? ecid { get; set; }
        public string? name { get; set; }
        public string? exp_cat { get; set; }
        public decimal? amount { get; set; }
        public string? doe { get; set; }
        public string? remarks { get; set; }

    }


    public class ShippingAddressDto
    {
        public int? said { get; set; }
        public int? uid { get; set; }
        public string? name { get; set; }
        public string? mobile { get; set; }
        public string? address { get; set; }
        public int? state_id { get; set; }
        public string? state { get; set; }
        public int? dist_id { get; set; }
        public string? district { get; set; }
        public string? city { get; set; }
        public string? pin_code { get; set; }
    }

    public class GetOrderInvFilter
    {
        public int? inv_gen { get; set; }
        public string? ord_inv_no { get; set; }
    }
    public class GetOrderInvModel
    {
        public int? ord_inv_id { get; set; }
        public int? inv_gen { get; set; }
        public string? ord_inv_no { get; set; }
        public string? pay_mode { get; set; }
        public DateTime? doe { get; set; }
        public string? t_amount { get; set; } 
    }

    public class GenerateOrderDto
    {
        public int? said { get; set; }
        public int? cart_id { get; set; } = 0;
        public int? inv_type { get; set; } = 0;
        public string? user_id { get; set; } = string.Empty;
        public string? name { get; set; } = string.Empty;
        public int? buyer_uid { get; set; }
        public int? seller_uid { get; set; }
        public string? gst_no { get; set; } = string.Empty;
        //[Required(ErrorMessage = "User name is required.")]
        public string? s_name { get; set; } = string.Empty; 
        public string? s_mobile_no { get; set; } = string.Empty;
         
        public int? state_id { get; set; } 
        public int? dist_id { get; set; }
        public string? s_city { get; set; } = string.Empty;

        /// <summary>
        /// [Required(ErrorMessage = "Address is required.")]
        /// </summary>
        public string? s_address { get; set; } = string.Empty;
        //[Required(ErrorMessage = "Pincode is required.")]
        public string? s_pincode { get; set; } = string.Empty;
        //[Range(1, int.MaxValue, ErrorMessage = "Select a valid payment method.")]
        public int? pay_id { get; set; }
        public string? bank_name { get; set; } = string.Empty;
        public string? check_no { get; set; } = string.Empty;
        public string? checkdate { get; set; }
        public decimal? scheme { get; set; } = 0;
        public decimal? discount { get; set; } = 0;
        public decimal? del_charge { get; set; } = 0;
        public decimal? net_delivery_charge { get; set; } = 0;
        public decimal? adjust_amt { get; set; } = 0;
        public string? coupon { get; set; } = string.Empty;
        public string? img_name { get; set; }

        public IFormFile? upload_pay_slip { get; set; }
        public string? panel { get; set; }
        public string? order_no { get; set; }
        public int? status { get; set; }
        public string? appr_by { get; set; }
        public string? remark { get; set; }

        public string? img_utr_no { get; set; }
        public string? img_utr_amount { get; set; }
        public string? img_utr_date { get; set; }
        public string? img_utr_full_text { get; set; }
        public string? img_utr_sender_name { get; set; }
        public string? img_utr_upi_id { get; set; }
    }


    public class GenerateOrderDB
    {
        public int? cart_id { get; set; } = 0;
        public int? inv_type { get; set; } = 0;
        public string? user_id { get; set; } = string.Empty;
        public int? buyer_uid { get; set; } = 0;
        public int? seller_uid { get; set; } = 0;
        public string? gst_no { get; set; } = string.Empty;
        public string? s_name { get; set; } = string.Empty;
        public string? s_mobile_no { get; set; } = string.Empty;
        public int? state_id { get; set; }
        public int? dist_id { get; set; }
        public string? s_city { get; set; } = string.Empty;
        public string? s_address { get; set; } = string.Empty;
        public string? s_pincode { get; set; } = string.Empty;
        public int? pay_id { get; set; } = 0;
        public string? bank_name { get; set; } = string.Empty;
        public string? check_no { get; set; } = string.Empty;
        public DateTime? checkdate { get; set; }
        public decimal? scheme { get; set; } = 0;
        public decimal? discount { get; set; } = 0;
        public decimal? del_charge { get; set; } = 0;
        public decimal? net_delivery_charge { get; set; } = 0;
        public decimal? adjust_amt { get; set; } = 0;
        public string? coupon { get; set; } = string.Empty;

        public string? img_name { get; set; }
        public string? panel { get; set; }
        public string? appr_by { get; set; }
    }



    public class OrderDetailFromDB
    {
        public string? order_no { get; set; }
        public int? inv_type { get; set; }
        public int? buyer_uid { get; set; }
        public int? seller_uid { get; set; }
    }

    public class GenerateInvoiceDto
    {
        public int? is_old_inv { get; set; }
        public string? invoice_no { get; set; }
        public string? doe { get; set; }


        public string? order_no { get; set; }
        public int? cart_id { get; set; } = 0;
        public int? inv_type { get; set; }
        public string? user_id { get; set; } = string.Empty;
        public string? name { get; set; } = string.Empty;
        public int? buyer_uid { get; set; }
        public int? seller_uid { get; set; }
        public string? gst_no { get; set; } = string.Empty;
        [Required(ErrorMessage = "User name is required.")]
        public string? s_name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mobile No. is required.")]
        public string? s_mobile_no { get; set; } = string.Empty;

        [Required(ErrorMessage = "State is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid state.")]
        public int? state_id { get; set; }

        [Required(ErrorMessage = "District is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid district.")]
        public int? dist_id { get; set; }
        public string? s_city { get; set; } = string.Empty;

        [Required(ErrorMessage = "Address is required.")]
        public string? s_address { get; set; } = string.Empty;
        [Required(ErrorMessage = "Pincode is required.")]
        public string? s_pincode { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "Select a valid payment method.")]
        public int? pay_id { get; set; }
        public string? bank_name { get; set; } = string.Empty;
        public string? check_no { get; set; } = string.Empty;
        public DateTime? checkdate { get; set; }

        public decimal? receive_amount { get; set; } = 0;
        public decimal? scheme { get; set; } = 0;
        public decimal? discount { get; set; } = 0;
        public decimal? del_charge_tax { get; set; } = 0;
        public decimal? del_charge { get; set; } = 0;
        public decimal? net_delivery_charge { get; set; } = 0;
        public decimal? adjust_amt { get; set; } = 0;
        //public decimal? PaybleAmount { get; set; } = 0;
        public string? coupon { get; set; } = string.Empty;
        public string? img_name { get; set; }
        public string? panel { get; set; }
        public int? order_id { get; set; }
        public int? status { get; set; }
        public string? appr_by { get; set; }
        public string? remark { get; set; }

    }

    public class GenerateInvoiceDB
    {
        public int? is_old_inv { get; set; }
        public string? invoice_no { get; set; }
        public string? doe { get; set; }
        public int? cart_id { get; set; } = 0;
        public int? inv_type { get; set; } = 0;
        public string? user_id { get; set; } = string.Empty;
        public int? buyer_uid { get; set; } = 0;
        public int? seller_uid { get; set; } = 0;
        public string? gst_no { get; set; } = string.Empty;
        public string? s_name { get; set; } = string.Empty;
        public string? s_mobile_no { get; set; } = string.Empty;
        public int? state_id { get; set; }
        public int? dist_id { get; set; }
        public string? s_city { get; set; } = string.Empty;
        public string? s_address { get; set; } = string.Empty;
        public string? s_pincode { get; set; } = string.Empty;
        public int? pay_id { get; set; } = 0;
        public string? bank_name { get; set; } = string.Empty;
        public string? check_no { get; set; } = string.Empty;
        public DateTime? checkdate { get; set; }
        public decimal? receive_amount { get; set; } = 0;
        public decimal? scheme { get; set; } = 0;
        public decimal? discount { get; set; } = 0;
        public decimal? del_charge { get; set; } = 0;
        public decimal? net_delivery_charge { get; set; } = 0;
        public decimal? adjust_amt { get; set; } = 0;
        public string? coupon { get; set; } = string.Empty;
        public int? order_id { get; set; }
        public string? img_name { get; set; }
        public string? panel { get; set; }
        public string? appr_by { get; set; }
        public string? remark { get; set; }
    }


    public class OrderListFilter
    {
        public string? from_date { get; set; }
        public string? to_date { get; set; }
        public int? uid { get; set; }
        public string? buyer_user_id { get; set; }
        public string? seller_user_id { get; set; }
        public int? buyer_uid { get; set; }
        public int? seller_uid { get; set; }
        public string? order_no { get; set; }
        public string? mobile_no { get; set; }
        public int? status { get; set; }
    }


    public class UserOrderListFilter
    {
        public string? from_date { get; set; }
        public string? to_date { get; set; }
        public int? uid { get; set; }
        public string? buyer_user_id { get; set; }
        public string? seller_user_id { get; set; }
        public int? buyer_uid { get; set; }
        public int? seller_uid { get; set; }
        public string? order_no { get; set; }
        public string? mobile_no { get; set; }
        public int? status { get; set; }
    }


    public class InvoiceListFilter
    {
        public string? invoice_no { get; set; }
        public int? uid { get; set; }
        public string? buyer_user_id { get; set; }
        public string? seller_user_id { get; set; }
        public int? buyer_uid { get; set; }
        public int? seller_uid { get; set; }
        public int? status { get; set; }
        public string? from_date { get; set; }
        public string? to_date { get; set; }

    }

    public class UserInvoiceListFilter
    {
        public string? invoice_no { get; set; }
        public int? uid { get; set; }
        public string? buyer_user_id { get; set; }
        public string? seller_user_id { get; set; }
        public int? buyer_uid { get; set; }
        public int? seller_uid { get; set; }
        public int? status { get; set; }
        public string? from_date { get; set; }
        public string? to_date { get; set; }

    }

    public class UserWiseCommissionFilter
    {
        public int? uid { get; set; }
        public string? seller_user_id { get; set; }
        public string? from_date { get; set; }
        public string? to_date { get; set; }

    }

    public class WalletTransactionFilter
    {
        public string? from_date { get; set; }
        public string? to_date { get; set; }
        public string? d_c { get; set; } // 'D' or 'C'
        public int? mode_id { get; set; }
        public int? uid { get; set; }
        public string? user_id { get; set; }
        public int? in_page_no { get; set; } = 1;
        public int? in_page_record { get; set; } = 10;
    }


    public class FranWalletTransactionModel
    {
        public int? tran_id { get; set; }
        public string? user_id { get; set; }
        public string? name { get; set; }
        public int mode_id { get; set; }
        public string? d_c { get; set; }
        public decimal? debit { get; set; }
        public decimal? credit { get; set; }
        public decimal? balance { get; set; }
        public DateTime? doe { get; set; }
        public string? remarks { get; set; }
        public string? tran_no { get; set; }
        public string? voucher_type { get; set; }
    }
    public class OrderIdRequestModel
    {
        public int? order_id { get; set; }
    }
    public class AdminOrderProductViewModel
    {
        public string? prod_code { get; set; }
        public string? prod_name { get; set; }
        public string? hsncode { get; set; }
        public string? pack_size { get; set; }
        public int? actual_qty { get; set; }
        public int? qty { get; set; }
        public decimal? mrp { get; set; }
        public decimal? price { get; set; }
        public decimal? gross { get; set; }
        public decimal? gst { get; set; }
        public decimal? gst_amt { get; set; }
        public decimal? total_amt { get; set; }
        public int? prod_type { get; set; }
        public string? mfg_date { get; set; }
        public string? exp_date { get; set; }
    }

    public class UserOrderProductViewModel
    {
        public string? prod_code { get; set; }
        public string? prod_name { get; set; }
        public string? hsncode { get; set; }
        public string? pack_size { get; set; }
        public int? actual_qty { get; set; }
        public int? qty { get; set; }
        public decimal? mrp { get; set; }
        public decimal? price { get; set; }
        public decimal? gross { get; set; }
        public decimal? gst { get; set; }
        public decimal? gst_amt { get; set; }
        public decimal? total_amt { get; set; }
        public int? prod_type { get; set; }
        public string? mfg_date { get; set; }
        public string? exp_date { get; set; }
    }
    public class InvoiceIdRequestModel
    {
        public int? inv_id { get; set; }
    }
    public class AdminInvoiceProductViewModel
    {
        public string? prod_code { get; set; }
        public string? prod_name { get; set; }
        public string? hsncode { get; set; }
        public string? pack_size { get; set; }
        public int qty { get; set; }
        public int actual_qty { get; set; }
        public decimal perc { get; set; }
        public decimal comm { get; set; }
        public decimal mrp { get; set; }
        public decimal price { get; set; }
        public decimal gross { get; set; }
        public decimal gst { get; set; }
        public decimal gst_amt { get; set; }
        public decimal total_amt { get; set; }
        public int prod_type { get; set; }
        public string? mfg_date { get; set; }
        public string? exp_date { get; set; }
    }

    public class StockListFilter
    {
        public int? uid { get; set; }
    }

    public class UserBinaryFilter
    {
        public string? user_id { get; set; }
    }
    public class UserDownlineBinaryReport
    {
        public int? uid { get; set; }
        public string? user_id { get; set; }
        public string? name { get; set; }
        public string? side { get; set; }
        public string? sponsor_user_id { get; set; }
        public string? sponsor_name { get; set; }
        public int? status { get; set; }
        public int? is_paid { get; set; }
        public decimal? total_left { get; set; }
        public decimal? total_right { get; set; }
        public decimal? new_left { get; set; }
        public decimal? new_right { get; set; }
        public decimal? pbv { get; set; }
        public decimal? gbv { get; set; }
        public decimal? prev_pbv { get; set; }
        public decimal? prev_gbv { get; set; }
        public string? rank_name { get; set; }

    }
    public class UserDownlineReport
    {
        public int? uid { get; set; }
        public string? user_id { get; set; }
        public string? name { get; set; }
        public string? side { get; set; }
        public string? sponsor_user_id { get; set; }
        public string? sponsor_name { get; set; }
        public int? status { get; set; }
        public int? is_paid { get; set; }
        public decimal? total_left { get; set; }
        public decimal? total_right { get; set; }
        public decimal? new_left { get; set; }
        public decimal? new_right { get; set; }
        public decimal? pbv { get; set; }
        public decimal? gbv { get; set; }
        public decimal? prev_pbv { get; set; }
        public decimal? prev_gbv { get; set; }
        public string? rank_name { get; set; }

    }

    public class UserMatrixFilter
    {
        public string? user_id { get; set; }
    }
    public class SalesDashboardValueFilter
    {
        public string? from_date { get; set; }
        public string? to_date { get; set; }
        public int? uid { get; set; }

    }



    public class SalesCollectionGraphFilter
    {
        public string? from_date { get; set; }
        public string? to_date { get; set; }
        public int? uid { get; set; }

    }


    public class StockTranViewModel
    {
        public string? user_id { get; set; }
        public string? name { get; set; }
        public string? prod_name { get; set; }
        public string? product_with_packsize { get; set; }
        public int? qty { get; set; }
        public int? product_qty { get; set; }
        public DateTime? doe { get; set; }
        public string? invoice_no { get; set; }
    }


    public class StockTranRequestModel
    {
        public int? uid { get; set; }
        public int? pid { get; set; }
    }





    public class UpdateInvoiceDto
    {
        public int? inv_id { get; set; }
        public string? invoice_no { get; set; }
        public decimal? t_amount { get; set; }

        public decimal? commission { get; set; }
        public string? doe { get; set; }

        public int? delivery_status { get; set; }
        public string? transport { get; set; }
        public string? tracking { get; set; }
        public string? dispatch_date { get; set; }
        public string? del_by { get; set; }

        public IFormFile? upload_lr_1 { get; set; }
        public IFormFile? upload_lr_2 { get; set; }
        public string? lr_1 { get; set; }
        public string? lr_2 { get; set; }
        public decimal? receive_amount { get; set; }
        /* 
         public int? pay_id { get; set; }
         public string? bank_name { get; set; } 
         public string? check_no { get; set; }  
         public DateTime? checkdate { get; set; } */

        public string? remark { get; set; }
    }



    public class ReceivePaymentDto
    {
        public int? buyer_uid { get; set; }
        public int? voucher_id { get; set; }
        public decimal? receive_amount { get; set; }
        public string? particulars { get; set; }
        public string? voucher_no { get; set; }
        public string? doe { get; set; }
        public string? login_user_id { get; set; }
    }


    public class ChangeSponsor
    {
        public int? user_type { get; set; }
        public int? uid { get; set; }
        public int? sponsor_uid { get; set; }
        public string? user_id { get; set; }
        public string? login_user_id { get; set; }
    }

    public class ChangePassword
    {
        public string? old_password { get; set; }
        public string? password { get; set; }
        public int? uid { get; set; }
    }


    public class DispatchedProductDB
    {
        public int? inv_id { get; set; }
        public int? delivery_status { get; set; }
        public string? transport { get; set; }
        public string? tracking { get; set; }
        public string? del_by { get; set; }
        //public IFormFile? upload_lr_1 { get; set; }
        //public IFormFile? upload_lr_2 { get; set; }
        public string? lr_1 { get; set; }
        public string? lr_2 { get; set; }
    }
    public class buyer_uid_filter
    {
        public int? buyer_uid { get; set; }
        public string? from_date { get; set; }
        public string? to_date { get; set; }
    }

    public class PendingInvoiceFilter
    {
        public string? buyer_uid { get; set; }
        public string? from_date { get; set; }
        public string? to_date { get; set; }

    }

    public class PartyWiseOutstandingFilter
    {
        public string? from_date { get; set; }
        public string? to_date { get; set; }
        public int? uid { get; set; }
    }


    public class PartyWiseOutstandingViewModel
    {
        public string user_id { get; set; }
        public string name { get; set; }
        public decimal outstanding { get; set; }
        public decimal collection { get; set; }
        public decimal targert { get; set; }
        public decimal sales { get; set; }
    }


    public class ExpensesFilter
    {
        public int? eid { get; set; }

    }

    public class CancelInvoiceFilter
    {
        public int? inv_id { get; set; }
        public string? login_user_id { get; set; }

    }

    public class PendingInvoiceResult
    {
        public int inv_id { get; set; }
        public string invoice_no { get; set; }
        public DateTime doe { get; set; }
        public string buyer_name { get; set; }
        public decimal due_amount { get; set; }
        public int due_days { get; set; }
    }


    public class FranInvoiceViewModel
    {
        public int inv_id { get; set; }
        public string invoice_no { get; set; }
        public string order_no { get; set; }

        public string buyer_user_id { get; set; }
        public string buyer_name { get; set; }
        public string buyer_mobile { get; set; }

        public string seller_user_id { get; set; }
        public string seller_name { get; set; }

        public decimal t_gross { get; set; }
        public decimal t_tax_rs { get; set; }
        public decimal amount { get; set; }
        public decimal del_charge { get; set; }
        public decimal discount { get; set; }
        public decimal adjust_amt { get; set; }
        public decimal t_amount { get; set; }

        public string inv_type { get; set; }
        public byte status { get; set; }

        public DateTime doe { get; set; }

        public string pay_mode { get; set; }

        public string seller_state { get; set; }
        public string buyer_state { get; set; }

        public string bank_name { get; set; }
        public string check_no { get; set; }
        public DateTime? checkdate { get; set; }

        public string s_name { get; set; }
        public string s_mobile_no { get; set; }
        public string s_state { get; set; }
        public string s_city { get; set; }
        public string s_district { get; set; }
        public string s_address { get; set; }
        public string s_pincode { get; set; }

        public string gst_no { get; set; }

        public decimal receive_amount { get; set; }
        public decimal commission { get; set; }

        public decimal due_amount { get; set; }
        public int due_days { get; set; }

        public string delivery_status { get; set; }
        public string transport { get; set; }
        public string tracking { get; set; }
        public string del_by { get; set; }

        public DateTime? dispatch_date { get; set; }
        public string lr_1 { get; set; }
        public string lr_2 { get; set; }
    }



    public class UserInvoiceViewModel
    {
        public int inv_id { get; set; }
        public string invoice_no { get; set; }
        public string order_no { get; set; }

        public string buyer_user_id { get; set; }
        public string buyer_name { get; set; }
        public string buyer_mobile { get; set; }

        public string seller_user_id { get; set; }
        public string seller_name { get; set; }
        public decimal t_bv { get; set; }
        public decimal t_gross { get; set; }
        public decimal t_tax_rs { get; set; }
        public decimal amount { get; set; }
        public decimal del_charge { get; set; }
        public decimal discount { get; set; }
        public decimal adjust_amt { get; set; }
        public decimal t_amount { get; set; }

        public string inv_type { get; set; }
        public byte status { get; set; }

        public DateTime doe { get; set; }

        public string pay_mode { get; set; }

        public string seller_state { get; set; }
        public string buyer_state { get; set; }

        public string bank_name { get; set; }
        public string check_no { get; set; }
        public DateTime? checkdate { get; set; }

        public string s_name { get; set; }
        public string s_mobile_no { get; set; }
        public string s_state { get; set; }
        public string s_city { get; set; }
        public string s_district { get; set; }
        public string s_address { get; set; }
        public string s_pincode { get; set; }

        public string gst_no { get; set; }

        public decimal receive_amount { get; set; }
        public decimal commission { get; set; }

        public decimal due_amount { get; set; }
        public int due_days { get; set; }

        public string delivery_status { get; set; }
        public string transport { get; set; }
        public string tracking { get; set; }
        public string del_by { get; set; }

        public DateTime? dispatch_date { get; set; }
        public string lr_1 { get; set; }
        public string lr_2 { get; set; }
    }

    public class UserWiseCommissionViewModel
    {
        public string? doe { get; set; }
        public string? seller_name { get; set; }
        public decimal? t_gross { get; set; }
        public decimal? t_tax_rs { get; set; }
        public decimal? amount { get; set; }
        public decimal? t_amount { get; set; }
        public decimal? receive_amount { get; set; }
        public decimal? commission { get; set; }

    }

    public class UserOrderViewModel
    {
        public int order_id { get; set; }
        public string order_no { get; set; }

        public int buyer_uid { get; set; }
        public int seller_uid { get; set; }

        public decimal t_gross { get; set; }
        public decimal t_tax_rs { get; set; }
        public decimal amount { get; set; }
        public decimal t_amount { get; set; }

        public string gst_no { get; set; }
        public decimal adjust_amt { get; set; }

        public byte status { get; set; }
        public DateTime doe { get; set; }

        public string buyer_user_id { get; set; }
        public string buyer_name { get; set; }

        public string seller_user_id { get; set; }
        public string seller_name { get; set; }
    }


    public class FranOrderViewModel
    {
        public int order_id { get; set; }
        public string order_no { get; set; }

        public int buyer_uid { get; set; }
        public int seller_uid { get; set; }

        public decimal t_gross { get; set; }
        public decimal t_tax_rs { get; set; }
        public decimal amount { get; set; }
        public decimal t_amount { get; set; }

        public string gst_no { get; set; }
        public decimal adjust_amt { get; set; }

        public byte status { get; set; }
        public DateTime doe { get; set; }

        public string buyer_user_id { get; set; }
        public string buyer_name { get; set; }

        public string seller_user_id { get; set; }
        public string seller_name { get; set; }
    }

    public class DueInvoiceViewModel
    {
        public int inv_id { get; set; }              // i.inv_id
        public string invoice_no { get; set; }       // i.invoice_no
        public DateTime doe { get; set; }            // i.doe
        public string buyer_name { get; set; }       // bu.com_name as buyer_name
        public decimal due_amount { get; set; }      // (i.t_amount - i.receive_amount)
        public int due_days { get; set; }            // DATEDIFF(DAY, i.doe, GETDATE())
    }


    public class UserReportDto
    {
        public int uid { get; set; }
        public string user_id { get; set; }
        public string com_name { get; set; }
        public string name { get; set; }
        public string display_name { get; set; }
        public string mobile { get; set; }
        public string email_id { get; set; }
        public string password { get; set; }
        public DateTime? dob { get; set; }
        public string status { get; set; }
        public bool is_paid { get; set; }
        public DateTime doe { get; set; }

        public string sponsor_user_id { get; set; }
        public string sponsor_name { get; set; }

        public string gender { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string district { get; set; }
        public string pin_code { get; set; }

        public string tin_no { get; set; }
        public string cin_no { get; set; }
        public string gst_no { get; set; }
        public string aadhar_no { get; set; }

        public string bank_name { get; set; }
        public string account_no { get; set; }
        public string ifsc { get; set; }
        public string pan_no { get; set; }

        public string bank_status { get; set; }
        public string pan_status { get; set; }
        public string aadhar_status { get; set; }
        public string gst_status { get; set; }

        public string bank_img { get; set; }
        public string pan_img { get; set; }
        public string gst_img { get; set; }
        public string aadhar_img_1 { get; set; }
        public string aadhar_img_2 { get; set; }
        public string pf_img { get; set; }
        public decimal? prev_pbv { get; set; }
        public string rank_name { get; set; }
    }

    public class StockReportDto
    {
        public string user_id { get; set; }
        public string user_name { get; set; }
        public string prod_name { get; set; }
        public string product_with_packsize { get; set; }
        public decimal product_qty { get; set; }
        public int pid { get; set; }
        public int uid { get; set; }
    }


    public class MonthWiseProductQtyFilter
    {
        public string? from_date { get; set; }
        public string? to_date { get; set; }
        public int? sub_role_id { get; set; }
        public int? seller_uid { get; set; }
        public int? prod_type { get; set; }
        public int? uid { get; set; }
    }


    public class MonthWiseProductQtyModel
    {
        public string? prod_code { get; set; }
        public string? prod_name { get; set; }
        public Dictionary<string, decimal>? month_qty { get; set; }
    }
    public class SalesCollectionFilter
    {
        public string? from_date { get; set; }
        public string? to_date { get; set; }
        public int? sub_role_id { get; set; }
        public int? mode_id { get; set; }
        public int? uid { get; set; }
    }


    public class ProfitLossFilter
    {
        public string? from_date { get; set; }
        public string? to_date { get; set; }
        public int? uid { get; set; }
    }


    public class SalesCollectionCommisssionFilter
    {
        public string? from_date { get; set; }
        public string? to_date { get; set; }
        public int? sub_role_id { get; set; }
        public int? mode_id { get; set; }
        public int? uid { get; set; }
    }
    public class SalesCollectionModel
    {
        public string? name { get; set; }
        public Dictionary<string, decimal>? month_qty { get; set; }
    }

    public class ProfitLossModel
    {
        public int? sno { get; set; }
        public string? name { get; set; }
        public Dictionary<string, decimal>? month_qty { get; set; }
    }


    public class SalesCollectionCommissionModel
    {
        public string? name { get; set; }
        public Dictionary<string, decimal>? month_qty { get; set; }
    }

    public class VendorDetailRequestModel
    {
        public int? purchase_id { get; set; }
    }


    public class UserTarggetFilter
    {
        public int? uid { get; set; }

    }
    public class UserTarggetDto
    {
        public int? uid { get; set; }
        public int? sponsor_uid { get; set; }
        public string? name { get; set; }
        public string? s_name { get; set; }
        public decimal? prev_pbv { get; set; }
    }


    public class InvoiceDetails_DB
    {

        public int? inv_id { get; set; }
        public string? name { get; set; }
        public string? mobile { get; set; }
        public string? seller_mobile { get; set; }
        public string? invoice_no { get; set; }
        public string? t_amount { get; set; }
        public string? transport { get; set; }
        public string? tracking { get; set; }
        public string? lr_1 { get; set; }
        public string? doe { get; set; }
        public string? dispatch_date { get; set; }

    }


    public class UserDetailsFromDB
    {
        public int? uid { get; set; }
        public string? user_id { get; set; }
        public string? name { get; set; }
        public string? com_name { get; set; }
        public string? mobile { get; set; }
        public string? company_wallet { get; set; }
        public string? company_due { get; set; }


    }

    public class OrderDetails_DB
    {

        public int? order_id { get; set; }
        public string? name { get; set; }
        public string? mobile { get; set; }
        public string? seller_mobile { get; set; }
        public string? order_no { get; set; }
        public string? t_amount { get; set; }
        public string? doe { get; set; } 
    }

    [Keyless]
    public class get_root_product
    { 
        public int? seller_uid { get; set; }
        public int? uid { get; set; }
        public int? cat_id { get; set; }
        public int? sub_cat_id { get; set; } 
        public string? fetch_type { get; set; }
    }

    [Keyless]
    public class get_root_product_detail
    {
        public string? seo_url { get; set; }
        public int? seller_uid { get; set; }
        public int? pid { get; set; }
        public int? size_id { get; set; }
        public int? color_id { get; set; }
    }


    public class ProductViewModel
    {
        public int? variant_id { get; set; }
        public int? pid { get; set; }
        public int? size_id { get; set; }
        public int? color_id { get; set; }
        public int? variant_qty { get; set; }
        public string? prod_name { get; set; }
        public string? prod_code { get; set; }
        public string? size { get; set; }
        public string? color_name { get; set; }
        public string? color_code { get; set; }
        public decimal? mrp { get; set; }
        public decimal? price { get; set; }
        public decimal? bv { get; set; }
        public string? img1 { get; set; }
        public double? rating { get; set; }
        public int? review { get; set; }
        public double? discount { get; set; } 
        public string? seo_url { get; set; } 
    }

    public class ProductDetailViewModel
    {
        public int? variant_id { get; set; }
        public string? cat_name { get; set; }
        public int? pid { get; set; }
        public int? size_id { get; set; }
        public int? color_id { get; set; }
        public int? variant_qty { get; set; }
        public int? product_qty { get; set; }
        public string? prod_name { get; set; }
        public string? prod_code { get; set; }
        public string? prod_desc { get; set; }
        public string? hsncode { get; set; }
        public string? size { get; set; }
        public string? color_name { get; set; }
        public string? color_code { get; set; }

        public decimal? mrp { get; set; }
        public decimal? price { get; set; }
        public decimal? bv { get; set; }
        public double? rating { get; set; }
        public int? review { get; set; }
        public double? discount { get; set; } 
        
        public string? img1 { get; set; }
        public string? img2 { get; set; }
        public string? img3 { get; set; }
        public string? img4 { get; set; }
        public string? img5 { get; set; }
        public string? img6 { get; set; }
        public string? t1 { get; set; }
        public string? t2 { get; set; }
        public string? t3 { get; set; }
        public string? t4 { get; set; }
        public string? t5 { get; set; }
        public string? d1 { get; set; }
        public string? d2 { get; set; }
        public string? d3 { get; set; }
        public string? d4 { get; set; }
        public string? d5 { get; set; }

        public string? ingredient_title { get; set; }
        public List<string> ImageList { get; set; } = new();
        public List<string> Features { get; set; } = new();
        //public Dictionary<string, string> Specifications { get; set; } = new();
    }

    public class BinaryTeamFilter
    {
        public string? user_id { get; set; }
        public string? login_user_id { get; set; }

    }
    public class BinaryTeamTreeViewModel
    {
        public BinaryUserDto? node1 { get; set; }
        public BinaryUserDto? node2 { get; set; }
        public BinaryUserDto? node3 { get; set; }
        public BinaryUserDto? node4 { get; set; }
        public BinaryUserDto? node5 { get; set; }
        public BinaryUserDto? node6 { get; set; }
        public BinaryUserDto? node7 { get; set; }

    }

    public class BinaryUserDto
    {
        public int? uid { get; set; }
        public int? parent_uid { get; set; }
        public int? node { get; set; }
        public string? user_id { get; set; }
        public string? name { get; set; }
        public bool? is_paid { get; set; }
        public int? status { get; set; }
        public string? doe { get; set; }
        public string? p_side { get; set; }
        public decimal? new_left { get; set; }
        public decimal? new_right { get; set; }
        public decimal? prev_left { get; set; }
        public decimal? prev_right { get; set; }
        public string? rank_name { get; set; }
    }


    public class MatrixUserDto
    {
        public int? uid { get; set; }
        public int? sponsor_uid { get; set; }
        public int? Level { get; set; }
        public string? user_id { get; set; }
        public string? name { get; set; }
        public bool? is_paid { get; set; }
        public int? status { get; set; }
        public string? doe { get; set; }
        public decimal? pbv { get; set; }
        public decimal? gbv { get; set; }
        public decimal? prev_pbv { get; set; }
        public decimal? prev_gbv { get; set; }
        public string? rank_name { get; set; }
        public List<MatrixUserDto> Children { get; set; } = new();
    }


















    public class MakePayoutData
    {
        [Required(ErrorMessage = "Payout from date is required")]
        public string? from_date { get; set; }

        [Required(ErrorMessage = "Payout to date is required")]
        public string? to_date { get; set; }
    }


    public class matrix_payout_date_ViewModel
    {
        public int? payout_no { get; set; }
        public string? from_date { get; set; }
        public string? to_date { get; set; }
        public string? doe { get; set; }
        public bool? status { get; set; }
    }

    public class MatrixPayoutDispatchViewModel
    {
        public int? bmp_id { get; set; }
        public int? uid { get; set; }
        public int? payout_no { get; set; }
        public string? user_id { get; set; }
        public string? user_name { get; set; }
        public string? from_date { get; set; }
        public string? to_date { get; set; }
        public string? payout_period { get; set; }

        public decimal? collection_amt { get; set; }
        // Business volume
        public decimal? pbv { get; set; }
        public decimal? gbv { get; set; }

        // Income fields
        public decimal? total_inc { get; set; }
        public decimal? tds { get; set; }
        public decimal? hc { get; set; }
        public decimal? dispache_inc { get; set; }

        // Income breakdown (inc_1 ... inc_15)
        public decimal? inc_1 { get; set; }
        public decimal? inc_2 { get; set; }
        public decimal? inc_3 { get; set; }
        public decimal? inc_4 { get; set; }
        public decimal? inc_5 { get; set; }
        public decimal? inc_6 { get; set; }
        public decimal? inc_7 { get; set; }
        public decimal? inc_8 { get; set; }
        public decimal? inc_9 { get; set; }
        public decimal? inc_10 { get; set; }
        public decimal? inc_11 { get; set; }
        public decimal? inc_12 { get; set; }
        public decimal? inc_13 { get; set; }
        public decimal? inc_14 { get; set; }
        public decimal? inc_15 { get; set; }


        public bool? is_paid { get; set; }
        public string? remarks { get; set; }
        public bool? pan_verify { get; set; }
        public bool? bank_verify { get; set; }
        public decimal? pan_tax { get; set; }
        public bool? status { get; set; }
    }



    public class MatrixPayoutDispatchListFilter
    {
        public int? payout_no { get; set; }
        public int? uid { get; set; }
        public bool? status { get; set; }
    }


    public class UserIncomeViewModel
    {
        public string? user_id { get; set; }
        public string? name { get; set; }
        public string? buyer_user_id { get; set; }
        public string? buyer_name { get; set; }
        public decimal? amount { get; set; }
        public string? invoice_no { get; set; }
        public string? doe { get; set; }
        public string? remark { get; set; }

    }

    public class UserIncomeListFilter
    {
        public int? uid { get; set; }
        public string? from_date { get; set; }
        public string? to_date { get; set; }
        public string? invoice_no { get; set; }
    }


    public class UserCompanyWalletRequestListFilter
    {
        public string? from_date { get; set; }
        public string? to_date { get; set; }
        public int? uid { get; set; }
        public string? user_id { get; set; }
        public int? status { get; set; }
    }

    public class UserCompanyWalletRequestViewModel
    {
        public int? wallet_req_id { get; set; }
        public string? comp_ac_no { get; set; }
        public string? comp_bank { get; set; }
        public string? user_ac_no { get; set; }
        public string? user_bank { get; set; }
        public string? payment_type { get; set; }
        public string? bank_tran_no { get; set; }
        public string? slip_img { get; set; }
        public string? remark { get; set; }
        public string? response { get; set; }
        public decimal? amount { get; set; }
        public string? status { get; set; }
        public string? doe { get; set; }
        public string? login_user_id { get; set; }
        public string? approved_date { get; set; }
        public string? user_id { get; set; }
        public string? name { get; set; }

    }


    public class UserCompanyWalletRequestDto
    {
        public string? mst_key { get; set; }
        public int? uid { get; set; }
        public decimal? amount { get; set; }
        public int? wallet_req_id { get; set; }
        public string? slip_img { get; set; }
        public IFormFile upload_slip_img { get; set; }
        public int status { get; set; }

        public string? comp_ac_no { get; set; }
        public string? comp_bank { get; set; }
        public string? user_ac_no { get; set; }
        public string? user_bank { get; set; }
        public string? bank_tran_no { get; set; }
        public string? payment_type { get; set; }
        public string? remark { get; set; }
        public string? response { get; set; }
        public string? login_user_id { get; set; }
        public string? tran_password { get; set; }
    }


    public class DB_UserCompanyWalletRequest
    {
        public string? mst_key { get; set; }
        public int? wallet_req_id { get; set; }
        public int? uid { get; set; }
        public decimal? amount { get; set; }
        public string? slip_img { get; set; }
        public int status { get; set; }
        public string? comp_ac_no { get; set; }
        public string? comp_bank { get; set; }
        public string? user_ac_no { get; set; }
        public string? user_bank { get; set; }
        public string? bank_tran_no { get; set; }
        public string? payment_type { get; set; }
        public string? remark { get; set; }
        public string? response { get; set; }
        public string? login_user_id { get; set; }
        public string? tran_password { get; set; }
    }


    public class UserWalletPassbookFilter
    {
        public string? from_date { get; set; }
        public string? to_date { get; set; }
        public int? uid { get; set; }
        public string? user_id { get; set; }
        public string? d_c { get; set; }
        public string? paid_user_id { get; set; }
    }


    public class UserCompanyWalletPassbookViewModel
    {
        public int? tran_id { get; set; }
        public string? user_id { get; set; }
        public string? name { get; set; }
        public string? paid_user_id { get; set; }
        public string? paid_name { get; set; }
        public decimal? amount { get; set; }
        public decimal? balance { get; set; }
        public string? d_c { get; set; }
        public string? remarks { get; set; }
        public string? tran_no { get; set; }
        public string? mode_item { get; set; }
        public string? doe { get; set; }

    }


    public class UserPayoutWalletPassbookFilter
    {
        public string? from_date { get; set; }
        public string? to_date { get; set; }
        public int? uid { get; set; }
        public string? user_id { get; set; }
        public string? d_c { get; set; }
        public string? paid_user_id { get; set; }
    }


    public class UserPayoutWalletPassbookViewModel
    {
        public int? tran_id { get; set; }
        public string? user_id { get; set; }
        public string? name { get; set; }
        public string? paid_user_id { get; set; }
        public string? paid_name { get; set; }
        public decimal? amount { get; set; }
        public decimal? balance { get; set; }
        public string? d_c { get; set; }
        public string? remarks { get; set; }
        public string? tran_no { get; set; }
        public string? mode_item { get; set; }
        public string? doe { get; set; }

    }

    public class EnquiryDto
    {
        public int? enq_id { get; set; }
        public string? name { get; set; }
        public string? address { get; set; }
        public string? state { get; set; }
        public string? district { get; set; }
        public string? city { get; set; }
        public string? mobile { get; set; }
        public string? email_id { get; set; }
        public string? msg { get; set; }
        public string? remarks { get; set; }
        public string? visit_date { get; set; }
        public string? subject { get; set; }
        public string? enquiry_from { get; set; }
        public string? response_user_id { get; set; }
        public int status { get; set; }
    }


    public class PaymentResult
    {
        public string? utr_amount { get; set; }
        public string? utr_no { get; set; }
        public string? utr_doe { get; set; }
        public string? utr_fullText { get; set; }
        public string? utr_senderName { get; set; }
        public string? utr_upiId { get; set; }
    }



    public class ErrorViewModel
    {
        public string? RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
