using Microsoft.AspNetCore.Mvc;

namespace WebCode.Components
{
    public class TcInvoiceComponent: ViewComponent
    {
        //public IViewComponentResult Invoke()
        //{
        //    string data = "Hello TC Invoice";
        //    return View(data);
        //}

        //public IViewComponentResult Invoke()
        //{
        //    return Content("Component is working!");
        //}

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var data = await Task.FromResult("Hello from Component");
            return View("Default", data);
        }
    }
}
