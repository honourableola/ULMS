using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    public class TopicController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
