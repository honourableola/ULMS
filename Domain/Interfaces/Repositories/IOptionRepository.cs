using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repositories
{
    public interface IOptionRepository : IRepository<Option>
    {
        IEnumerable<Option> GetSelectedOptions(IList<Guid> ids);
    }
}
