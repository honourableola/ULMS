using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    public class InstructorController : Controller
    {
        public IActionResult DashBoard()
        {
            return View();
        }

        public IActionResult SampleInstructorTable()
        {
            return View();
        }
        public IActionResult AddInstructor()
        {
            return View();
        }

        public IActionResult InstructorProfile()
        {
            return View();
        }

        public IActionResult UpdateInstructor()
        {
            return View();
        }

        public IActionResult ViewAllInstructors()
        {
            return View();
        }
    }
}
