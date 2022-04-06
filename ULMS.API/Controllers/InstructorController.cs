using Domain.Interfaces.Services;
using Domain.Models.Datatable;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using System.Linq.Dynamic.Core;
using System;
using System.Linq;
using System.Threading.Tasks;
using static Domain.Models.InstructorViewModel;

namespace ULMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly IInstructorService _instructorService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public InstructorController(IInstructorService instructorService, IWebHostEnvironment webHostEnvironment, ApplicationContext context)
        {
            _instructorService = instructorService;
            _webHostEnvironment = webHostEnvironment;
            _context = context;
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

        [Route("GetInstructors")]
        [HttpPost]
        public async Task<IActionResult> GetInstructors()
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
                var instructors = await _instructorService.GetAllInstructors();
                var instructorData = instructors.Data;
                /*if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    instructorData = instructorData.OrderBy(sortColumn + " " + sortColumnDirection);
                }*/
                if (!string.IsNullOrEmpty(searchValue))
                {
                    instructorData = instructorData.Where(m => m.FirstName.ToLower().Contains(searchValue)
                                                || m.LastName.ToLower().Contains(searchValue)
                                                || m.InstructorLMSCode.ToLower().Contains(searchValue)
                                                || m.Email.ToLower().Contains(searchValue));
                }
                recordsTotal = instructorData.Count();
                var data = instructorData.Skip(skip).Take(pageSize).ToList();
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("Data")]
        [HttpGet]
        public async Task<IActionResult> LoadAllInstructors(DatatableRequest dataTable)
        {
            var page = dataTable.Pagination.Page;
            var limit = dataTable.Pagination.PerPage;

            var filter = await dataTable.Query.Get("filter",
                () => Task.FromResult<string?>(null),
                s => string.IsNullOrWhiteSpace(s)
                ? null : s.Trim());
          
            var instances = await _instructorService.LoadInstructorsAsync(filter, page, limit);
            var totalPages = (instances.TotalCount + limit - 1) / limit;
            var list = instances.Rows.ToList();
            var meta = new
            {
                page,
                perpage = limit,
                pages = totalPages,
                total = instances.TotalCount
            };
            return Ok(new
            {
                meta,
                data = list
            });
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
