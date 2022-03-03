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
    public class QuestionRepository : BaseRepository<Question>, IQuestionRepository
    {
        public QuestionRepository(ApplicationContext context)
        {
            _context = context;
        }
        public List<Question> GetQuestionsByModule(Guid moduleId)
        {
            var questions = _context.Questions
                .Where(a => a.ModuleId == moduleId).ToList();
            return questions;
        }

    }
}
