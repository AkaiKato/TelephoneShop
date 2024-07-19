using Domain.Interfaces.UoW;
using Microsoft.AspNetCore.Mvc;
using TelephoneShop.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TelephoneShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TelephoneController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public TelephoneController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            IEnumerable<Telephone> telephones;

            try
            {
                telephones = await _unitOfWork.TelephoneRepository.GetAllAsync(cancellationToken);
            }
            finally
            {
                _unitOfWork.Dispose();
            }

            return Ok(telephones);
        }

        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
