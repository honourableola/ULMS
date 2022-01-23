﻿using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domain.Models.ModuleViewModel;

namespace Domain.Interfaces.Services
{
    public interface IModuleService
    {
        public Task<BaseResponse> AddModule(CreateModuleRequestModel model);
        public Task<BaseResponse> UpdateModule(Guid id, UpdateModuleRequestModel model);
        public Task<BaseResponse> DeleteModule(Guid id);
        public Task<ModuleResponseModel> GetModule(Guid id);
        public Task<ModulesResponseModel> GetAllModules();
        public Task<ModulesResponseModel> GetModulesByCourse(Guid courseId);
        public Task<ModulesResponseModel> SearchModulesByName(string searchText);
    }
}
