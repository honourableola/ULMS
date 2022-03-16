using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using static Domain.Models.OptionViewModel;

namespace ULMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OptionController : ControllerBase
    {
        private readonly IOptionService _optionService;
        public OptionController(IOptionService optionService)
        {
            _optionService = optionService;
        }
        [Route("AddOption")]
        [HttpPost]
        public async Task<IActionResult> AddOption([FromBody] CreateOptionRequestModel model)
        {
            return Ok(await _optionService.AddOption(model));
        }

        [Route("DeleteOption/{id}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteOption([FromRoute] Guid id)
        {
            var response = await _optionService.DeleteOption(id);
            return Ok(response);
        }

        [Route("UpdateOption/{id}")]
        [HttpPut]
        public async Task<IActionResult> UpdateOption([FromRoute] Guid id, [FromBody] UpdateOptionRequestModel model)
        {
            var response = await _optionService.UpdateOption(id, model);
            return Ok(response);
        }

        [Route("GetOptionsByQuestion/{questionId}")]
        [HttpGet]
        public async Task<IActionResult> GetOptionsByQuestion([FromRoute] Guid questionId)
        {
            var response = await _optionService.GetOptionsByQuestion(questionId);
            return Ok(response);
        }

        [Route("GetOptionById/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetOptionById([FromRoute] Guid id)
        {
            var response = await _optionService.GetOption(id);
            return Ok(response);
        }

    }
}
