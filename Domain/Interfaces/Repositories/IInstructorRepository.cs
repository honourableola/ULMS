using Domain.DTOs;
using Domain.Entities;
using Domain.Paging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repositories
{
    public interface IInstructorRepository : IRepository<Instructor>
    {
        Task<IEnumerable<Instructor>> GetSelectedInstructors(IList<Guid> ids);
        Task<IList<Instructor>> GetInstructorsByCourse(Guid courseId);
        public Task<IEnumerable<Instructor>> SearchInstructorsByName(string searchText);
        public Task<PaginatedList<InstructorDTO>> LoadInstructorsAsync(string filter, int page, int limit);
        //public Task<List<Instructor>> NewLoadInstructorsAsync();
    }
}
