using DataAccessEF.Data;
using Domain.Interfaces.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
namespace DataAccessEF.GenericRepository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly DataContext _context;

        public GenericRepository(DataContext dataContext)
        {
            _context = dataContext;
        }

        public async Task<T?> GetAsync(int id, CancellationToken t)
        {
            return await _context.Set<T>().FindAsync(id, t);
        }

        public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Set<T>().ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken)
        {
            return await _context.Set<T>().Where(expression).ToListAsync(cancellationToken);
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken)
        {
            return await _context.Set<T>().AnyAsync(expression, cancellationToken);
        }

        public void Add (T item)
        {
            _context.Set<T>().Add(item);
        }

        public void Update(T item)
        {
            _context.Entry(item).State = EntityState.Modified;
            _context.Set<T>().Update(item);
        }

        public void Remove(T item)
        {
            _context.Set<T>().Remove(item);
        }
    }
}
