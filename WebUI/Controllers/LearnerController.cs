﻿using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    public class LearnerController : Controller
    {
        public IActionResult AddLearner()
        {
            return View();
        }

        public IActionResult LearnerProfile()
        {
            return View();
        }

        public IActionResult ViewAllLearners()
        {
            return View();
        }
    }
}
