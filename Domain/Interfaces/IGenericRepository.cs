using System.Linq.Expressions;

namespace Domain.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        T Get(int id);
        IEnumerable<T> GetAll();

        IEnumerable<T> Finde(Expression<Func<T, bool>> expression);

        void Add(T item);

        void Remove(T item);
    }
}
