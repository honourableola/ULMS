using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repositories
{
    public interface ITopicRepository : IRepository<Topic>
    {
        Task<IEnumerable<Topic>> SearchTopicsByTitle(string searchText);
        List<Topic> GetTopicsByModule(Guid moduleId);
    }
}
