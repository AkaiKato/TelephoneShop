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
        public IActionResult GetAll()
        {
            var allCities = _unitOfWork.CitiesRepository.GetAll();

            _unitOfWork.Dispose();

            return Ok(allCities);
        }

        [HttpGet("{id}/city")]
        public IActionResult GetById(int id) 
        {
            if(id <= 0)
                return BadRequest();

            if (!_unitOfWork.CitiesRepository.Any(x => x.Id == id))
                return NotFound($"No element with id {id}");

            var city = _unitOfWork.CitiesRepository.Get(id);

            _unitOfWork.Dispose();

            return Ok(city);
        }

        [HttpPost("createCity")]
        public IActionResult CreateCity([FromBody] Cities cityCreate)
        {
            if(cityCreate == null)
                return BadRequest(ModelState);

            if(_unitOfWork.CitiesRepository.Any(x => x.Name.Trim().ToLower() == cityCreate.Name.Trim().ToLower()))
                return BadRequest("Such sity is already created");

            _unitOfWork.CitiesRepository.Add(cityCreate);

            _unitOfWork.Save();

            _unitOfWork.Dispose();

            return Ok("Successfully created");
        }

        [HttpPut("UpdateCity")]
        public IActionResult UpdateCity([FromBody] Cities cityUpdate)
        {
            if (cityUpdate == null)
                return BadRequest(ModelState);

            if (!_unitOfWork.CitiesRepository.Any(x => x.Id == cityUpdate.Id))
                return NotFound("Such sity is not created");

            if (_unitOfWork.CitiesRepository.Any(x => x.Name.Trim().ToLower() == cityUpdate.Name.Trim().ToLower()
                    && x.Id != cityUpdate.Id))
                return BadRequest("Sity with such name already exists");

            var cityToUpdate = _unitOfWork.CitiesRepository.Find(x => x.Id == cityUpdate.Id).First();

            foreach (var item in cityUpdate.GetType().GetProperties())
            {
                var curPropertyUpdateValue = cityToUpdate.GetType().GetProperty(item.Name)!.GetValue(cityUpdate);
                if (!item.GetValue(cityToUpdate)!.Equals(curPropertyUpdateValue))
                {
                    item.SetValue(cityToUpdate, curPropertyUpdateValue);
                }
            }

            _unitOfWork.Save();

            _unitOfWork.Dispose();

            return Ok("Successfully updated");
        }
        
        [HttpDelete("deleteCity")]
        public IActionResult Delete(int id)
        {
            if (id <= 0)
                return BadRequest();

            if (!_unitOfWork.CitiesRepository.Any(x => x.Id == id))
                return NotFound($"No element with id {id}");

            _unitOfWork.CitiesRepository.Remove(_unitOfWork.CitiesRepository.Find(x => x.Id == id).First());

            _unitOfWork.Save();

            _unitOfWork.Dispose();

            return Ok("Successfully deleted");
        }


    }
}
