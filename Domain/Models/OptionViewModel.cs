using Domain.DTOs;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class OptionViewModel
    {
        public class CreateOptionRequestModel
        {
            public string Label { get; set; }
            public string OptionText { get; set; }
            public OptionStatus Status { get; set; }
            public Guid QuestionId { get; set; }

        }

        public class UpdateOptionRequestModel
        {
            public string Label { get; set; }
            public string OptionText { get; set; }
            public OptionStatus Status { get; set; }
        }
        public class OptionsResponseModel : BaseResponse
        {
            public IEnumerable<OptionDTO> Data { get; set; } = new List<OptionDTO>();
        }

        public class OptionResponseModel : BaseResponse
        {
            public OptionDTO Data { get; set; }
        }
    }
}
