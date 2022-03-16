using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using static Domain.Models.InstructorViewModel;

namespace ULMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorController : ControllerBase
    {
        private readonly IInstructorService _instructorService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public InstructorController(IInstructorService instructorService, IWebHostEnvironment webHostEnvironment)
        {
            _instructorService = instructorService;
            _webHostEnvironment = webHostEnvironment;
        }

        [Route("AddInstructor")]
        [HttpPost]
        public async Task<IActionResult> AddInstructor([FromBody] CreateInstructorRequestModel model)
        {
           /* if (file != null)
            {
                string imageDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "InstructorImages");
                Directory.CreateDirectory(imageDirectory);
                string contentType = file.ContentType.Split('/')[1];
                string instructorImage = $"{Guid.NewGuid()}.{contentType}";
                string fullPath = Path.Combine(imageDirectory, instructorImage);
                using (var fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
                model.InstructorPhoto = instructorImage;
            }*/
            var response = await _instructorService.AddInstructor(model);
            return Ok(response);
        }

        [Route("Delete/{id}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteInstructor([FromRoute] Guid id)
        {
            var response = await _instructorService.DeleteInstructor(id);
            return Ok(response);
        }

        [Route("UpdateInstructor/{id}")]
        [HttpPut]
        public async Task<IActionResult> UpdateInstructor([FromRoute] Guid id, [FromBody] UpdateInstructorRequestModel model)
        {
            var response = await _instructorService.UpdateInstructor(id, model);
            return Ok(response);
        }

        [Route("GetInstructorById/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetinstructorById([FromRoute] Guid id)
        {
            var response = await _instructorService.GetInstructorById(id);
            return Ok(response);
        }

        [Route("GetAllInstructors")]
        [HttpGet]
        public async Task<IActionResult> GetAllInstructors()
        {
            var response = await _instructorService.GetAllInstructors();
            return Ok(response);
        }

        [Route("GetInstructorsByCourse/{courseId}")]
        [HttpGet]
        public async Task<IActionResult> GetInstructorsByCourse([FromRoute] Guid courseId)
        {
            var response = await _instructorService.GetInstructorsByCourse(courseId);
            return Ok(response);
        }

        [Route("GetByEmail/{email}")]
        [HttpGet]
        public async Task<IActionResult> GetInstructorsByEmail([FromRoute] string email)
        {
            var response = await _instructorService.GetInstructorByEmail(email);
            return Ok(response);
        }

        [Route("SearchInstructorsByName/{searchText}")]
        [HttpGet]
        public async Task<IActionResult> SearchInstructorsByName([FromRoute] string searchText)
        {
            var response = await _instructorService.SearchInstructorsByName(searchText);
            return Ok(response);
        }
    }
}
