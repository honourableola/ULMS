using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
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
/*
        [Authorize]
        //[Route()]
        [HttpGet("GetAllQuestions")]
        public async Task<IActionResult> GetAllQuestions()
        {

            var response = await _questionService.GetAllQuestions();
            return Ok(response);
        }*/

        [Route("GetQuestions")]
        [HttpPost]
        public async Task<IActionResult> GetQuestions()
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault().ToLower();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                var options = await _questionService.GetAllQuestions();
                var optionData = options.Data;
                /*if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    instructorData = instructorData.OrderBy(sortColumn + " " + sortColumnDirection);
                }*/
                if (!string.IsNullOrEmpty(searchValue))
                {
                    optionData = optionData.Where(m => m.Points.ToString().ToLower().Contains(searchValue)
                                                 || m.QuestionText.ToLower().Contains(searchValue)
                                                 || m.ModuleName.ToLower().Contains(searchValue)
                                                 );

                }
                recordsTotal = optionData.Count();
                var data = optionData.Skip(skip).Take(pageSize).ToList();
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
