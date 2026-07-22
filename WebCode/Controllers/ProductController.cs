using AppModels;
using AppServices;
using AppWeb.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace WebCode.Controllers
{
    [Route("product")]
    public class ProductController : BaseController
    { 

        #region Constructors   
        private readonly IGetServices _getSvc;
        public ProductController(IConfiguration config, IAddServices addServices,
            IGetServices getSvc, IMemoryCache cache) : base(config, addServices, getSvc, cache)
        {
            _getSvc = getSvc;
        }
        #endregion


        [HttpGet("")]
        public async Task<IActionResult> Product(int? cat_id, int? sub_cat_id, string? cat_name, string? sub_cat_name)
        {
            ViewBag.cat_id = cat_id;
            ViewBag.sub_cat_id = sub_cat_id;
            ViewBag.cat_name = cat_name;
            ViewBag.sub_cat_name = sub_cat_name;

            var req = new get_root_product
            {
                seller_uid = Const.DEFAULT_SALLER,
                uid = 0,
                cat_id = cat_id,
                sub_cat_id = sub_cat_id,
                fetch_type = "ROOT"  // Can be changed to "BY_CATEGORY" or "BY_SUBCATEGORY" if needed
            };
            try
            {
                var products = await _getSvc.GetRootProductList(req) ?? new List<ProductViewModel>();
                return View(products);
            }
            catch (Exception ex)
            {
                return View(new List<ProductViewModel>()); // Return empty list on error
            }
        }
 

        [HttpGet("{seo_url}")]
        public async Task<IActionResult> Detail(string seo_url)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(seo_url))
                    return NotFound(nameof(Index));

                get_root_product_detail req = new get_root_product_detail();
                req.seller_uid = Const.DEFAULT_SALLER;
                req.seo_url = seo_url;
                var model = await _getSvc.ProdDetail(req);
                if (model != null)
                {
                    model.ImageList = new List<string>();
                    var images = new[] { model.img1, model.img2, model.img3, model.img4, model.img5, model.img6 };
                    foreach (var img in images)
                    {
                        if (!string.IsNullOrWhiteSpace(img))
                        {
                            model.ImageList.Add(img.Trim());
                        }
                    }

                    model.Features = new List<string>();
                    if (model.t1 != "")
                    {
                        model.Features.Add(model.t1);
                        model.Features.Add(model.d1);
                    }
                    if (model.t2 != "")
                    {
                        model.Features.Add(model.t2);
                        model.Features.Add(model.d2);
                    }
                    if (model.t3 != "")
                    {
                        model.Features.Add(model.t3);
                        model.Features.Add(model.d3);
                    }
                    if (model.t4 != "")
                    {
                        model.Features.Add(model.t4);
                        model.Features.Add(model.d4);
                    }
                    if (model.t5 != "")
                    {
                        model.Features.Add(model.t5);
                        model.Features.Add(model.d5);
                    }
                }
                return View(model);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }


        [HttpGet("get-product")]
        public async Task<IActionResult> GetProducts(int? cart_id = 0, string? fetch_type = null)
        {
            var req = new get_root_product
            {
                seller_uid = Const.DEFAULT_SALLER,
                uid = 0,
                cat_id = cart_id,
                sub_cat_id = 0,
                fetch_type = fetch_type
            };
            try
            {
                var products = await _getSvc.GetRootProductList(req) ?? new List<ProductViewModel>();
                return Ok(new { success = true, token_valid = true, data = products, message = "" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, token_valid = true, message = "Internal server error", details = ex.Message });
            }
        }

    }
}
