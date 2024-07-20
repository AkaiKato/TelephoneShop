using DataAccessEF.Data;
using DataAccessEF.GenericRepository;
using Domain.DTO.ReturnTypes;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using TelephoneShop.Models;

namespace DataAccessEF.TypeRepository
{
    public class TelephoneRepository : GenericRepository<Telephone>, ITelephoneRepository
    {
        public TelephoneRepository(DataContext dataContext) : base(dataContext)
        { }


        public async Task<List<ReturnTelephoneCityCost>> ReturnCityCostByItemIdAsync(int Id)
        {
            var results = await _context.Telephone.Join(_context.CitiesToTelephoneCost, x => x.Id, y => y.Telephone.Id, (x, y) => new ReturnTelephoneCityCost
            {
                Telephone = x.Id,
                City = y.City.Id,
                Cost = y.Cost,
            }).Where(x => x.Telephone == Id).ToListAsync();

            return results;
        }
    }
}
