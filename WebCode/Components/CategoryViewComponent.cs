using AppModels;
using AppServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;
namespace WebCode.Components
{
    [ViewComponent(Name = "Category")]
    public class CategoryViewComponent : ViewComponent
    {

        #region Constructors  
        private readonly IGetServices _getSvc;
        private readonly IMemoryCache _cache;
        public CategoryViewComponent(IGetServices getSvc, IMemoryCache cache)
        {
            _getSvc = getSvc;
            _cache = cache;
        }
        #endregion


        public async Task<IViewComponentResult> InvokeAsync()
        {
            string cacheKey = "CATEGORY_LIST";
            if (!_cache.TryGetValue(cacheKey, out List<CategoryViewModel> categories))
            {
                var searchObj = new guest_report_request_param
                { mst_key = "GET_CATEGORY_LIST" };
                var result = await _getSvc.master_reports(searchObj);
                if (result != null && !string.IsNullOrWhiteSpace(result.json_result))
                {
                    categories = JsonSerializer.Deserialize<List<CategoryViewModel>>
                    (
                        result.json_result, new JsonSerializerOptions
                        { PropertyNameCaseInsensitive = true }
                    ) ?? new List<CategoryViewModel>();
                }
                var cacheOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(12));
                _cache.Set(cacheKey, categories, cacheOptions);
            }
            return View(categories);
        }


        //public async Task<IViewComponentResult> InvokeAsync()
        //{
        //    string cacheKey = "CATEGORY_LIST";
        //    if (!_cache.TryGetValue(cacheKey, out List<dropdown_response> categories))
        //    {
        //        // First Time Database Call
        //        var search_obj = new dropdown_request_param { mst_key = "GET_CATEGORY_LIST" };
        //        categories = await _getSvc.master_dropdown(search_obj) as List<dropdown_response>;
        //        var cacheOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(12));
        //        _cache.Set(cacheKey, categories, cacheOptions);
        //    }
        //    return View(categories);
        //}
    }

}
