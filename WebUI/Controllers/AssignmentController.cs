using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    public class AssignmentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
