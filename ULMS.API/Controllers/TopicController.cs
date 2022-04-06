using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System;
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

        /*[Route("GetAllTopics")]
        [HttpGet]
        public async Task<IActionResult> GetAllTopics()
        {
            var response = await _topicService.GetAllTopics();
            return Ok(response);
        }*/

        [Route("GetTopics")]
        [HttpPost]
        public async Task<IActionResult> GetTopics()
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
                var topics = await _topicService.GetAllTopics();
                var topicData = topics.Data;
                /*if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    instructorData = instructorData.OrderBy(sortColumn + " " + sortColumnDirection);
                }*/
                if (!string.IsNullOrEmpty(searchValue))
                {
                    topicData = topicData.Where(m => m.ModuleName.ToString().ToLower().Contains(searchValue)
                                                 || m.Title.ToLower().Contains(searchValue)
                                                 || m.Content.ToLower().Contains(searchValue)
                                                 );

                }
                recordsTotal = topicData.Count();
                var data = topicData.Skip(skip).Take(pageSize).ToList();
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
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
