using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Domain.Models.ModuleViewModel;

namespace ULMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModuleController : ControllerBase
    {
        private readonly IModuleService _moduleService;
        public ModuleController(IModuleService moduleService)
        {
            _moduleService = moduleService;
        }

        [Route("AddModule")]
        [HttpPost]
        public async Task<IActionResult> AddModule([FromBody] CreateModuleRequestModel model)
        {
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

    }
}
