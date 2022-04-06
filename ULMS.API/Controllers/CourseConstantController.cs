using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using static Domain.Models.CourseConstantViewModel;

namespace ULMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseConstantController : ControllerBase
    {
        private readonly ICourseConstantService _courseConstantService;
        public CourseConstantController(ICourseConstantService courseConstantService)
        {
            _courseConstantService = courseConstantService;
        }

        [Route("AddCourseConstant")]
        [HttpPost]
        public async Task<IActionResult> AddCourseConstant(CreateCourseConstantRequestModel model)
        {
            var response = await _courseConstantService.AddCourseConstant(model);
            return Ok(response);
        }

        [Route("UpdateCourseConstant")]
        [HttpPut]
        public async Task<IActionResult> UpdateCourseConstant(UpdateCourseConstantRequestModel model)
        {
            var response = await _courseConstantService.UpdateCourseConstant(model);
            return Ok(response);
        }

       /* [Route("GetCourseConstant")]
        [HttpGet]
        public IActionResult GetCourseConstant()
        {
            var response = _courseConstantService.GetCourseConstant();
            return Ok(response);
        }*/

        [Route("GetCourseConstant")]
        [HttpPost]
        public async Task<IActionResult> GetCourseConstant()
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
                var courseConstant = _courseConstantService.GetCourseConstant();
               
                /*if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    instructorData = instructorData.OrderBy(sortColumn + " " + sortColumnDirection);
                }*/
               /* if (!string.IsNullOrEmpty(searchValue))
                {
                    courseConstant = courseConstant.Where(m => m.ToLower().Contains(searchValue)
                                                || m.LastName.ToLower().Contains(searchValue)
                                                || m.InstructorLMSCode.ToLower().Contains(searchValue)
                                                || m.Email.ToLower().Contains(searchValue));
                }*/
                recordsTotal = 1;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal};
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
