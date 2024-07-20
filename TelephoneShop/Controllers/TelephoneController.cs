using Domain.DTO.Create;
using Domain.DTO.Get;
using Domain.DTO.Update;
using Domain.Interfaces.UoW;
using Microsoft.AspNetCore.Mvc;
using TelephoneShop.Models;

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
            List<GetTelephone> getTelephones = [];

            var telephones = await _unitOfWork.TelephoneRepository.GetAllAsync(cancellationToken);

            if (telephones == null)
                return Ok(getTelephones);

            foreach (var item in telephones)
            {
                List<GetCityCost> costs = [];
                if (item.CitiesToTelephoneCost != null)
                {
                    foreach (var cityCost in item.CitiesToTelephoneCost)
                    {
                        costs.Add(
                            new GetCityCost
                            {
                                CityId = cityCost.City.Id,
                                City = cityCost.City.Name,
                                Cost = cityCost.Cost,
                            }
                        );
                    }
                }

                getTelephones.Add(
                    new GetTelephone
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Description = item.Description,
                        CatalogId = item.Catalog.Id,
                        CatalogName = item.Catalog.Name,
                        CityCost = costs,
                    }
                );
            }


            return Ok(getTelephones);
        }

        [HttpGet("{id}/Telephone")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            GetTelephone? telephone = null;

            if (id <= 0)
                return BadRequest();

            if (!await _unitOfWork.TelephoneRepository.AnyAsync(x => x.Id == id, cancellationToken))
                return NotFound($"No element with id {id}");

            var telephoneRaw = await _unitOfWork.TelephoneRepository.GetAsync(id, cancellationToken);

            List<GetCityCost> costs = [];

            if (telephoneRaw!.CitiesToTelephoneCost != null)
            {
                foreach (var cityCost in telephoneRaw.CitiesToTelephoneCost)
                {
                    costs.Add(
                        new GetCityCost
                        {
                            CityId = cityCost.City.Id,
                            City = cityCost.City.Name,
                            Cost = cityCost.Cost,
                        }
                    );
                }
            }


            telephone = new GetTelephone
            {
                Id = telephoneRaw!.Id,
                Name = telephoneRaw.Name,
                Description = telephoneRaw.Description,
                CatalogId = telephoneRaw.Catalog.Id,
                CatalogName = telephoneRaw.Catalog.Name,
                CityCost = costs,
            };


            return Ok(telephone);
        }

        [HttpPost("CreateTelephone")]
        public async Task<IActionResult> CreateTelephoneAsync([FromBody] CreateTelephone createTelephone, CancellationToken cancellationToken)
        {

            if (createTelephone == null)
                return BadRequest(ModelState);

            if (await _unitOfWork.TelephoneRepository.AnyAsync(x => x.Name.Trim().ToLower() == createTelephone.Name.Trim().ToLower(), cancellationToken))
                return BadRequest("Such Telephone is already created");

            if (!await _unitOfWork.CatalogRepository.AnyAsync(x => x.Id == createTelephone.Catalog, cancellationToken))
            {
                return NotFound("Not");
            }

            var catalog = await _unitOfWork.CatalogRepository.GetAsync(createTelephone.Catalog, cancellationToken);

            var newTelephone = new Telephone
            {
                Name = createTelephone.Name,
                Description = createTelephone.Description,
                Catalog = catalog!,
            };

            var cities = new List<CitiesToTelephoneCost>();

            foreach (var item in createTelephone.CTTCost)
            {
                var city = await _unitOfWork.CitiesRepository.GetAsync(item.City, cancellationToken);
                if (city == null)
                {
                    return NotFound("nope");
                }
                cities.Add(new CitiesToTelephoneCost { City = city, Cost = item.Cost, Telephone = newTelephone });
            }

            foreach (var item in cities)
            {
                _unitOfWork.CitiesToTelephoneCost.Add(item);
            }

            newTelephone.CitiesToTelephoneCost = cities;

            _unitOfWork.TelephoneRepository.Add(newTelephone);

            await _unitOfWork.SaveAsync(cancellationToken);


            return Ok("Successfully created");
        }

        [HttpPut("UpdateTelephone")]
        public async Task<IActionResult> UpdateTelephone([FromBody] UpdateTelephone telephoneUpdate, CancellationToken cancellationToken)
        {

            if (telephoneUpdate == null)
                return BadRequest(ModelState);

            if (!await _unitOfWork.TelephoneRepository.AnyAsync(x => x.Id == telephoneUpdate.Id, cancellationToken))
                return NotFound("Such Telephone is not created");

            if (await _unitOfWork.TelephoneRepository.AnyAsync(x => x.Name.Trim().ToLower() == telephoneUpdate.Name.Trim().ToLower()
                    && x.Id != telephoneUpdate.Id, cancellationToken))
                return BadRequest("Telephone with such name already exists");

            var telephoneToUpdate = await _unitOfWork.TelephoneRepository.GetAsync(telephoneUpdate.Id, cancellationToken);

            telephoneToUpdate!.Name = telephoneUpdate.Name;
            telephoneToUpdate.Description = telephoneUpdate.Description;

            if (telephoneToUpdate.Catalog.Id != telephoneUpdate.CatalogId)
            {
                var catalog = await _unitOfWork.CatalogRepository.GetAsync(telephoneUpdate.CatalogId, cancellationToken);
                if (catalog == null)
                {
                    return NotFound("No such catalog");
                }
                telephoneToUpdate.Catalog = catalog;
            }

            if (telephoneToUpdate.CitiesToTelephoneCost != null)
            {
                var tt = await _unitOfWork.TelephoneRepository.ReturnCityCostByItemIdAsync(telephoneUpdate.Id);
                foreach (var item in telephoneUpdate.CityCost)
                {
                    if (!tt.Any(x => x.Telephone == telephoneUpdate.Id))
                        return NotFound("No such CityCost");

                    telephoneToUpdate.CitiesToTelephoneCost!.Find(x => x.City.Id == item!.CityId)!.Cost = item!.Cost;
                }
            }

            _unitOfWork.TelephoneRepository.Update(telephoneToUpdate);

            await _unitOfWork.SaveAsync(cancellationToken);

            return Ok("Successfully updated");
        }

        [HttpDelete("DeleteTelephone")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            if (id <= 0)
                return BadRequest();

            if (!await _unitOfWork.TelephoneRepository.AnyAsync(x => x.Id == id, cancellationToken))
                return NotFound($"No element with id {id}");

            var deletedTelephone = await _unitOfWork.TelephoneRepository.GetAsync(id, cancellationToken);

            _unitOfWork.TelephoneRepository.Remove(deletedTelephone!);

            await _unitOfWork.SaveAsync(cancellationToken);

            return Ok("Successfully deleted");
        }    
    }
}
