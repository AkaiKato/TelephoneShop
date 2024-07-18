using DataAccessEF.Data;
using Domain.Interfaces;
using Domain.Interfaces.UoW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessEF.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;

        public UnitOfWork(DataContext context)
        {
            _context = context;
        }

        public ICatalogRepository CatalogRepository { get; private set; }

        public ICitiesRepository CitiesRepository {get; private set; }

        public ICitiesToTelephoneCostRepository CitiesToTelephoneCost {get; private set; }

        public ITelephoneRepository TelephoneRepository { get; private set; }

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
