using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Domain.Models.AssessmentViewModel;

namespace ULMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssessmentController : ControllerBase
    {
        private readonly IAssessmentService _assessmentService;
        public AssessmentController(IAssessmentService assessmentService)
        {
            _assessmentService = assessmentService;
        }

        [Route("GenerateAssessment")]
        [HttpPost]
        public async Task<IActionResult> GenerateAssessment([FromBody] CreateAssessmentRequestModel model)
        {
            return Ok(await _assessmentService.GenerateAssessment(model));
        }

        [Route("DeleteAssessment/{id}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteAssessment([FromRoute] Guid id)
        {
            var response = await _assessmentService.DeleteAssessment(id);
            return Ok(response);
        }

        [Route("UpdateAssessment/{id}")]
        [HttpPut]
        public async Task<IActionResult> UpdateAssessment([FromRoute] Guid id, [FromBody] UpdateAssessmentRequestModel model)
        {
            var response = await _assessmentService.UpdateAssessment(id, model);
            return Ok(response);
        }

        [Route("GetAssessmentById/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetAssessmentById([FromRoute] Guid id)
        {
            var response = await _assessmentService.GetAssessment(id);
            return Ok(response);
        }

        [Route("GetAssessmentsByCourse/{courseId}")]
        [HttpGet]
        public async Task<IActionResult> GetAssessmentsByCourse([FromRoute] Guid courseId)
        {
            var response = await _assessmentService.GetAssessmentsByCourse(courseId);
            return Ok(response);
        }

        [Route("GetAllAssessments")]
        [HttpGet]
        public async Task<IActionResult> GetAllAssessments()
        {
            var response = await _assessmentService.GetAllAssessments();
            return Ok(response);
        }

        /*[Route("SearchTopicsByTitle/{searchText}")]
        [HttpGet]
        public async Task<IActionResult> SearchTopicsByTitle([FromRoute] string searchText)
        {
            var response = await _topicService.SearchTopicsByTitle(searchText);
            return Ok(response);
        }*/

    }
}
