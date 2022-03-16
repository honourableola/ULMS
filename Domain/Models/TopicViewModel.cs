using Domain.DTOs;
using System;
using System.Collections.Generic;

namespace Domain.Models
{
    public class TopicViewModel
    {
        public class CreateTopicRequestModel
        {
            public string Title { get; set; }

            public string Content { get; set; }

            public Guid ModuleId { get; set; }

        }

        public class UpdateTopicRequestModel
        {
            public string Title { get; set; }

            public string Content { get; set; }
        }
        public class TopicsResponseModel : BaseResponse
        {
            public IEnumerable<TopicDTO> Data { get; set; } = new List<TopicDTO>();
        }

        public class TopicResponseModel : BaseResponse
        {
            public TopicDTO Data { get; set; }
        }
    }
}
