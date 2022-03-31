using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using static Domain.Models.QuestionViewModel;

namespace ULMS.API.Controllers
{
    //[Authorize(Roles ="learner")]
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionService _questionService;
        public QuestionController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        [Route("AddQuestion")]
        [HttpPost]
        public async Task<IActionResult> AddQuestion([FromBody] CreateQuestionRequestModel model)
        {
            return Ok(await _questionService.AddQuestion(model));
        }

        [Route("DeleteQuestion/{id}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteQuestion([FromRoute] Guid id)
        {
            var response = await _questionService.DeleteQuestion(id);
            return Ok(response);
        }

        [Route("UpdateQuestion/{id}")]
        [HttpPut]
        public async Task<IActionResult> UpdateQuestion([FromRoute] Guid id, [FromBody] UpdateQuestionRequestModel model)
        {
            var response = await _questionService.UpdateQuestion(id, model);
            return Ok(response);
        }

        [Route("GetQuestionById/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetQuestionById([FromRoute] Guid id)
        {
            var response = await _questionService.GetQuestionById(id);
            return Ok(response);
        }


        //Check for Query
        [Authorize]
        [Route("GetQuestionsByModule/{moduleId}")]
        [HttpGet]
        public async Task<IActionResult> GetQuestionsByCourse([FromRoute] Guid moduleId)
        {
            var response = await _questionService.GetQuestionsByModule(moduleId);
            return Ok(response);
        }

        [Authorize]
        //[Route()]
        [HttpGet("GetAllQuestions")]
        public async Task<IActionResult> GetAllQuestions()
        {

            var response = await _questionService.GetAllQuestions();
            return Ok(response);
        }
    }
}
