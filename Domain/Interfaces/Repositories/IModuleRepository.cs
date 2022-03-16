using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repositories
{
    public interface IModuleRepository : IRepository<Module>
    {
        Task<IEnumerable<Module>> SearchModuleByName(string searchText);
    }
}
