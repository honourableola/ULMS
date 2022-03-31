using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    public class AdminController : Controller
    {

        public IActionResult DashBoard()
        {
            return View();
        }

        public IActionResult AddAdmin()
        {
            return View();
        }

        public IActionResult ViewAdminProfile()
        {
            return View();
        }

        public IActionResult ViewAllAdmins()
        {
            return View();
        }

        public IActionResult UpdateAdmin()
        {
            return View();
        }
    }
}
