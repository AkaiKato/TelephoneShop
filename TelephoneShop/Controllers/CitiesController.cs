using Domain.Interfaces.UoW;
using Microsoft.AspNetCore.Mvc;
using TelephoneShop.Models;

namespace TelephoneShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CitiesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IEnumerable<Cities> GetAll()
        {
            return _unitOfWork.CitiesRepository.GetAll();
        }
    }
}
