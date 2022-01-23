using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Domain.Models.TopicViewModel;

namespace ULMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TopicController : ControllerBase
    {
        private readonly ITopicService _topicService;
        public TopicController(ITopicService topicService)
        {
            _topicService = topicService;
        }

        [Route("AddTopic")]
        [HttpPost]
        public async Task<IActionResult> AddTopic([FromBody] CreateTopicRequestModel model)
        {
            var response = await _topicService.AddTopic(model);
            return Ok(response);
        }

        [Route("Delete/{id}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteTopic([FromRoute] Guid id)
        {
            var response = await _topicService.DeleteTopic(id);
            return Ok(response);
        }

        [Route("UpdateTopic/{id}")]
        [HttpPut]
        public async Task<IActionResult> UpdateTopic([FromRoute] Guid id, [FromBody] UpdateTopicRequestModel model)
        {
            var response = await _topicService.UpdateTopic(id, model);
            return Ok(response);
        }

        [Route("GetTopicById/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetTopicById([FromRoute] Guid id)
        {
            var response = await _topicService.GetTopic(id);
            return Ok(response);
        }

        [Route("GetTopicsByModule/{moduleId}")] 
        [HttpGet]
        public async Task<IActionResult> GetTopicsByModule([FromRoute] Guid moduleId)
        {
            var response = await _topicService.GetTopicsByModule(moduleId);
            return Ok(response);
        }

        [Route("GetAllTopics")]
        [HttpGet]
        public async Task<IActionResult> GetAllTopics()
        {
            var response = await _topicService.GetAllTopics();
            return Ok(response);
        }

        [Route("SearchTopicsByTitle/{searchText}")]
        [HttpGet]
        public async Task<IActionResult> SearchTopicsByTitle([FromRoute] string searchText)
        {
            var response = await _topicService.SearchTopicsByTitle(searchText);
            return Ok(response);
        }

    }
}
