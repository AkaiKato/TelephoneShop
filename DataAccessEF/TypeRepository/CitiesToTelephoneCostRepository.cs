using DataAccessEF.Data;
using DataAccessEF.GenericRepository;
using Domain.Interfaces;
using TelephoneShop.Models;

namespace DataAccessEF.TypeRepository
{
    public class CitiesToTelephoneCostRepository : GenericRepository<CitiesToTelephoneCost>, ICitiesToTelephoneCostRepository
    {
        public CitiesToTelephoneCostRepository(DataContext dataContext) : base(dataContext)
        { }

    }
}
