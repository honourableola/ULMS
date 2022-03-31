using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    public class CourseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult allCourses()
        {
            return View();
        }
    }
}
