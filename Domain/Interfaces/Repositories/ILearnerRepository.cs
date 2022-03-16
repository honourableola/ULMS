using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repositories
{
    public interface ILearnerRepository : IRepository<Learner>
    {
        Task<IEnumerable<Learner>> GetSelectedLearners(IList<Guid> ids);
        Task<IList<Learner>> GetLearnersByCourse(Guid courseId);
        Task<IEnumerable<Learner>> SearchLearnersByName(string searchText);
    }
}
