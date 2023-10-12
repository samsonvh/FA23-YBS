
using System.Linq.Expressions;
using YBS.Data.Context;

namespace YBS.Data.Repositories.Implements
{
    public class GenericRepository<T> : IGenericRepositoty<T> where T : class
    {
        private readonly YBSContext _context;
        public GenericRepository(YBSContext context)
        {
            _context = context;
        }
        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public void AddRange(IEnumerable<T> entities)
        {
            _context.Set<T>().AddRange(entities);
        }
        public void Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }

        public void UpdateRange(IEnumerable<T> entities)
        {
            _context.Set<T>().UpdateRange(entities);
        }
        public IQueryable<T> Find(Expression<Func<T, bool>> expression)
        {
            return _context.Set<T>().Where(expression);
        }

        public IQueryable<T> GetAll()
        {
            return _context.Set<T>();
        }

        public async Task<T> GetByID(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }
    }
}