using Domain.DTOs;
using System;
using System.Collections.Generic;

namespace Domain.Models
{
    public class ModuleViewModel
    {
        public class CreateModuleRequestModel
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string Content { get; set; }
            public Guid CourseId { get; set; }
            public string ModuleImage1 { get; set; }
            public string ModuleImage2 { get; set; }
            public string ModulePDF1 { get; set; }
            public string ModulePDF2 { get; set; }
            public string ModuleVideo1 { get; set; }
            public string ModuleVideo2 { get; set; }

        }

        public class UpdateModuleRequestModel
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string Content { get; set; }
            public string ModuleImage1 { get; set; }
            public string ModuleImage2 { get; set; }
            public string ModulePDF1 { get; set; }
            public string ModulePDF2 { get; set; }
            public string ModuleVideo1 { get; set; }
            public string ModuleVideo2 { get; set; }
        }
        public class ModulesResponseModel : BaseResponse
        {
            public IEnumerable<ModuleDTO> Data { get; set; } = new List<ModuleDTO>();
        }

        public class ModuleResponseModel : BaseResponse
        {
            public ModuleDTO Data { get; set; }
        }
    }
}
