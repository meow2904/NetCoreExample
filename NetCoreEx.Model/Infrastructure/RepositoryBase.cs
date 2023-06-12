
using NetCoreEx.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace NetCoreEx.Model.Infrastructure
{
    public interface IRepository<T, in K> where T : class
    {
        IQueryable<T> FindAll(params Expression<Func<T, object>>[] includeProperties);

        IQueryable<T> FindAll(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);

        T FindById(K id);

        Task<T> FindByIdAsync(K id);

        T FindSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);

        Task<T> FindSingleAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);

        T FindFirst(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);

        Task<T> FindFirstAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);

        void Remove(T entity);

        void RemoveMultiple(List<T> entities);

        T Add(T entity);

        void Update(T entity);

        bool CheckContains(Expression<Func<T, bool>> predicate);

        Task<bool> CheckContainsAsync(Expression<Func<T, bool>> predicate);

        int Count(Expression<Func<T, bool>> where);

        Task<int> CountAsync(Expression<Func<T, bool>> where);

        T ExecStore(string sql, params object[] parameters);

        Task<T> ExecStoreAsync(string sql, params object[] parameters);

        Task<int> CountAsync();
    }

    public class RepositoryBase<T, TK> : IRepository<T, TK>, IDisposable where T : class
    {
        private readonly NetCoreExDbContext _dataContext;

        public RepositoryBase(NetCoreExDbContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void Dispose()
        {
            if (_dataContext != null)
            {
                _dataContext.Dispose();
            }
        }

        public IQueryable<T> FindAll(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> items = _dataContext.Set<T>();
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    items = items.Include(includeProperty);
                }
            }
            return items;
        }

        public IQueryable<T> FindAll(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> items = _dataContext.Set<T>();
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    items = items.Include(includeProperty);
                }
            }
            return items.Where(predicate);
        }

        public T FindById(TK id)
        {
            return _dataContext.Set<T>().Find(id);
        }

        public async Task<T> FindByIdAsync(TK id)
        {
            return await _dataContext.Set<T>().FindAsync(id);
        }

        public T FindSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            return FindAll(includeProperties).SingleOrDefault(predicate);
        }

        public async Task<T> FindSingleAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            return await FindAll(includeProperties).SingleOrDefaultAsync(predicate);
        }

        public void Remove(T entity)
        {
            _dataContext.Set<T>().Remove(entity);
        }
        public void RemoveMultiple(List<T> entities)
        {
            _dataContext.Set<T>().RemoveRange(entities);
        }

        public void Update(T entity)
        {
            _dataContext.Set<T>().Update(entity);
        }

        public T Add(T entity)
        {
            return _dataContext.Set<T>().Add(entity).Entity;
        }

        public bool CheckContains(Expression<Func<T, bool>> predicate)
        {
            return _dataContext.Set<T>().Count<T>(predicate) > 0;
        }

        public async Task<bool> CheckContainsAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dataContext.Set<T>().CountAsync<T>(predicate) > 0;
        }

        public T FindFirst(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            return FindAll(includeProperties).FirstOrDefault(predicate);
        }

        public async Task<T> FindFirstAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            return await FindAll(includeProperties).FirstOrDefaultAsync(predicate);
        }

        public int Count(Expression<Func<T, bool>> where)
        {
            return _dataContext.Set<T>().Count(where);
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> where)
        {
            return await _dataContext.Set<T>().CountAsync(where);
        }

        public T ExecStore(string sql, params object[] parameters)
        {
            return _dataContext.Set<T>().FromSqlRaw(sql, parameters).FirstOrDefault();
        }

        public async Task<T> ExecStoreAsync(string sql, params object[] parameters)
        {
            return await _dataContext.Set<T>().FromSqlRaw(sql, parameters).FirstOrDefaultAsync();
        }

        public async Task<int> CountAsync()
        {
            return await _dataContext.Set<T>().CountAsync();
        }
    }
}