using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using static Domain.Models.ModuleViewModel;

namespace ULMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModuleController : ControllerBase
    {
        private readonly IModuleService _moduleService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ModuleController(IModuleService moduleService, IWebHostEnvironment webHostEnvironment)
        {
            _moduleService = moduleService;
            _webHostEnvironment = webHostEnvironment;
        }

        [Route("AddModule")]
        [HttpPost]
        public async Task<IActionResult> AddModule([FromBody] CreateModuleRequestModel model /*IFormFile image1, IFormFile image2, IFormFile pdf1, IFormFile pdf2, IFormFile videoFile1, IFormFile videoFile2*/)
        {
            /*if (image1 != null || image2 != null)
            {
                string imageDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "ModuleImages");
                Directory.CreateDirectory(imageDirectory);
                string contentType1 = image1.ContentType.Split('/')[1];
                string contentType2 = image2.ContentType.Split('/')[1];
                string moduleImage1 = $"{Guid.NewGuid()}.{contentType1}";
                string moduleImage2 = $"{Guid.NewGuid()}.{contentType2}";
                string fullPath1 = Path.Combine(imageDirectory, moduleImage1);
                string fullPath2 = Path.Combine(imageDirectory, moduleImage2);
                using (var fileStream = new FileStream(fullPath1, FileMode.Create))
                {
                    image1.CopyTo(fileStream);
                }
                using (var fileStream = new FileStream(fullPath2, FileMode.Create))
                {
                    image2.CopyTo(fileStream);
                }
                model.ModuleImage1 = moduleImage1;
                model.ModuleImage2 = moduleImage2;
            }

            if (pdf1 != null || pdf2 != null)
            {
                string pdfDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "ModulePDFs");
                Directory.CreateDirectory(pdfDirectory);
                string contentType1 = pdf1.ContentType.Split('/')[1];
                string contentType2 = pdf2.ContentType.Split('/')[1];
                string modulePdf1 = $"{Guid.NewGuid()}.{contentType1}";
                string modulePdf2 = $"{Guid.NewGuid()}.{contentType2}";
                string fullPath1 = Path.Combine(pdfDirectory, modulePdf1);
                string fullPath2 = Path.Combine(pdfDirectory, modulePdf2);
                using (var fileStream = new FileStream(fullPath1, FileMode.Create))
                {
                    pdf1.CopyTo(fileStream);
                }
                using (var fileStream = new FileStream(fullPath2, FileMode.Create))
                {
                    pdf2.CopyTo(fileStream);
                }
                model.ModulePDF1 = modulePdf1;
                model.ModulePDF2 = modulePdf2;
            }

            if (videoFile1 != null || videoFile2 != null)
            {
                string videoDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "ModuleVideos");
                Directory.CreateDirectory(videoDirectory);
                string contentType1 = pdf1.ContentType.Split('/')[1];
                string contentType2 = pdf2.ContentType.Split('/')[1];
                string moduleVideo1 = $"{Guid.NewGuid()}.{contentType1}";
                string moduleVideo2 = $"{Guid.NewGuid()}.{contentType2}";
                string fullPath1 = Path.Combine(videoDirectory, moduleVideo1);
                string fullPath2 = Path.Combine(videoDirectory, moduleVideo2);
                using (var fileStream = new FileStream(fullPath1, FileMode.Create))
                {
                    videoFile1.CopyTo(fileStream);
                }
                using (var fileStream = new FileStream(fullPath2, FileMode.Create))
                {
                    videoFile2.CopyTo(fileStream);
                }
                model.ModuleVideo1 = moduleVideo1;
                model.ModuleVideo2 = moduleVideo2;
            }*/
            var response = await _moduleService.AddModule(model);
            return Ok(response);
        }

        [Route("Delete/{id}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteModule([FromRoute] Guid id)
        {
            var response = await _moduleService.DeleteModule(id);
            return Ok(response);
        }

        [Route("UpdateModule/{id}")]
        [HttpPut]
        public async Task<IActionResult> UpdateModule([FromRoute] Guid id, [FromBody] UpdateModuleRequestModel model)
        {
            var response = await _moduleService.UpdateModule(id, model);
            return Ok(response);
        }

        [Route("GetById/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetModuleById([FromRoute] Guid id)
        {
            var response = await _moduleService.GetModule(id);
            return Ok(response);
        }

        [Route("GetAllModules")]
        [HttpGet]
        public async Task<IActionResult> GetAllModules()
        {
            var response = await _moduleService.GetAllModules();
            return Ok(response);
        }

        [Route("GetModulesByCourse/{courseId}")]
        [HttpGet]
        public async Task<IActionResult> GetModulesByCourse([FromRoute] Guid courseId)
        {
            var response = await _moduleService.GetModulesByCourse(courseId);
            return Ok(response);
        }

        [Route("SearchModulesByName/{searchText}")]
        [HttpGet]
        public async Task<IActionResult> SearchModulesByName([FromRoute] string searchText)
        {
            var response = await _moduleService.SearchModulesByName(searchText);
            return Ok(response);
        }

    }
}
