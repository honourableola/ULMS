using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
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
/*
        [Route("GetAllOptions")]
        [HttpGet]
        public async Task<IActionResult> GetAllOptions()
        {
            var response = await _optionService.GetAllOptions();
            return Ok(response);
        }*/

        [Route("GetOptions")]
        [HttpPost]
        public async Task<IActionResult> GetOptions()
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
                var options = await _optionService.GetAllOptions();
                var optionData = options.Data;
                /*if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    instructorData = instructorData.OrderBy(sortColumn + " " + sortColumnDirection);
                }*/
                if (!string.IsNullOrEmpty(searchValue))
                {
                    optionData = optionData.Where(m => m.Label.ToLower().Contains(searchValue)
                                                 || m.OptionText.ToLower().Contains(searchValue)
                                                 || m.Status.ToString().ToLower().Contains(searchValue)
                                                 );

                }
                recordsTotal = optionData.Count();
                var data = optionData.Skip(skip).Take(pageSize).ToList();
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
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
