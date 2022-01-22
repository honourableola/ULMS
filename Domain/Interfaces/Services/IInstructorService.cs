using Domain.Entities;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domain.Models.InstructorViewModel;

namespace Domain.Interfaces.Services
{
    public interface IInstructorService
    {
        public Task<BaseResponse> AddInstructor(CreateInstructorRequestModel model);
        public Task<BaseResponse> UpdateInstructor(Guid id, UpdateInstructorRequestModel model);
        public Task<BaseResponse> DeleteInstructor(Guid id);
        public Task<InstructorResponseModel> GetInstructorById(Guid id);
        public Task<InstructorResponseModel> GetInstructorByEmail(string email);
        public Task<IEnumerable<Instructor>> SearchInstructorsByName(string searchText);
        public Task<InstructorsResponseModel> GetInstructorsByCourse(Guid courseId);
        public Task<InstructorsResponseModel> GetAllInstructors();
    }
}
