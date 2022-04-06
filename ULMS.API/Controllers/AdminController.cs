using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static Domain.Models.AdminViewModel;

namespace ULMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AdminController(IAdminService adminService, IWebHostEnvironment webHostEnvironment)
        {
            _adminService = adminService;
            _webHostEnvironment = webHostEnvironment;
        }


        [Route("AddAdmin")]
        [HttpPost]
        public async Task<IActionResult> AddAdmin(/*[FromHeader(Name = "Tenant")] string tenant,*/ [FromBody]CreateAdminRequestModel model/*, IFormFile file*/)
        {
            var response = await _adminService.AddAdmin(model);
            return Ok(response);
        }

        [Route("UploadAdminPicture/{adminId}")]
        [HttpPost]
        public async Task<IActionResult> UploadAdminImage([FromHeader(Name = "Tenant")] string tenant, [FromRoute] Guid adminId, IFormFile file)
        {
            if (file != null)
            {
                string imageDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "AdminImages");
                Directory.CreateDirectory(imageDirectory);
                string contentType = file.ContentType.Split('/')[1];
                string adminImage = $"{Guid.NewGuid()}.{contentType}";
                string fullPath = Path.Combine(imageDirectory, adminImage);
                using (var fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
                //model.AdminPhoto = adminImage;
            }
            //var response = await _adminService.UploadPhoto(adminId, model);
            return Ok();
        }

        [Route("UpdateAdmin/{id}")]
        [HttpPut]
        public async Task<IActionResult> UpdateAdmin([FromRoute] Guid id, [FromBody] UpdateAdminRequestModel model)
        {
            var response = await _adminService.UpdateAdmin(id, model);
            return Ok(response);
        }

        [Route("DeleteAdmin/{id}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteAdmin ([FromRoute] Guid id)
        {
            var response = await _adminService.DeleteAdmin(id);
            return Ok(response);
        }

        [Route("GetAdminById/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetAdminById ([FromRoute] Guid id)
        {
            var response = await _adminService.GetAdmin(id);
            return Ok(response);
        }

        [Route("GetAllAdmins")]
        [HttpGet]
        public async Task<IActionResult> GetAllAdmins ()
        {
            var response = await _adminService.GetAllAdmins();
            return Ok(response);
        }

        [Route("getAdmins")]
        [HttpPost]
        public async Task<IActionResult> GetAdmins()
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
                var instructors = await _adminService.GetAllAdmins();
                var instructorData = instructors.Data;
                /*if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    instructorData = instructorData.OrderBy(sortColumn + " " + sortColumnDirection);
                }*/
                if (!string.IsNullOrEmpty(searchValue))
                {
                    instructorData = instructorData.Where(m => m.FirstName.ToLower().Contains(searchValue)
                                                || m.LastName.ToLower().Contains(searchValue)
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

    }
}
