namespace Chat.Data.Repositories
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Chat.Data.Entities;
    using Microsoft.EntityFrameworkCore;

    public class Repository<T> : IRepository<T> where T : BaseEntity
    {

        #region Settings

        private readonly ChatDbContext _context;
        private DbSet<T> _entity;

        public Repository(ChatDbContext context)
        {
            _context = context;
            _entity = context.Set<T>();
        }

        #endregion       

        public IQueryable<T> Find(Expression<Func<T, bool>> predicate)
        {
            var result = _entity.Where(predicate).AsQueryable();

            return result;
        }

        public T FindOne(Expression<Func<T, bool>> predicate)
        {
            var result = _entity.FirstOrDefault(predicate);

            return result;
        }

        public IQueryable<T> GetAll()
        {
            return _entity.Where(x => !x.IsDeleted).AsQueryable();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            var result = await _entity.FindAsync(id);

            return result == null ? null : result.IsDeleted ? null : result;
        }

        public async Task CreateAsync(T entity)
        {
            entity.CreatedDate = DateTime.UtcNow;
            entity.UpdatedDate = DateTime.UtcNow;
            entity.IsActive = true;
            entity.IsDeleted = false;
            await _entity.AddAsync(entity);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            entity.UpdatedDate = DateTime.UtcNow;
            _entity.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            entity.UpdatedDate = DateTime.UtcNow;
            entity.IsActive = false;
            entity.IsDeleted = true;
            await _context.SaveChangesAsync();
        }
    }
}