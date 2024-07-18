using DataAccessEF.Data;
using DataAccessEF.GenericRepository;
using Domain.Interfaces;
using TelephoneShop.Models;

namespace DataAccessEF.TypeRepository
{
    public class CitiesRepository : GenericRepository<Cities>, ICitiesRepository
    {
        public CitiesRepository(DataContext dataContext) : base(dataContext)
        { }
    }
}
