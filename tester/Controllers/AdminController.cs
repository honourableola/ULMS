using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Domain.Models.AdminViewModel;

namespace tester.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }



        public IActionResult Index()
        {
            var admins = _adminService.GetAllAdmins();
            return View(admins);
        }

        public IActionResult Create()
        {
            return View();
        }

       /* [HttpPost]
        public IActionResult Create(CreateAdminRequestModel model)
        {
            _adminService.AddAdmin(model);
            return RedirectToAction("Index");
        }*/
    }
}
