﻿using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repositories
{
    public interface IInstructorRepository : IRepository<Instructor>
    {
        Task<IEnumerable<Instructor>> GetSelectedInstructors(IList<Guid> ids);
        Task<IList<Instructor>> GetInstructorsByCourse(Guid courseId);
        public Task<IEnumerable<Instructor>> SearchInstructorsByName(string searchText);
    }
}