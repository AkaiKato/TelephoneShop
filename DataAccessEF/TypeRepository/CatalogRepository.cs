using DataAccessEF.Data;
using DataAccessEF.GenericRepository;
using Domain.Interfaces;
using TelephoneShop.Models;

namespace DataAccessEF.TypeRepository
{
    public class CatalogRepository : GenericRepository<Catalog>, ICatalogRepository
    {
        public CatalogRepository(DataContext dataContext) : base(dataContext)
        { }
    }
}
