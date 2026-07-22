using Microsoft.AspNetCore.Mvc;

namespace WebCode.Components
{
    [ViewComponent(Name= "TcInvoice")]
    public class TcInvoiceViewComponent : ViewComponent
    { 
        public IViewComponentResult Invoke()
        {
            return View(); // no model needed
        }
    }
}
