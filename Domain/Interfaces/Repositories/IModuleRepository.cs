using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repositories
{
    public interface IModuleRepository : IRepository<Module>
    {
        Task<IEnumerable<Module>> SearchModuleByName(string searchText);
    }
}
