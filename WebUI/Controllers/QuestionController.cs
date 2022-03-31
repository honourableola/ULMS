using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    public class QuestionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
