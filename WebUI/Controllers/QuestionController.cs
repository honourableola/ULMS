using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    public class QuestionController : Controller
    {
        public IActionResult AddQuestion()
        {
            return View();
        }

        public IActionResult UpdateQuestion()
        {
            return View();
        }

        public IActionResult ViewQuestionDetails()
        {
            return View();
        }

        public IActionResult GetQuestions()
        {
            return View();
        }
    }
}
