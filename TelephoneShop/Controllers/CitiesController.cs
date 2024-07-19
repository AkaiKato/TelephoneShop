using AutoMapper;
using Domain.DTO.Create;
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
        private readonly IMapper _mapper;

        public CitiesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("GetAllCities")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            IEnumerable<Cities> allCities;

            try
            {
                allCities = await _unitOfWork.CitiesRepository.GetAllAsync(cancellationToken);
            }
            finally
            {
                _unitOfWork.Dispose();
            }

            return Ok(allCities);
        }

        [HttpGet("{id}/city")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken) 
        {
            Cities? city;
            try
            {
                if (id <= 0)
                    return BadRequest();

                if (!await _unitOfWork.CitiesRepository.AnyAsync(x => x.Id == id, cancellationToken))
                    return NotFound($"No element with id {id}");

                city = await _unitOfWork.CitiesRepository.GetAsync(id, cancellationToken);
            }
            finally
            {
                _unitOfWork.Dispose();
            }

            return Ok(city);
        }

        [HttpPost("CreateCity")]
        public async Task<IActionResult> CreateCityAsync([FromBody] CreateCity cityCreate, CancellationToken cancellationToken)
        {
            try
            {
                if (cityCreate == null)
                    return BadRequest(ModelState);

                if (await _unitOfWork.CitiesRepository.AnyAsync(x => x.Name.Trim().ToLower() == cityCreate.Name.Trim().ToLower(), cancellationToken))
                    return BadRequest("Such sity is already created");

                var cityMaped = _mapper.Map<Cities>(cityCreate);

                _unitOfWork.CitiesRepository.Add(cityMaped);

                await _unitOfWork.SaveAsync(cancellationToken);
            }
            finally
            {
                _unitOfWork.Dispose();
            }

            return Ok("Successfully created");
        }

        [HttpPut("UpdateCity")]
        public async Task<IActionResult> UpdateCity([FromBody] Cities cityUpdate, CancellationToken cancellationToken)
        {
            try
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
            }
            finally
            {
                _unitOfWork.Dispose();
            }

            return Ok("Successfully updated");
        }
        
        [HttpDelete("DeleteCity")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            try
            {
                if (id <= 0)
                    return BadRequest();

                if (!await _unitOfWork.CitiesRepository.AnyAsync(x => x.Id == id, cancellationToken))
                    return NotFound($"No element with id {id}");

                var deletedCity = await _unitOfWork.CitiesRepository.GetAsync(id, cancellationToken);

                _unitOfWork.CitiesRepository.Remove(deletedCity!);

                await _unitOfWork.SaveAsync(cancellationToken);
            }
            finally
            {
                _unitOfWork.Dispose();
            }

            return Ok("Successfully deleted");
        }


    }
}
