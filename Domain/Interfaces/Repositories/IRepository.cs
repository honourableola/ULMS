using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repositories
{
    public interface IRepository<T>
    {
        Task<T> GetAsync(Guid id);

        Task<IEnumerable<T>> GetAsync(IList<Guid> ids);

        Task<T> GetAsync(Expression<Func<T, bool>> expression);

        Task<bool> ExistsAsync(Guid id);

        Task<bool> ExistsAsync(Expression<Func<T, bool>> expression);

        Task<T> AddAsync(T entity);

        Task<IEnumerable<T>> AddAsync(IEnumerable<T> entities);

        Task<T> UpdateAsync(T entity);

        Task DeleteAsync(Guid id);

        Task DeleteAsync(T entity);
        IQueryable<T> Query();

        Task<IList<T>> GetAllAsync(Expression<Func<T, bool>> expression);

        Task<int> SaveChangesAsync();

        Task<TEntity> GetAsync<TEntity>(Guid id) where TEntity : BaseEntity;
        Task<TEntity> UpdateAsync<TEntity>(TEntity entity) where TEntity : BaseEntity;
        Task DeleteAsync<TEntity>(Guid id) where TEntity : BaseEntity, new();
        Task DeleteAsync<TEntity>(TEntity entity) where TEntity : BaseEntity;
        Task<TEntity> AddAsync<TEntity>(TEntity entity) where TEntity : BaseEntity;
        Task<IEnumerable<TEntity>> GetAsync<TEntity>(IList<Guid> ids) where TEntity : BaseEntity;
        Task<bool> ExistsAsync<TEntity>(Guid id) where TEntity : BaseEntity;
        Task<bool> ExistsAsync<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : BaseEntity;
        IQueryable<TEntity> Query<TEntity>() where TEntity : BaseEntity;
        IQueryable<TEntity> Query<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : BaseEntity;
    }
}
