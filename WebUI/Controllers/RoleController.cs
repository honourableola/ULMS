using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    public class RoleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
