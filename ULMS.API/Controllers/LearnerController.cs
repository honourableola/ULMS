using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
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
        private readonly IWebHostEnvironment _webHostEnvironment;
        public LearnerController(ILearnerService learnerService, IWebHostEnvironment webHostEnvironment)
        {
            _learnerService = learnerService;
            _webHostEnvironment = webHostEnvironment;
        }

        [Route("AddLearner")]
        [HttpPost]
        public async Task<IActionResult> AddLearner([FromBody] CreateLearnerRequestModel model, IFormFile file)
        {

            if (file != null)
            {
                string imageDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "LearnerImages");
                Directory.CreateDirectory(imageDirectory);
                string contentType = file.ContentType.Split('/')[1];
                string learnerImage = $"{Guid.NewGuid()}.{contentType}";
                string fullPath = Path.Combine(imageDirectory, learnerImage);
                using (var fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
                model.LearnerPhoto = learnerImage;
            }
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

        [Route("SearchLearnersByName/{searchText}")]
        [HttpGet]
        public async Task<IActionResult> SearchLearnersByName([FromRoute] string searchText)
        {
            var response = await _learnerService.SearchLearnersByName(searchText);
            return Ok(response);
        }
    }
}
