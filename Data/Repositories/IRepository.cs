using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using chat.Data.Entities;

namespace chat.Data.Repositories
{
    public interface IRepository<T> where T : IBaseEntity
    {
        IQueryable<T> Find(Expression<Func<T, bool>> includes);
        T FindOne(Expression<Func<T, bool>> specification);
        IQueryable<T> GetAll();
        Task<T> GetByIdAsync(int id);
        Task CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}