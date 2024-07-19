using System.Linq.Expressions;

namespace Domain.Interfaces.Generic
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetAsync(int id, CancellationToken cancellationToken);
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken);

        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken);

        Task<bool> AnyAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken);

        void Add (T item);

        void Update (T item);

        void Remove(T item);

    }
}
