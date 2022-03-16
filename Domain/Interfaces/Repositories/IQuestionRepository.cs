using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repositories
{
    public interface IQuestionRepository : IRepository<Question>
    {
        public List<Question> GetQuestionsByModule(Guid moduleId);
        public Question GetQuestionsById(Guid id);
        public Task<List<Question>> GetQuestionsByModuleAsync(Guid moduleId);
    }
}
