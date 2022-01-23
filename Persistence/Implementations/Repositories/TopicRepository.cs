using Domain.Entities;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Implementations.Repositories
{
    public class TopicRepository : BaseRepository<Topic>, ITopicRepository
    {
        public TopicRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Topic>> SearchTopicsByTitle(string searchText)
        {    
            return await _context.Topics.Include(o => o.Module).Where(topic => EF.Functions.Like(topic.Title, $"%{searchText}%")).ToListAsync();
       
        }
    }
}
