using Domain.DTO.Create;
using Domain.DTO.Get;
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

        [HttpGet("GetAllCities")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            List<GetCity>? allCities = [];

            Thread.Sleep(10000);

            var allCitiesRaw = await _unitOfWork.CitiesRepository.GetAllAsync(cancellationToken);

            foreach (var city in allCitiesRaw)
            {
                allCities.Add(new GetCity
                {
                    Id = city.Id,
                    Name = city.Name,
                });
            }

            return Ok(allCities);
        }

        [HttpGet("{id}/city")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            Cities? city = null;

            if (id <= 0)
                return BadRequest();

            if (!await _unitOfWork.CitiesRepository.AnyAsync(x => x.Id == id, cancellationToken))
                return NotFound($"No element with id {id}");

            city = await _unitOfWork.CitiesRepository.GetAsync(id, cancellationToken);


            return Ok(new GetCity { Id = city!.Id, Name = city.Name });
        }

        [HttpPost("CreateCity")]
        public async Task<IActionResult> CreateCityAsync([FromBody] CreateCity cityCreate, CancellationToken cancellationToken)
        {

            if (cityCreate == null)
                return BadRequest(ModelState);

            if (await _unitOfWork.CitiesRepository.AnyAsync(x => x.Name.Trim().ToLower() == cityCreate.Name.Trim().ToLower(), cancellationToken))
                return BadRequest("Such sity is already created");

            var newCity = new Cities
            {
                Name = cityCreate.Name,
            };

            _unitOfWork.CitiesRepository.Add(newCity);

            await _unitOfWork.SaveAsync(cancellationToken);



            return Ok("Successfully created");
        }

        [HttpPut("UpdateCity")]
        public async Task<IActionResult> UpdateCity([FromBody] Cities cityUpdate, CancellationToken cancellationToken)
        {

            if (cityUpdate == null)
                return BadRequest(ModelState);

            if (!await _unitOfWork.CitiesRepository.AnyAsync(x => x.Id == cityUpdate.Id, cancellationToken))
                return NotFound("Such sity is not created");

            if (await _unitOfWork.CitiesRepository.AnyAsync(x => x.Name.Trim().ToLower() == cityUpdate.Name.Trim().ToLower()
                    && x.Id != cityUpdate.Id, cancellationToken))
                return BadRequest("Sity with such name already exists");

            var cityToUpdate = (await _unitOfWork.CitiesRepository.FindAsync(x => x.Id == cityUpdate.Id, cancellationToken)).First();

            foreach (var item in cityUpdate.GetType().GetProperties())
            {
                var curPropertyUpdateValue = cityToUpdate.GetType().GetProperty(item.Name)!.GetValue(cityUpdate);
                if (!item.GetValue(cityToUpdate)!.Equals(curPropertyUpdateValue))
                {
                    item.SetValue(cityToUpdate, curPropertyUpdateValue);
                }
            }

            await _unitOfWork.SaveAsync(cancellationToken);


            return Ok("Successfully updated");
        }

        [HttpDelete("DeleteCity")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {

            if (id <= 0)
                return BadRequest();

            if (!await _unitOfWork.CitiesRepository.AnyAsync(x => x.Id == id, cancellationToken))
                return NotFound($"No element with id {id}");

            var deletedCity = await _unitOfWork.CitiesRepository.GetAsync(id, cancellationToken);

            _unitOfWork.CitiesRepository.Remove(deletedCity!);

            await _unitOfWork.SaveAsync(cancellationToken);

            return Ok("Successfully deleted");
        }


    }
}
