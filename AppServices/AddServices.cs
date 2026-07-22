using AppModels;
namespace AppServices
{
    public interface IAddServices
    {
        public Task<db_common_res> AddSize(SizeDto obj);
        public Task<db_common_res> AddBatch(BatchDto obj);
        public Task<db_common_res> AddCategory(CategoryDto obj);
        public Task<db_common_res> AddProduct(ProductView obj);
        public Task<db_common_res> AddUser(AddUpdateUserRequest obj);

        public Task<db_common_res> UpdateUserKyc(UpdateUserKycDto obj);
        public Task<db_common_res> AddVendor(VendorDto obj);
        public Task<db_common_res> GenarateVendorPO(AdminPurchaseDto obj);


        public Task<db_common_res> AddCartData(add_cart_data obj);
        public Task<db_common_res> GetCartData(filter_cart_data obj);
        public Task<db_common_res> GetCountCartData(filter_cart_data obj);
        public Task<db_common_res> DeletCartData(delete_cart_data obj);
        public Task<db_common_res> GetCartSchemeData(filter_cart_data obj);
         
        public Task<db_common_res> UpdateInvoiceDetailId(update_invoice_detail_id obj);

        public Task<db_common_res> ReceivePO_FromVendor(AdminPurchaseReceiveDto obj);


        public Task<db_common_res> ReceivedPayment(ReceivePaymentDto obj);

        public Task<db_common_res> FranCancelInvoice(CancelInvoiceFilter obj);

        public Task<db_common_res> UserCancelInvoice(CancelInvoiceFilter obj);

        public Task<db_common_res> DispatchedProduct(UpdateInvoiceDto req);
        public Task<db_common_res> ChangeSponsor(ChangeSponsor obj);
        public Task<db_common_res> ChangePassword(ChangePassword obj);

        public Task<db_common_res> ProfitLossDelete(ExpensesFilter obj);

        public Task<db_common_res> GenarateFranchiseOrderInvoice(GenerateInvoiceDto obj);
        public Task<db_common_res> GenarateUserInvoice(GenerateInvoiceDto obj);

        public Task<db_order_res> GenerateUserOrder(GenerateOrderDto obj);

        public Task<db_common_res> GenerateMatrixPayout(MakePayoutData obj);

        public Task<db_common_res> AddUpdateUserCompanyWalletRequest(UserCompanyWalletRequestDto obj);

        public Task<db_common_res> SaveEnquiry(EnquiryDto obj);

        public Task<db_common_res> ProcessSmsPayment(SmsPaymentReceive obj);

        public Task<db_common_res> AddShippingAddress(ShippingAddressDto obj);

        public Task<db_common_res> GetAddressList(AddressFilter obj);

        public Task<db_common_res> AddIngredient(IngredientDto obj);

    }
    public class AddServices : IAddServices
    {
        #region Constructure
        private readonly IFileService _fileService;
        private readonly IDapperContext _db;
        public AddServices(IDapperContext dapperContext, IFileService fileService)
        {
            _db = dapperContext;
            _fileService = fileService;
        }
        #endregion


        #region AddSize
        public async Task<db_common_res> AddSize(SizeDto obj)
        {
            try
            {
                return await _db.ExecuteQueryFirstOrDefaultAsync<db_common_res>("sp_add_update_size", obj);
            }
            catch (Exception ex) { return new db_common_res { error = "1", json_result = ex.Message }; }
        }
        #endregion


        #region AddBatch
        public async Task<db_common_res> AddBatch(BatchDto obj)
        {
            try
            {
                return await _db.ExecuteQueryFirstOrDefaultAsync<db_common_res>("sp_add_update_batch", obj);
            }
            catch (Exception ex) { return new db_common_res { error = "1", json_result = ex.Message }; }
        }
        #endregion


        #region AddCategory
        public async Task<db_common_res> AddCategory(CategoryDto obj)
        {
            try
            {
                return await _db.ExecuteQueryFirstOrDefaultAsync<db_common_res>("sp_add_update_category", obj);
            }
            catch (Exception ex) { return new db_common_res { error = "1", json_result = ex.Message }; }
        }
        #endregion


        #region AddProduct
        public async Task<db_common_res> AddProduct(ProductView req)
        {
            try
            {
                var filePath = Const.IMAGEPATH;
                //var filePath = Path.Combine("C:", "LIVEAPP", "IMAGES", "TEST");

                req.img_name_1 = await _fileService.SaveFileAsync(req.img1, filePath, "prod1_", saveToDisk: false);
                req.img_name_2 = await _fileService.SaveFileAsync(req.img2, filePath, "prod2_", saveToDisk: false);
                req.img_name_3 = await _fileService.SaveFileAsync(req.img3, filePath, "prod3_", saveToDisk: false);
                req.img_name_4 = await _fileService.SaveFileAsync(req.img4, filePath, "prod4_", saveToDisk: false);
                req.img_name_5 = await _fileService.SaveFileAsync(req.img5, filePath, "prod5_", saveToDisk: false);
                req.img_name_6 = await _fileService.SaveFileAsync(req.img6, filePath, "prod6_", saveToDisk: false);

                var obj = new Product_db()
                {
                    pid = req.pid,
                    prod_code = req.prod_code,
                    prod_name = req.prod_name,
                    prod_desc = req.prod_desc,
                    cat_id = req.cat_id,
                    sub_cat_id = req.sub_cat_id,
                    hsncode = req.hsncode,
                    t1 = req.t1,
                    d1 = req.d1,
                    t2 = req.t2,
                    d2 = req.d2,
                    t3 = req.t3,
                    d3 = req.d3,
                    t4 = req.t4,
                    d4 = req.d4,
                    t5 = req.t5,
                    d5 = req.d5,
                    is_pkd = req.is_pkd,
                    is_shop = req.is_shop,
                    is_arv = req.is_arv,
                    is_po = req.is_po,
                    status = req.status,
                    batch_id = req.batch_id,
                    barcode = req.barcode,
                    lenght = req.lenght,
                    width = req.width,
                    height = req.height,
                    lenght_unit = req.lenght_unit,
                    weight = req.weight,
                    case_size = req.case_size,

                    pur_price = req.pur_price,
                    mrp = req.mrp,
                    asso_price = req.asso_price,
                    cust_price = req.cust_price,
                    fran_price = req.fran_price,
                    b_bv = req.b_bv,
                    m_bv = req.m_bv,
                    f_bv = req.f_bv,
                    gst = req.gst,
                    img_name_1 = req.img_name_1,
                    img_name_2 = req.img_name_2,
                    img_name_3 = req.img_name_3,
                    img_name_4 = req.img_name_4,
                    img_name_5 = req.img_name_5,
                    img_name_6 = req.img_name_6,

                    is_trending = req.is_trending,
                    is_best_selling = req.is_best_selling,
                    seo_title = req.seo_title,
                    seo_description = req.seo_description,
                    ingredient_title = req.ingredient_title 
                };
                var db_result = await _db.ExecuteQueryFirstOrDefaultAsync<db_common_res>("sp_add_update_product", obj);
                if (db_result.error == "0")
                {
                    // Now save images to disk
                    await _fileService.SaveFileAsync(req.img1, filePath, req.img_name_1, true);
                    await _fileService.SaveFileAsync(req.img2, filePath, req.img_name_2, true);
                    await _fileService.SaveFileAsync(req.img3, filePath, req.img_name_3, true);
                    await _fileService.SaveFileAsync(req.img4, filePath, req.img_name_4, true);
                    await _fileService.SaveFileAsync(req.img5, filePath, req.img_name_5, true);
                    await _fileService.SaveFileAsync(req.img6, filePath, req.img_name_6, true);
                }
                return db_result;

            }
            catch (Exception ex) { return new db_common_res { error = "1", json_result = ex.Message }; }
        }
        #endregion


        #region AddUser
        public async Task<db_common_res> AddUser(AddUpdateUserRequest req)
        {
            try
            {
                var filePath = Const.IMAGEPATH;
                //var filePath = Path.Combine("C:", "LIVEAPP", "IMAGES", "TEST");

                req.pan_img = await _fileService.SaveFileAsync(req.upload_pan, filePath, "pan_", saveToDisk: false);
                req.bank_img = await _fileService.SaveFileAsync(req.upload_bank, filePath, "bank_", saveToDisk: false);
                req.aadhar_img_1 = await _fileService.SaveFileAsync(req.upload_aadhar_1, filePath, "aadhar_f_", saveToDisk: false);
                req.aadhar_img_2 = await _fileService.SaveFileAsync(req.upload_aadhar_2, filePath, "aadhar_b_", saveToDisk: false);
                req.gst_img = await _fileService.SaveFileAsync(req.upload_gst_img, filePath, "gst_", saveToDisk: false);
                req.pf_img = await _fileService.SaveFileAsync(req.upload_pf_img, filePath, "pf_", saveToDisk: false);


                var db_obj = new DB_UserRequest()
                {
                    uid = req.uid,
                    user_id = req.user_id,
                    login_uid = req.login_uid,
                    role_id = req.role_id,
                    sub_role_id = req.sub_role_id,
                    sub_role_name = req.sub_role_name,
                    submit_type = req.submit_type,
                    sponsor_user_id = req.sponsor_user_id,
                    p_side = req.p_side,
                    name = req.name,
                    com_name = req.com_name,
                    password = req.password,

                    tran_password = req.tran_password,
                    dob = req.dob,
                    address = req.address,
                    state_id = req.state_id,
                    dist_id = req.dist_id,
                    city = req.city,
                    pin_code = req.pin_code,
                    mobile = req.mobile,
                    email_id = req.email_id,
                    gender = req.gender,

                    pan_no = req.pan_no,
                    gst_no = req.gst_no,
                    aadhar_no = req.aadhar_no,
                    bank_id = req.bank_id,
                    branch = req.branch,
                    account_no = req.account_no,
                    ac_type = req.ac_type,
                    ifsc = req.ifsc,
                    nom_name = req.nom_name,
                    nom_rela = req.nom_rela,

                    aadhar_img_1 = req.aadhar_img_1,
                    aadhar_img_2 = req.aadhar_img_2,
                    pan_img = req.pan_img,
                    bank_img = req.bank_img,
                    gst_img = req.gst_img,
                    pf_img = req.pf_img
                };

                var db_result = await _db.ExecuteQueryFirstOrDefaultAsync<db_common_res>("sp_add_network", db_obj);
                if (db_result.error == "0")
                {
                    // Now save images to disk
                    await _fileService.SaveFileAsync(req.upload_pan, filePath, req.pan_img, true);
                    await _fileService.SaveFileAsync(req.upload_bank, filePath, req.bank_img, true);
                    await _fileService.SaveFileAsync(req.upload_aadhar_1, filePath, req.aadhar_img_1, true);
                    await _fileService.SaveFileAsync(req.upload_aadhar_2, filePath, req.aadhar_img_2, true);
                    await _fileService.SaveFileAsync(req.upload_gst_img, filePath, req.gst_img, true);
                    await _fileService.SaveFileAsync(req.upload_pf_img, filePath, req.pf_img, true);
                }
                return db_result;
            }
            catch (Exception ex) { return new db_common_res { error ="1" , json_result = ex.Message }; }
             
        }
        #endregion


        #region UpdateUserKyc
        public async Task<db_common_res> UpdateUserKyc(UpdateUserKycDto req)
        {
            try
            {
                return await _db.ExecuteQueryFirstOrDefaultAsync<db_common_res>("usp_update_user_kyc", req);
            }
            catch (Exception ex)
            {
                return new db_common_res { error = "1", json_result = "Error: " + ex.Message };
            }
        }
        #endregion


        #region AddVendor
        public async Task<db_common_res> AddVendor(VendorDto obj)
        {
            try
            {
                var db_obj = new Vendor_db()
                {
                    vendor_id = obj.vendor_id,
                    vendor_name = obj.vendor_name,
                    display_name = obj.display_name,
                    email_id = obj.email_id,
                    phone_no = obj.phone_no,
                    mobile_id = obj.mobile_id,
                    state_id = obj.state_id,
                    skype_name = obj.skype_name,
                    designation = obj.designation,
                    department = obj.department,
                    website = obj.website,
                    status = obj.status
                };
                return await _db.ExecuteQueryFirstOrDefaultAsync<db_common_res>("sp_add_update_vendor", db_obj);
            }
            catch (Exception ex) { return new db_common_res { error = "1", json_result = ex.Message }; }
        }
        #endregion


        #region GenarateVendorPO
        public async Task<db_common_res> GenarateVendorPO(AdminPurchaseDto obj)
        {
            try
            {
                var db_obj = new VendorPurchase_DB()
                {
                    cart_id = obj.cart_id,
                    vendor_id = obj.vendor_id,
                    delivery_date = obj.delivery_date,
                    doe = obj.doe,
                    invoice_no = obj.invoice_no,
                    buyer_uid = obj.buyer_uid
                };
                return await _db.ExecuteQueryFirstOrDefaultAsync<db_common_res>("sp_generate_admin_purchase", db_obj);
            }
            catch (Exception ex) { return new db_common_res { error = "1", json_result = ex.Message }; }
        }
        #endregion


        #region AddCartData
        public async Task<db_common_res> AddCartData(add_cart_data obj)
        {
            try
            {
                return await _db.ExecuteQueryFirstOrDefaultAsync<db_common_res>("sp_add_cart_data", obj);
            }
            catch (Exception ex) { return new db_common_res { error = "1", json_result = ex.Message }; }
        }
        #endregion


        #region GetCartData
        public async Task<db_common_res> GetCartData(filter_cart_data obj)
        {
            try
            {
                return await _db.ExecuteQueryFirstOrDefaultAsync<db_common_res>("sp_get_cart_data", obj);
            }
            catch (Exception ex) { return new db_common_res { error = "1", json_result = ex.Message }; }
        }
        #endregion


        #region GetCountCartData
        public async Task<db_common_res> GetCountCartData(filter_cart_data obj)
        {
            try
            {
                return await _db.ExecuteQueryFirstOrDefaultAsync<db_common_res>("sp_get_count_cart_data", obj);
            }
            catch (Exception ex) { return new db_common_res { error = "1", json_result = ex.Message }; }
        }
        #endregion


        #region GetCartSchemeData
        public async Task<db_common_res> GetCartSchemeData(filter_cart_data obj)
        {
            try
            {
                return await _db.ExecuteQueryFirstOrDefaultAsync<db_common_res>("sp_get_cart_scheme_data", obj);
            }
            catch (Exception ex) { return new db_common_res { error = "1", json_result = ex.Message }; }
        }
        #endregion


        #region DeletCartData
        public async Task<db_common_res> DeletCartData(delete_cart_data obj)
        {
            try
            {
                return await _db.ExecuteQueryFirstOrDefaultAsync<db_common_res>("sp_delete_cart_data", obj);
            }
            catch (Exception ex) { return new db_common_res { error = "1", json_result = ex.Message }; }
        }
        #endregion



        #region UpdateInvoiceDetailId
        public async Task<db_common_res> UpdateInvoiceDetailId(update_invoice_detail_id obj)
        {
            try
            {
                return await _db.ExecuteQueryFirstOrDefaultAsync<db_common_res>("sp_update_invoice_detail_id", obj);
            }
            catch (Exception ex) { return new db_common_res { error = "1", json_result = ex.Message }; }
        }
        #endregion



        #region ReceivePO_FromVendor
        public async Task<db_common_res> ReceivePO_FromVendor(AdminPurchaseReceiveDto obj)
        {
            try
            {
                return await _db.ExecuteQueryFirstOrDefaultAsync<db_common_res>("sp_generate_admin_purchase_receive", obj);
            }
            catch (Exception ex) { return new db_common_res { error = "1", json_result = ex.Message }; }
        }
        #endregion



        #region GenarateFranchiseOrderInvoice 
        public async Task<db_common_res> GenarateFranchiseOrderInvoice(GenerateInvoiceDto req)
        {
            try
            {
                // Order
                if (req.cart_id == 11) // Order
                {
                    var db_obj = new GenerateOrderDB()
                    {
                        cart_id = req.cart_id,
                        inv_type = req.inv_type,
                        buyer_uid = req.buyer_uid,
                        user_id = req.user_id,
                        seller_uid = req.seller_uid,

                        gst_no = req.gst_no,
                        s_name = req.s_name,

                        s_mobile_no = req.s_mobile_no,
                        state_id = req.state_id,
                        dist_id = req.dist_id,
                        s_city = req.s_city,
                        s_address = req.s_address,

                        s_pincode = req.s_pincode,
                        pay_id = req.pay_id,
                        bank_name = req.bank_name,
                        check_no = req.check_no,
                        checkdate = req.checkdate,

                        scheme = req.scheme,
                        discount = req.discount,
                        del_charge = req.del_charge,
                        net_delivery_charge = req.net_delivery_charge,
                        adjust_amt = req.adjust_amt,

                        coupon = req.coupon,
                        img_name = req.img_name,
                        panel = req.panel,
                        appr_by = req.appr_by,
                    };
                    return await _db.ExecuteQueryFirstOrDefaultAsync<db_common_res>("usp_generate_admin_order", db_obj);
                }
                // Invoice
                else if (req.cart_id == 10 || req.cart_id == 9) // Invoice
                {
                    var db_obj = new GenerateInvoiceDB()
                    {
                        is_old_inv = req.is_old_inv,
                        invoice_no = req.invoice_no,
                        doe = req.doe,
                        cart_id = req.cart_id,
                        inv_type = req.inv_type,
                        buyer_uid = req.buyer_uid,
                        user_id = req.user_id,
                        seller_uid = req.seller_uid,

                        gst_no = req.gst_no,
                        s_name = req.s_name,

                        s_mobile_no = req.s_mobile_no,
                        state_id = req.state_id,
                        dist_id = req.dist_id,
                        s_city = req.s_city,
                        s_address = req.s_address,

                        s_pincode = req.s_pincode,
                        pay_id = req.pay_id,
                        bank_name = req.bank_name,
                        check_no = req.check_no,
                        checkdate = req.checkdate,

                        scheme = req.scheme,
                        discount = req.discount,
                        del_charge = req.del_charge,
                        net_delivery_charge = req.net_delivery_charge,
                        adjust_amt = req.adjust_amt,

                        coupon = req.coupon,
                        img_name = req.img_name,
                        panel = req.panel,
                        appr_by = req.appr_by,
                        order_id = req.order_id,
                        receive_amount = req.receive_amount,
                        remark = req.remark
                    };
                    return await _db.ExecuteQueryFirstOrDefaultAsync<db_common_res>("usp_generate_admin_invoice", db_obj);
                }
                return null;
            }
            catch (Exception ex) { return new db_common_res { error = "1", json_result = ex.Message }; }
        }
        #endregion


        #region GenarateUserInvoice
        public async Task<db_common_res> GenarateUserInvoice(GenerateInvoiceDto req)
        {
            try
            {
                // 6 = Repurchase, 7 = AWS Product, 8 = Free Sample
                if (req.cart_id == 6 || req.cart_id == 7 || req.cart_id == 8)
                {
                    var db_obj = new GenerateInvoiceDB()
                    {
                        is_old_inv = req.is_old_inv,
                        invoice_no = req.invoice_no,
                        doe = req.doe,
                        cart_id = req.cart_id,
                        inv_type = req.inv_type,
                        buyer_uid = req.buyer_uid,
                        user_id = req.user_id,
                        seller_uid = req.seller_uid,

                        gst_no = req.gst_no,
                        s_name = req.s_name,

                        s_mobile_no = req.s_mobile_no,
                        state_id = req.state_id,
                        dist_id = req.dist_id,
                        s_city = req.s_city,
                        s_address = req.s_address,

                        s_pincode = req.s_pincode,
                        pay_id = req.pay_id,
                        bank_name = req.bank_name,
                        check_no = req.check_no,
                        checkdate = req.checkdate,

                        scheme = req.scheme,
                        discount = req.discount,
                        del_charge = req.del_charge,
                        net_delivery_charge = req.net_delivery_charge,
                        adjust_amt = req.adjust_amt,

                        coupon = req.coupon,
                        img_name = req.img_name,
                        panel = req.panel,
                        appr_by = req.appr_by,
                        order_id = req.order_id,
                        receive_amount = req.receive_amount,
                        remark = req.remark
                    };
                    return await _db.ExecuteQueryFirstOrDefaultAsync<db_common_res>("usp_generate_user_invoice", db_obj);
                }

                return null;
            }
            catch (Exception ex) { return new db_common_res { error = "1", json_result = ex.Message }; }
        }
        #endregion


        #region Received Payment
        public async Task<db_common_res> ReceivedPayment(ReceivePaymentDto obj)
        {
            try
            {
                return await _db.ExecuteQueryFirstOrDefaultAsync<db_common_res>("sp_receive_payment_party_wise", obj);
            }
            catch (Exception ex) { return new db_common_res { error = "1", json_result = ex.Message }; }
        }
        #endregion


        #region DispatchedProduct
        public async Task<db_common_res> DispatchedProduct(UpdateInvoiceDto req)
        {
            try
            {
                var filePath = Const.IMAGEPATH;
                //var filePath = Path.Combine("C:", "LIVEAPP", "IMAGES", "TEST");

                req.lr_1 = await _fileService.SaveFileAsync(req.upload_lr_1, filePath, "lr_1_", saveToDisk: false);
                req.lr_2 = await _fileService.SaveFileAsync(req.upload_lr_2, filePath, "lr_2_", saveToDisk: false);

                var db_obj = new DispatchedProductDB()
                {
                    inv_id = req.inv_id,
                    delivery_status = req.delivery_status,
                    transport = req.transport,
                    tracking = req.tracking,
                    del_by = req.del_by,
                    lr_1 = req.lr_1,
                    lr_2 = req.lr_2
                };

                var db_result = await _db.ExecuteQueryFirstOrDefaultAsync<db_common_res>("sp_update_dispatch_inv", db_obj);
                if (db_result.error == "0")
                {
                    // Now save images to disk
                    await _fileService.SaveFileAsync(req.upload_lr_1, filePath, req.lr_1, true);
                    await _fileService.SaveFileAsync(req.upload_lr_2, filePath, req.lr_2, true);
                }
                return db_result;
            }
            catch (Exception ex) { return new db_common_res { error = "1", json_result = ex.Message }; }
        }
        #endregion


        #region ChangeSponsor
        public async Task<db_common_res> ChangeSponsor(ChangeSponsor obj)
        {
            try
            {
                return await _db.ExecuteQueryFirstOrDefaultAsync<db_common_res>("sp_change_sponsor", obj);
            }
            catch (Exception ex) { return new db_common_res { error = "1", json_result = ex.Message }; }
        }
        #endregion


        #region ChangePassword
        public async Task<db_common_res> ChangePassword(ChangePassword obj)
        {
            try
            {
                return await _db.ExecuteQueryFirstOrDefaultAsync<db_common_res>("sp_change_password", obj);
            }
            catch (Exception ex) { return new db_common_res { error = "1", json_result = ex.Message }; }
        }
        #endregion


        #region FranCancelInvoice
        public async Task<db_common_res> FranCancelInvoice(CancelInvoiceFilter obj)
        {
            try
            {
                return await _db.ExecuteQueryFirstOrDefaultAsync<db_common_res>("sp_admin_invoice_cancel", obj);
            }
            catch (Exception ex) { return new db_common_res { error = "1", json_result = ex.Message }; }
        }
        #endregion


        #region UserCancelInvoice
        public async Task<db_common_res> UserCancelInvoice(CancelInvoiceFilter obj)
        {
            try
            {
                return await _db.ExecuteQueryFirstOrDefaultAsync<db_common_res>("sp_cancel_user_invoice", obj);
            }
            catch (Exception ex) { return new db_common_res { error = "1", json_result = ex.Message }; }
        }
        #endregion


        #region ProfitLossDelete
        public async Task<db_common_res> ProfitLossDelete(ExpensesFilter obj)
        {
            try
            {
                return await _db.ExecuteQueryFirstOrDefaultAsync<db_common_res>("sp_expenses_delete", obj);
            }
            catch (Exception ex) { return new db_common_res { error = "1", json_result = ex.Message }; }
        }
        #endregion


        #region GenerateUserOrder
        public async Task<db_order_res> GenerateUserOrder(GenerateOrderDto req)
        {
            try
            {
                var dbDto = new GenerateUserOrderDto
                {
                    cart_id = req.cart_id,
                    inv_type = req.inv_type,
                    buyer_uid = req.buyer_uid,
                    user_id = req.user_id,
                    seller_uid = req.seller_uid,
                    gst_no = req.gst_no,
                    s_name = req.s_name,

                    s_mobile_no = req.s_mobile_no,
                    state_id = req.state_id,
                    dist_id = req.dist_id,
                    s_city = req.s_city,
                    s_address = req.s_address,

                    s_pincode = req.s_pincode,
                    pay_id = req.pay_id,
                    bank_name = req.bank_name,
                    check_no = req.check_no,
                    checkdate = req.checkdate,

                    scheme = req.scheme,
                    discount = req.discount,
                    del_charge = req.del_charge,
                    net_delivery_charge = req.net_delivery_charge,
                    adjust_amt = req.adjust_amt,

                    coupon = req.coupon,
                    img_name = req.img_name,
                    panel = req.panel,
                    appr_by = req.appr_by,

                    img_utr_amount = req.img_utr_amount,
                    img_utr_no = req.img_utr_no,
                    img_utr_date = req.img_utr_date,
                    img_utr_full_text = req.img_utr_full_text,
                    img_utr_sender_name = req.img_utr_sender_name,
                    img_utr_upi_id = req.img_utr_upi_id, 
                    said = req.said
                };
                return await _db.ExecuteQueryFirstOrDefaultAsync<db_order_res>("usp_generate_user_order", dbDto);
            }
            catch (Exception ex)
            {
                return new db_order_res { error = "1", json_result = ex.Message };
            }
        }
        #endregion


        #region GenerateMatrixPayout
        public async Task<db_common_res> GenerateMatrixPayout(MakePayoutData req)
        {
            try
            {
                return await _db.ExecuteQueryFirstOrDefaultAsync<db_common_res>("usp_generate_matrix_payout", req);
            }
            catch (Exception ex) { return new db_common_res { error = "1", json_result = ex.Message }; }
        }
        #endregion


        #region AddUpdateUserCompanyWalletRequest
        public async Task<db_common_res> AddUpdateUserCompanyWalletRequest(UserCompanyWalletRequestDto req)
        {
            try
            {
                var filePath = Const.IMAGEPATH;
                req.slip_img = await _fileService.SaveFileAsync(req.upload_slip_img, filePath, "p_slip_", saveToDisk: false);
                var db_obj = new DB_UserCompanyWalletRequest()
                {
                    mst_key = req.mst_key,
                    wallet_req_id = req.wallet_req_id,
                    uid = req.uid,
                    amount = req.amount,
                    slip_img = req.slip_img,
                    comp_ac_no = req.comp_ac_no,
                    comp_bank = req.comp_bank,
                    user_ac_no = req.user_ac_no,
                    user_bank = req.user_bank,
                    bank_tran_no = req.bank_tran_no,
                    payment_type = req.payment_type,
                    remark = req.remark,
                    response = req.response,
                    login_user_id = req.login_user_id,
                    tran_password = req.tran_password,
                    status = req.status
                };
                var db_result = await _db.ExecuteQueryFirstOrDefaultAsync<db_common_res>("usp_user_wallet_request", db_obj);
                if (db_result.error == "0")
                {
                    // Now save images to disk
                    await _fileService.SaveFileAsync(req.upload_slip_img, filePath, req.slip_img, true);
                }
                return db_result;
            }
            catch (Exception ex)
            {
                return new db_common_res { error = "1", json_result = "Error: " + ex.Message };
            }
        }
        #endregion


        #region SaveEnquiry
        public async Task<db_common_res> SaveEnquiry(EnquiryDto obj)
        {
            try
            {
                return await _db.ExecuteQueryFirstOrDefaultAsync<db_common_res>("usp_enquiry_save", obj);
            }
            catch (Exception ex)
            {
                return new db_common_res { error = "1", json_result = "Error: " + ex.Message };
            }
        }
        #endregion


        #region ProcessSmsPayment
        public async Task<db_common_res> ProcessSmsPayment(SmsPaymentReceive obj)
        {
            try
            {
                return await _db.ExecuteQueryFirstOrDefaultAsync<db_common_res>("usp_save_online_payment", obj);
            }
            catch (Exception ex)
            {
                return new db_common_res { error = "1", json_result = "Error: " + ex.Message };
            }
        }
        #endregion


        #region AddShippingAddress
        public async Task<db_common_res> AddShippingAddress(ShippingAddressDto obj)
        {
            try
            {
                return await _db.ExecuteQueryFirstOrDefaultAsync<db_common_res>("usp_save_shipping_address", obj);
            }
            catch (Exception ex) { return new db_common_res { error = "1", json_result = ex.Message }; }
        }
        #endregion


        #region GetAddressList
        public async Task<db_common_res> GetAddressList(AddressFilter obj)
        {
            try
            {
                return await _db.ExecuteQueryFirstOrDefaultAsync<db_common_res>("sp_get_address_list", obj);
            }
            catch (Exception ex) { return new db_common_res { error = "1", json_result = ex.Message }; }
        }
        #endregion



        #region AddUpdateProject
        public async Task<db_common_res> AddIngredient(IngredientDto req)
        {
            try
            {
                var filePath = Const.IMAGEPATH;
                req.ingredient_img = await _fileService.SaveFileAsync(req.upload_ingredient_img, filePath, "Ingredient_", saveToDisk: false);
                var db_obj = new DB_Ingredient()
                {
                    ingredient_id = req.ingredient_id,
                    pid = req.pid,
                    ingredient_name = req.ingredient_name,
                    ingredient_desc = req.ingredient_desc,
                    display_order = req.display_order,
                    ingredient_img = req.ingredient_img 
                }; 
                var db_result = await _db.ExecuteQueryFirstOrDefaultAsync<db_common_res>("usp_add_ingredient", db_obj);
                if (db_result.error == "0")
                {
                    // Now save images to disk
                    await _fileService.SaveFileAsync(req.upload_ingredient_img, filePath, req.ingredient_img, true);
                }
                return db_result;
            }
            catch (Exception ex)
            {
                return new db_common_res { error = "1", json_result = "Error: " + ex.Message };
            }
        }
        #endregion
    }
}
