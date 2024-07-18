using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        T Get(int id);
        IEnumerable<T> GetAll();

        IEnumerable<T> Finde(Expression<Func<T, bool>> expression);

        void Add (T item);

        void Remove (T item);
    }
}
