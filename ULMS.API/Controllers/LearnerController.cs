using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Domain.Models.LearnerViewModel;

namespace ULMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LearnerController : ControllerBase
    {
        private readonly ILearnerService _learnerService;
        public LearnerController(ILearnerService learnerService)
        {
            _learnerService = learnerService;
        }

        [Route("AddLearner")]
        [HttpPost]
        public async Task<IActionResult> AddLearner([FromBody] CreateLearnerRequestModel model)
        {
            var response = await _learnerService.AddLearner(model);
            return Ok(response);
        }

        [Route("Delete/{id}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteLearner([FromRoute] Guid id)
        {
            var response = await _learnerService.DeleteLearner(id);
            return Ok(response);
        }

        [Route("UpdateLearner/{id}")]
        [HttpPut]
        public async Task<IActionResult> UpdateLearner([FromRoute] Guid id, [FromBody] UpdateLearnerRequestModel model)
        {
            var response = await _learnerService.UpdateLearner(id, model);
            return Ok(response);
        }

        [Route("GetLearnerById/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetLearnerById([FromRoute] Guid id)
        {
            var response = await _learnerService.GetLearnerById(id);
            return Ok(response);
        }

        [Route("GetAllLearners")]
        [HttpGet]
        public async Task<IActionResult> GetAllLearners()
        {
            var response = await _learnerService.GetAllLearners();
            return Ok(response);
        }

        [Route("GetLearnersByCourse/{courseId}")]
        [HttpGet]
        public async Task<IActionResult> GetLearnersByCourse([FromRoute] Guid courseId)
        {
            var response = await _learnerService.GetLearnersByCourse(courseId);
            return Ok(response);
        }

        [Route("GetByEmail/{email}")]
        [HttpGet]
        public async Task<IActionResult> GetLearnerByEmail([FromRoute] string email)
        {
            var response = await _learnerService.GetLearnerByEmail(email);
            return Ok(response);
        }
    }
}
