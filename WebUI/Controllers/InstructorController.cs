using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    public class InstructorController : Controller
    {
        public IActionResult AddInstructor()
        {
            return View();
        }

        public IActionResult InstructorProfile()
        {
            return View();
        }

        public IActionResult ViewAllInstructors()
        {
            return View();
        }
    }
}
