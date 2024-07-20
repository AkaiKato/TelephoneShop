using Domain.DTO.ReturnTypes;
using Domain.Interfaces.Generic;
using TelephoneShop.Models;

namespace Domain.Interfaces
{
    public interface ITelephoneRepository : IGenericRepository<Telephone>
    {
        Task<List<ReturnTelephoneCityCost>> ReturnCityCostByItemIdAsync(int Id);
    }
}
