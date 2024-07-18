using System.Linq.Expressions;

namespace Domain.Interfaces.Generic
{
    public interface IGenericRepository<T> where T : class
    {
        T? Get(int id);
        IEnumerable<T> GetAll();

        IEnumerable<T> Find(Expression<Func<T, bool>> expression);

        bool Any(Expression<Func<T, bool>> expression);

        void Add(T item);

        void Update(T item);

        void Remove(T item);

    }
}
