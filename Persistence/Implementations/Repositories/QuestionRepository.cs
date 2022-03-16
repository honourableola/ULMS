using Domain.Entities;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var questions = _context.Questions.Include(q => q.Options)
                .Where(a => a.ModuleId == moduleId).ToList();
            return questions;
           
        }

        public Question GetQuestionsById(Guid id)
        {
            var question = _context.Questions.Include(q => q.Options)
                .SingleOrDefault(a => a.Id == id);
            return question;

        }

        public async Task<List<Question>> GetQuestionsByModuleAsync(Guid moduleId)
        {
            var questions = await _context.Questions.Include(q => q.Options).Where(a => a.ModuleId == moduleId).ToListAsync();
            return questions;
        }
    }
}
