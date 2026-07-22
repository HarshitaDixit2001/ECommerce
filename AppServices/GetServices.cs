using AppModels;
using Newtonsoft.Json;

namespace AppServices
{ 

    public interface IGetServices
    {
        public Task<db_common_res> master_reports(guest_report_request_param obj);
        public Task<db_common_res> master_update_status(update_table_row_status_master_param obj);
        public Task<db_common_res> master_delete_table_row(delete_table_row_master_param obj);

        public Task<object> master_dropdown(dropdown_request_param obj);
        public Task<List<UserReportDto>> UserList(UserListFilter obj);
        public Task<List<UserReportDto>> UserListBySubAdmin(UserListFilter obj);
        public Task<db_common_res> UserDetail(UserListBasicDetailFilter obj);
        public Task<db_common_res> UpdateOrderDetail(UpdateOrderDetailFilter obj);
        public Task<db_common_res> UserUpdateOrderDetail(UpdateOrderDetailFilter obj);
        public Task<db_common_res> GetVendorPODetail(inv_id_detail obj); 
        public Task<db_common_res> GetUserProductMapping(UserProductMappingFilter obj);
        public Task<List<FranOrderViewModel>> FranOrderList(OrderListFilter obj);
        public Task<List<UserOrderViewModel>> UserOrderList(UserOrderListFilter obj);
        public Task<List<FranInvoiceViewModel>> FranInvoiceList(InvoiceListFilter obj);
        public Task<List<UserInvoiceViewModel>> UserInvoiceList(UserInvoiceListFilter obj);
        public Task<List<FranWalletTransactionModel>> FranWalletTransactionList(WalletTransactionFilter obj);
        public Task<db_common_res> GetOrderProductsByOrderId(OrderIdRequestModel obj);

        public Task<db_common_res> GetUserOrderProductsByOrderId(OrderIdRequestModel obj);

        public Task<db_common_res> GetOrderBuyerSellerDetail(OrderIdRequestModel obj);

        public Task<db_common_res> GetUserOrderBuyerSellerDetail(OrderIdRequestModel obj);

        public Task<db_common_res> GetInvoiceProductsByOrderId(InvoiceIdRequestModel obj);
        public Task<db_common_res> GetPaymentHistory(buyer_uid_filter obj);
        public Task<db_common_res> GetInvoiceBuyerSellerDetail(InvoiceIdRequestModel obj);
        public Task<List<StockReportDto>> StockList(StockListFilter obj);
        public Task<db_common_res> GetStockTranByUIdPID(StockTranRequestModel obj);
        
        public Task<List<VendorPurchaseOrderDto>> VendorPOList(AdminVendorPurchaseFilter obj);
        public Task<db_common_res> SalesDashboard(SalesDashboardValueFilter obj);
        public Task<List<MonthWiseProductQtyModel>> GetMonthlyProductQty(MonthWiseProductQtyFilter obj);
        public Task<List<SalesCollectionModel>> GetSalesCollection(SalesCollectionFilter obj);
        public Task<List<SalesCollectionCommissionModel>> GetSalesCollectionCommission(SalesCollectionCommisssionFilter obj);
        public Task<db_common_res> GetVendorPODetailReport(VendorDetailRequestModel obj);
        public Task<db_common_res> GetVendorPOProductDetail(VendorDetailRequestModel obj);
        public Task<db_common_res> GetSalesCollectionGraph(SalesCollectionGraphFilter obj);
        public Task<db_common_res> GetCategoryWiseGraph(SalesCollectionGraphFilter obj);
        public Task<db_common_res> GetProductWiseGraph(SalesCollectionGraphFilter obj);
        public Task<db_common_res> GetUseTarget(UserTarggetFilter obj);
        public Task<List<PartyWiseOutstandingViewModel>> PartyWiseOutstanding(PartyWiseOutstandingFilter obj);
        public Task<db_common_res> GetExpensesCategory();
        public Task<List<ExpensesMappingDto>> GetExpenses(ExpensesMappingFilter obj);

        public Task<List<ProfitLossModel>> GetProfitLossReport(ProfitLossFilter obj);
 

        public Task<List<ProductViewModel>> GetRootProductList(get_root_product obj);

        public Task<ProductDetailViewModel> ProdDetail(get_root_product_detail obj);

        public Task<List<matrix_payout_date_ViewModel>> Get_Repurchase_Payout_Period();
 
        public Task<List<MatrixPayoutDispatchViewModel>> Repurchase_Payout_Dispatch(MatrixPayoutDispatchListFilter obj);

        public Task<List<UserIncomeViewModel>> UserIncomeList(UserIncomeListFilter obj);

        public Task<db_common_res> GetMatrixTreeView(matrix_tree_view_filter obj);

        public Task<db_common_res> UserDashboard(UserTarggetFilter obj);


        public Task<List<UserCompanyWalletRequestViewModel>> GetUserCompanyWalletRequest(UserCompanyWalletRequestListFilter obj);
        public Task<List<UserCompanyWalletPassbookViewModel>> UserCompanyWalletPassbook(UserWalletPassbookFilter obj);
        public Task<List<UserPayoutWalletPassbookViewModel>> UserPayoutWalletPassbook(UserPayoutWalletPassbookFilter obj);


        public Task<List<BinaryUserDto>> GetUserBinaryTree(BinaryTeamFilter obj);

        public Task<List<UserDownlineReport>> UserMatrixList(UserMatrixFilter obj);

        public Task<List<UserDownlineBinaryReport>> UserBinaryList(UserBinaryFilter obj);

        public Task<db_common_res> GetOrdInvSuccess(GetOrderInvFilter obj);

    }


    public class GetServices : IGetServices
    {
        #region Constuctors
        private readonly IDapperContext _db;
        public GetServices(IDapperContext dapperContext)
        {
            _db = dapperContext;
        }
        #endregion


        #region master_reports
        public async Task<db_common_res> master_reports(guest_report_request_param obj)
        {
            try
            {
                return await _db.ExecuteQueryFirstOrDefaultAsync<db_common_res>("sp_get_master_data", obj);
            } 
            catch (Exception ex)
            { 
                return new db_common_res
                { error = "1", json_result = "An unexpected error occurred" };
            }
        }
        #endregion


        #region master_dropdown
        public async Task<object> master_dropdown(dropdown_request_param obj)
        {
            try
            {
                var result = await _db.ExecuteQueryFirstOrDefaultAsync<db_common_res>("sp_get_master_dropdown", obj);
                List<dropdown_response> json = JsonConvert.DeserializeObject<List<dropdown_response>>(result.json_result);
                return json;
            }
            catch (Exception exe) { return null; }
        }
        #endregion


        #region master_update_status
        public async Task<db_common_res> master_update_status(update_table_row_status_master_param obj)
        {
            try
            {
                return await _db.ExecuteQueryFirstOrDefaultAsync<db_common_res>("usp_update_table_row_status_master", obj);
            }
            catch (Exception ex)
            {
                return new db_common_res { error = "1", json_result = "An unexpected error occurred" };
            }
        }
        #endregion


        #region master_delete_table_row
        public async Task<db_common_res> master_delete_table_row(delete_table_row_master_param obj)
        {
            try
            {
                return await _db.ExecuteQueryFirstOrDefaultAsync<db_common_res>("usp_delete_table_row_master", obj);
            }
            catch (Exception ex)
            {
                return new db_common_res { error = "1", json_result = "An unexpected error occurred" };
            }
        }
        #endregion
         

        #region UserList
        public async Task<List<UserReportDto>> UserList(UserListFilter obj)
        {
            try 
            {
                var result = await _db.ExecuteStoredProcedureAsync<UserReportDto>("sp_get_user_network", obj);
                return result.ToList();
            }
            catch (Exception exe)
            {
                return null;
            }
        }
        #endregion


        #region UserListBySubAdmin
        public async Task<List<UserReportDto>> UserListBySubAdmin(UserListFilter obj)
        {
            try
            {
                var result = await _db.ExecuteStoredProcedureAsync<UserReportDto>("sp_get_user_by_subadmin", obj);
                return result.ToList();
            }
            catch (Exception exe)
            {
                return null;
            }
        }
        #endregion


        #region UserDetail
        public async Task<db_common_res> UserDetail(UserListBasicDetailFilter obj)
        {
            try
            {
                return await _db.ExecuteQueryFirstOrDefaultAsync<db_common_res>("sp_get_edit_user_detail", obj);
            }
            catch (Exception exe)
            {
                return null;
            }
        }
        #endregion


        #region AdminOrderDetailUpdate
        public async Task<db_common_res> UpdateOrderDetail(UpdateOrderDetailFilter obj)
        {
            try
            {
                return await _db.ExecuteQueryFirstOrDefaultAsync<db_common_res>("sp_update_order_detail", obj);
            }
            catch (Exception exe)
            {
                return null;
            }
        }
        #endregion


        #region UserOrderDetailUpdate
        public async Task<db_common_res> UserUpdateOrderDetail(UpdateOrderDetailFilter obj)
        {
            try
            {
                return await _db.ExecuteQueryFirstOrDefaultAsync<db_common_res>("sp_user_update_order_detail", obj);
            }
            catch (Exception exe)
            {
                return null;
            }
        }
        #endregion


        #region GetVendorPODetail
        public async Task<db_common_res> GetVendorPODetail(inv_id_detail obj)
        {
            try
            {
                return await _db.ExecuteQueryFirstOrDefaultAsync<db_common_res>("sp_get_invoice_data", obj);
            }
            catch (Exception exe)
            {
                return null;
            }
        }
        #endregion

 
        #region Franchise_Order_List
        public async Task<List<FranOrderViewModel>> FranOrderList(OrderListFilter obj)
        {
            try
            {
                var result = await _db.ExecuteStoredProcedureAsync<FranOrderViewModel>("sp_get_fran_orders", obj);
                return result.ToList();
            }
            catch { return null; }
        }
        #endregion


        #region User_Order_List
        public async Task<List<UserOrderViewModel>> UserOrderList(UserOrderListFilter obj)
        {
            try
            {
                var result = await _db.ExecuteStoredProcedureAsync<UserOrderViewModel>("sp_get_user_orders", obj);
                return result.ToList();
            }
            catch { return null; }
        }
        #endregion


        #region FranInvoice_List
        public async Task<List<FranInvoiceViewModel>> FranInvoiceList(InvoiceListFilter obj)
        {
            try
            {
                var result = await _db.ExecuteStoredProcedureAsync<FranInvoiceViewModel>("sp_get_fran_invoice", obj);
                return result.ToList();
            }
            catch {  return null; }
        }
        #endregion


        #region UserInvoice_List
        public async Task<List<UserInvoiceViewModel>> UserInvoiceList(UserInvoiceListFilter obj)
        {
            try
            {
                var result = await _db.ExecuteStoredProcedureAsync<UserInvoiceViewModel>("sp_get_user_invoice", obj);
                return result.ToList();
            }
            catch { return null; }
        }
        #endregion


        #region FranWalletTransactionList
        public async Task<List<FranWalletTransactionModel>> FranWalletTransactionList(WalletTransactionFilter obj)
        {
            try
            {
                var result = await _db.ExecuteStoredProcedureAsync<FranWalletTransactionModel>("sp_get_fran_wallet_transaction", obj);
                return result.ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion


        #region Admin Order Product Detail
        public async Task<db_common_res> GetOrderProductsByOrderId(OrderIdRequestModel obj)
        {
            try
            {
                return await _db.ExecuteQueryFirstOrDefaultAsync<db_common_res>("sp_get_admin_order_products", obj);
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion


        #region User Order Product Detail
        public async Task<db_common_res> GetUserOrderProductsByOrderId(OrderIdRequestModel obj)
        {
            try
            {
                return await _db.ExecuteQueryFirstOrDefaultAsync<db_common_res>("usp_get_user_order_products", obj);
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion


        #region GetOrderBuyerSellerDetail
        public async Task<db_common_res> GetOrderBuyerSellerDetail(OrderIdRequestModel obj)
        {
            try
            {
                return await _db.ExecuteQueryFirstOrDefaultAsync<db_common_res>("sp_get_admin_order_buyer", obj);
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        #region GetUserOrderBuyerSellerDetail
        public async Task<db_common_res> GetUserOrderBuyerSellerDetail(OrderIdRequestModel obj)
        {
            try
            {
                return await _db.ExecuteQueryFirstOrDefaultAsync<db_common_res>("usp_get_user_order_buyer", obj);
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion
        #region GetInvoiceProductsByOrderId
        public async Task<db_common_res> GetInvoiceProductsByOrderId(InvoiceIdRequestModel obj)
        {
            try
            {
                return await _db.ExecuteQueryFirstOrDefaultAsync<db_common_res>("sp_get_admin_invoice_products", obj);
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion


        #region GetPaymentHistory
        public async Task<db_common_res> GetPaymentHistory(buyer_uid_filter obj)
        {
            try
            {
                return await _db.ExecuteQueryFirstOrDefaultAsync<db_common_res>("sp_get_admin_payment_history", obj);
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion


        #region GetInvoiceBuyerSellerDetail
        public async Task<db_common_res> GetInvoiceBuyerSellerDetail(InvoiceIdRequestModel obj)
        {
            try
            {
                return await _db.ExecuteQueryFirstOrDefaultAsync<db_common_res>("sp_get_admin_invoice_buyer", obj);
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion


        #region StockList
        public async Task<List<StockReportDto>> StockList(StockListFilter obj)
        {
            try
            {
                var result = await _db.ExecuteStoredProcedureAsync<StockReportDto>("sp_get_stocks", obj);
                return result.ToList();
                //return await _db.ExecuteQueryFirstOrDefaultAsync<db_common_pageing_res>("sp_get_stocks", obj);
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion


        #region StockTran
        public async Task<db_common_res> GetStockTranByUIdPID(StockTranRequestModel obj)
        {
            try
            {
                return await _db.ExecuteQueryFirstOrDefaultAsync<db_common_res>("sp_get_stock_tran", obj);
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion


        #region Vendor_PO_List
        public async Task<List<VendorPurchaseOrderDto>> VendorPOList(AdminVendorPurchaseFilter obj)
        {
            try
            {
                var result = await _db.ExecuteStoredProcedureAsync<VendorPurchaseOrderDto>("sp_get_vendor_purchase_order", obj);
                return result.ToList();
               // return await _db.ExecuteQueryFirstOrDefaultAsync<db_common_pageing_res>("sp_get_vendor_purchase_order", obj);
            }
            catch (Exception exe) { return null; }
        }
        #endregion


        #region SalesDashboard
        public async Task<db_common_res> SalesDashboard(SalesDashboardValueFilter obj)
        {
            try
            {
                return await _db.ExecuteQueryFirstOrDefaultAsync<db_common_res>("sp_get_salles_dashboad_values", obj);
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion


        #region GetMonthlyProductQty
        public async Task<List<MonthWiseProductQtyModel>> GetMonthlyProductQty(MonthWiseProductQtyFilter obj)
        {
            try
            { 
                var list = new List<MonthWiseProductQtyModel>();
                var result = await _db.ExecuteStoredProcedure("sp_get_monthwise_product_qty", obj); // returns List<dynamic>

                foreach (var row in result)
                {
                    var dict = new Dictionary<string, decimal>();
                    string prodCode = row.prod_code;
                    string prodName = row.prod_name;

                    foreach (var prop in ((IDictionary<string, object>)row))
                    {
                        var key = prop.Key;
                        var val = prop.Value;

                        if (key != "prod_code" && key != "prod_name")
                        {
                            if (val == null)
                            {
                                dict[key] = 0; // ✅ store null explicitly
                            }
                            else if (decimal.TryParse(val.ToString(), out var qty))
                            {
                                dict[key] = qty;
                            }
                        }
                    }

                    list.Add(new MonthWiseProductQtyModel
                    {
                        prod_code = prodCode,
                        prod_name = prodName,
                        month_qty = dict
                    });
                }

                return list;
            }
            catch
            {
                return new List<MonthWiseProductQtyModel>();
            }
        }
        #endregion


        #region GetSalesCollection
        public async Task<List<SalesCollectionModel>> GetSalesCollection(SalesCollectionFilter obj)
        {
            try
            {
                var list = new List<SalesCollectionModel>();
                var result = await _db.ExecuteStoredProcedure("sp_get_sales_and_collection", obj); // returns List<dynamic>

                foreach (var row in result)
                {
                    var dict = new Dictionary<string, decimal>();
                    
                    string name = row.name;

                    foreach (var prop in ((IDictionary<string, object>)row))
                    {
                        var key = prop.Key;
                        var val = prop.Value;

                        if (key != "name")
                        {
                            if (val == null)
                            {
                                dict[key] = 0; // ✅ store null explicitly
                            }
                            else if (decimal.TryParse(val.ToString(), out var qty))
                            {
                                dict[key] = qty;
                            }
                        }
                    }

                    list.Add(new SalesCollectionModel
                    {
                        name = name,
                        month_qty = dict
                    });
                }

                return list;
            }
            catch
            {
                return new List<SalesCollectionModel>();
            }
        }
        #endregion


        #region GetSalesCollectionCommission 
        public async Task<List<SalesCollectionCommissionModel>> GetSalesCollectionCommission(SalesCollectionCommisssionFilter obj)
        {
            try
            {
                var list = new List<SalesCollectionCommissionModel>();
                var result = await _db.ExecuteStoredProcedure("sp_get_sales_and_collection_commission", obj); // returns List<dynamic>

                foreach (var row in result)
                {
                    var dict = new Dictionary<string, decimal>();

                    string name = row.name;

                    foreach (var prop in ((IDictionary<string, object>)row))
                    {
                        var key = prop.Key;
                        var val = prop.Value;

                        if (key != "name")
                        {
                            if (val == null)
                            {
                                dict[key] = 0; // ✅ store null explicitly
                            }
                            else if (decimal.TryParse(val.ToString(), out var qty))
                            {
                                dict[key] = qty;
                            }
                        }
                    }

                    list.Add(new SalesCollectionCommissionModel
                    {
                        name = name,
                        month_qty = dict
                    });
                }

                return list;
            }
            catch
            {
                return new List<SalesCollectionCommissionModel>();
            }
        }
        #endregion


        #region GetVendorPODetailReport
        public async Task<db_common_res> GetVendorPOProductDetail(VendorDetailRequestModel obj)
        {
            try
            {
                return await _db.ExecuteQueryFirstOrDefaultAsync<db_common_res>("sp_get_admin_purchase_order_products", obj);
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion


        #region GetVendorPOProductDetail 
        public async Task<db_common_res> GetVendorPODetailReport(VendorDetailRequestModel obj)
        {
            try
            {
                return await _db.ExecuteQueryFirstOrDefaultAsync<db_common_res>("sp_get_vendor_po_seller", obj);
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion


        #region GetSalesCollectionGraph
        public async Task<db_common_res> GetSalesCollectionGraph(SalesCollectionGraphFilter obj)
        {
            try
            {
                return await _db.ExecuteQueryFirstOrDefaultAsync<db_common_res>("sp_get_sales_and_collection_graph", obj);
            }
            catch (Exception) { return null; }
        }
        #endregion


        #region GetCategoryWiseGraph
        public async Task<db_common_res> GetCategoryWiseGraph(SalesCollectionGraphFilter obj)
        {
            try
            {
                return await _db.ExecuteQueryFirstOrDefaultAsync<db_common_res>("sp_get_category_graph", obj);
            }
            catch (Exception) { return null; }
        }
        #endregion


        #region GetProductWiseGraph
        public async Task<db_common_res> GetProductWiseGraph(SalesCollectionGraphFilter obj)
        {
            try
            {
                return await _db.ExecuteQueryFirstOrDefaultAsync<db_common_res>("sp_get_product_graph", obj);
            }
            catch (Exception) { return null; }
        }
        #endregion
 

        #region PartyWiseOutstanding
        public async Task<List<PartyWiseOutstandingViewModel>> PartyWiseOutstanding(PartyWiseOutstandingFilter obj)
        {
            try
            {
                var result = await _db.ExecuteStoredProcedureAsync<PartyWiseOutstandingViewModel>("sp_get_outstanding_party_wise", obj);
                return result.ToList();
            }
            catch
            {
                return null;
            }
        }
        #endregion


        #region Expenses
        public async Task<db_common_res> GetExpensesCategory()
        {
            try
            { 
                return await _db.ExecuteQueryFirstOrDefaultAsync<db_common_res>("sp_get_expenses_category");
            }
            catch (Exception exe)
            {
                return null;
            }
        } 
         
        public async Task<List<ExpensesMappingDto>> GetExpenses(ExpensesMappingFilter obj)
        {
            try
            {
                var result = await _db.ExecuteStoredProcedureAsync<ExpensesMappingDto>("sp_get_user_expenses_data", obj);
                return result.ToList();
            }
            catch (Exception exe)
            {
                return null;
            }
        }
        #endregion


        #region GetProfitLossReport
        public async Task<List<ProfitLossModel>> GetProfitLossReport(ProfitLossFilter obj)
        {
            try
            {
                var list = new List<ProfitLossModel>();
                var result = await _db.ExecuteStoredProcedure("usp_get_profit_and_loss", obj); // returns List<dynamic>

                foreach (var row in result)
                {
                    var dict = new Dictionary<string, decimal>(); 
                    string name = row.name;
                     int sno = row.sno;
                    foreach (var prop in ((IDictionary<string, object>)row))
                    {
                        var key = prop.Key;
                        var val = prop.Value;

                        if (key != "name" && key != "sno")
                        {
                            if (val == null)
                            {
                                dict[key] = 0; // ✅ store null explicitly
                            }
                            else if (decimal.TryParse(val.ToString(), out var qty))
                            {
                                dict[key] = qty;
                            }
                        }
                    }

                    list.Add(new ProfitLossModel
                    {
                        sno= sno,
                        name = name,
                        month_qty = dict
                    });
                } 
                return list;
            }
            catch
            {
                return new List<ProfitLossModel>();
            }
        }
        #endregion
         
         
        #region ProductList
        public async Task<List<ProductViewModel>> GetRootProductList(get_root_product obj)
        {
            try
            {
                var result = await _db.ExecuteStoredProcedureAsync<ProductViewModel>("sp_get_root_product", obj);
                return result.ToList();
            }
            catch (Exception exe)
            {
                return null;
            }
        }
        #endregion


        #region Product Details
        public async Task<ProductDetailViewModel> ProdDetail(get_root_product_detail obj)
        {
            try
            {
                return await _db.ExecuteQueryFirstOrDefaultAsync<ProductDetailViewModel>("sp_get_root_product_details", obj);
            }
            catch (Exception exe)
            {
                return null;
            }
        }

        Task<db_common_res> IGetServices.GetUserProductMapping(UserProductMappingFilter obj)
        {
            throw new NotImplementedException();
        }

        Task<db_common_res> IGetServices.GetUseTarget(UserTarggetFilter obj)
        {
            throw new NotImplementedException();
        }
        #endregion


        #region InvoicGet_Repurchase_Payout_Periode_List
        public async Task<List<matrix_payout_date_ViewModel>> Get_Repurchase_Payout_Period()
        {
            try
            {
                var result = await _db.ExecuteStoredProcedureAsync<matrix_payout_date_ViewModel>("sp_get_matrix_monthly_date");
                return result.ToList();
            }
            catch { return null; }
        }
        #endregion


        #region Repurchase_Payout_Dispatch
        public async Task<List<MatrixPayoutDispatchViewModel>> Repurchase_Payout_Dispatch(MatrixPayoutDispatchListFilter obj)
        {
            try
            {
                var result = await _db.ExecuteStoredProcedureAsync<MatrixPayoutDispatchViewModel>("sp_get_matrix_monthly_payout", obj);
                return result.ToList();
            }
            catch { return null; }
        }
        #endregion


        #region UserIncomeList
        public async Task<List<UserIncomeViewModel>> UserIncomeList(UserIncomeListFilter obj)
        {
            try
            {
                var result = await _db.ExecuteStoredProcedureAsync<UserIncomeViewModel>("usp_get_user_income_list", obj);
                return result.ToList();
            }
            catch { return null; }
        }
        #endregion


        #region GetMatrixTreeView
        public async Task<db_common_res> GetMatrixTreeView(matrix_tree_view_filter obj)
        {
            try
            {
                return await _db.ExecuteQueryFirstOrDefaultAsync<db_common_res>("sp_get_user_tree_view_matrix", obj);
            }
            catch (Exception exe) { return null; }
        }
        #endregion


        #region UserDashboad
        public async Task<db_common_res> UserDashboard(UserTarggetFilter obj)
        {
            try
            {
                return await _db.ExecuteQueryFirstOrDefaultAsync<db_common_res>("usp_get_user_dashboard", obj);
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion


        #region GetUserCompanyWalletRequest
        public async Task<List<UserCompanyWalletRequestViewModel>> GetUserCompanyWalletRequest(UserCompanyWalletRequestListFilter obj)
        {
            try
            {
                var result = await _db.ExecuteStoredProcedureAsync<UserCompanyWalletRequestViewModel>("usp_get_user_company_wallet_request", obj);
                return result.ToList();
            }
            catch { return null; }
        }
        #endregion


        #region UserCompanyWalletPassbook
        public async Task<List<UserCompanyWalletPassbookViewModel>> UserCompanyWalletPassbook(UserWalletPassbookFilter obj)
        {
            try
            {
                var result = await _db.ExecuteStoredProcedureAsync<UserCompanyWalletPassbookViewModel>("sp_get_user_wallet_transaction", obj);
                return result.ToList();
            }
            catch { return null; }
        }
        #endregion


        #region User Payout Wallet Passbook
        public async Task<List<UserPayoutWalletPassbookViewModel>> UserPayoutWalletPassbook(UserPayoutWalletPassbookFilter obj)
        {
            try
            {
                var result = await _db.ExecuteStoredProcedureAsync<UserPayoutWalletPassbookViewModel>("sp_get_user_payout_wallet_transaction", obj);
                return result.ToList();
            }
            catch { return null; }
        }
        #endregion


        #region Get User Binary Tree
        public async Task<List<BinaryUserDto>> GetUserBinaryTree(BinaryTeamFilter obj)
        {
            try
            {
                var dbResult = await _db.ExecuteStoredProcedureAsync<db_common_res>("sp_get_user_binary_tree", obj);
                var response = dbResult.FirstOrDefault();

                if (response == null || string.IsNullOrEmpty(response.json_result))
                    return new List<BinaryUserDto>();

                return JsonConvert.DeserializeObject<List<BinaryUserDto>>(response.json_result) ?? new List<BinaryUserDto>();
            }
            catch { return new List<BinaryUserDto>(); }
        }
        #endregion


        #region UserMatrixList
        public async Task<List<UserDownlineReport>> UserMatrixList(UserMatrixFilter obj)
        {
            try
            {
                var result = await _db.ExecuteStoredProcedureAsync<UserDownlineReport>("sp_get_user_matrix_list", obj);
                return result.ToList();
            }
            catch { return null; }
        }
        #endregion


        #region UserBinaryList
        public async Task<List<UserDownlineBinaryReport>> UserBinaryList(UserBinaryFilter obj)
        {
            try
            {
                var result = await _db.ExecuteStoredProcedureAsync<UserDownlineBinaryReport>("sp_get_user_binary_list", obj);
                return result.ToList();
            }
            catch { return null; }
        }
        #endregion


        #region GetOrdInvSuccess
        public async Task<db_common_res> GetOrdInvSuccess(GetOrderInvFilter obj)
        {
            try
            {
                return await _db.ExecuteQueryFirstOrDefaultAsync<db_common_res>("usp_get_ord_inv_success", obj);
            }
            catch (Exception exe)
            {
                return null;
            }
        }
        #endregion

    }

}
