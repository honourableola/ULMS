using Domain.Entities;
using Domain.Models;
using System.Threading.Tasks;
using static Domain.Models.CourseConstantViewModel;

namespace Domain.Interfaces.Services
{
    public interface ICourseConstantService
    {
        public Task<BaseResponse> AddCourseConstant(CreateCourseConstantRequestModel model);
        public Task<BaseResponse> UpdateCourseConstant(UpdateCourseConstantRequestModel model);
        public CourseConstant GetCourseConstant();
    }
}
