using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domain.Models.AdminViewModel;

namespace Domain.Interfaces.Services
{
    public interface IAdminService
    {
        public Task<BaseResponse> AddAdmin(CreateAdminRequestModel model);
        public Task<BaseResponse> UpdateAdmin(Guid id, UpdateAdminRequestModel model);
        public Task<BaseResponse> DeleteAdmin(Guid id);
        public Task<AdminResponseModel> GetAdmin(Guid id);
        public Task<AdminsResponseModel> GetAllAdmins();
        public Task<BaseResponse> UploadPhoto(Guid adminId, UpdateAdminPhotoRequestModel model);
    }
}
