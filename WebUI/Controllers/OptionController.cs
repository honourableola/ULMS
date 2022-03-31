using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    public class OptionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
