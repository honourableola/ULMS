using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
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

        //[Authorize]
        [Route("AddLearner")]
        [HttpPost]
        public async Task<IActionResult> AddLearner([FromBody] CreateLearnerRequestModel model)
        {

           /* if (file != null)
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
            }*/
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

        //[Authorize]
        [Route("GetAllLearners")]
        [HttpGet]
        public async Task<IActionResult> GetAllLearners()
        {
            var response = await _learnerService.GetAllLearners();
            return Ok(response);
        }

        [Route("GetLearners")]
        [HttpPost]
        public async Task<IActionResult> GetLearners()
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
                var learners = await _learnerService.GetAllLearners();
                var learnerData = learners.Data;
                /*if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    instructorData = instructorData.OrderBy(sortColumn + " " + sortColumnDirection);
                }*/
                if (!string.IsNullOrEmpty(searchValue))
                {
                    learnerData = learnerData.Where(m => m.FirstName.ToLower().Contains(searchValue)
                                                || m.LastName.ToLower().Contains(searchValue)
                                                || m.LearnerLMSCode.ToLower().Contains(searchValue)
                                                || m.Email.ToLower().Contains(searchValue));
                }
                recordsTotal = learnerData.Count();
                var data = learnerData.Skip(skip).Take(pageSize).ToList();
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
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
