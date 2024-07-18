using DataAccessEF.Data;
using DataAccessEF.GenericRepository;
using Domain.Interfaces;
using TelephoneShop.Models;

namespace DataAccessEF.TypeRepository
{
    public class TelephoneRepository : GenericRepository<Telephone>, ITelephoneRepository
    {
        public TelephoneRepository(DataContext dataContext) : base(dataContext)
        { }
    }
}
