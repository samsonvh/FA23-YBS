using System.Linq.Expressions;
using YBS.Data;
using YBS.Data.Context;
using YBS.Repository.Interfacces;

namespace YBS.Repository.Implements
{
    public class GenericRepositorycs<T> : IGenericRepository<T> where T : class
    {
        protected readonly YBSContext _context;
        public GenericRepositorycs(YBSContext context)
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
        public IQueryable<T> Find(Expression<Func<T, bool>> expression)
        {
            return _context.Set<T>().Where(expression);
        }
        public IQueryable<T> GetAll()
        {
            return _context.Set<T>();
        }
        public T GetById(int id)
        {
            return _context.Set<T>().Find(id);
        }
        public void Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
        }
        public void RemoveRange(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }
        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }
        public void UpdateRange(IEnumerable<T> entities)
        {
            _context.Set<T>().UpdateRange(entities);
        }
    }
}
