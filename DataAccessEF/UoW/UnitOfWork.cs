using DataAccessEF.Data;
using DataAccessEF.TypeRepository;
using Domain.Interfaces;
using Domain.Interfaces.UoW;

namespace DataAccessEF.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;

        public ICatalogRepository CatalogRepository { get; private set; }

        public ICitiesRepository CitiesRepository { get; private set; }

        public ICitiesToTelephoneCostRepository CitiesToTelephoneCost { get; private set; }

        public ITelephoneRepository TelephoneRepository { get; private set; }

        public UnitOfWork(DataContext context)
        {
            _context = context;
            CatalogRepository = new CatalogRepository(_context);
            CitiesRepository = new CitiesRepository(_context);
            CitiesToTelephoneCost = new CitiesToTelephoneCostRepository(_context);
            TelephoneRepository = new TelephoneRepository(_context);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
