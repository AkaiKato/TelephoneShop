using Domain.DTO.Create;
using Domain.Interfaces.UoW;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TelephoneShop.Models;

namespace TelephoneShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CatalogController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAllCatalogs")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            IEnumerable<Catalog> allCatalogs;

            try
            {
                allCatalogs = await _unitOfWork.CatalogRepository.GetAllAsync(cancellationToken);
            }
            finally
            {
                _unitOfWork.Dispose();
            }

            return Ok(allCatalogs);
        }

        [HttpGet("{id}/Catalog")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            Catalog? catalog;
            try
            {
                if (id <= 0)
                    return BadRequest();

                if (!await _unitOfWork.CatalogRepository.AnyAsync(x => x.Id == id, cancellationToken))
                    return NotFound($"No element with id {id}");

                catalog = await _unitOfWork.CatalogRepository.GetAsync(id, cancellationToken);
            }
            finally
            {
                _unitOfWork.Dispose();
            }

            return Ok(catalog);
        }

        [HttpPost("CreateCatalog")]
        public async Task<IActionResult> CreateCityAsync([FromBody] CreateCatalog catalogCreate, CancellationToken cancellationToken)
        {
            try
            {
                if (catalogCreate == null)
                    return BadRequest(ModelState);

                if (await _unitOfWork.CatalogRepository.AnyAsync(x => x.Name.Trim().ToLower() == catalogCreate.Name.Trim().ToLower(), cancellationToken))
                    return BadRequest("Such catalog is already created");

                if(catalogCreate.ParentCatalog != null && await _unitOfWork.CatalogRepository.FindAsync(x => x.Id == catalogCreate.ParentCatalog, cancellationToken) == null)
                    return NotFound("No such parent catalog");

                var newCatalog = new Catalog
                {
                    Name = catalogCreate.Name,
                    Description = catalogCreate.Description,
                };

                _unitOfWork.CatalogRepository.Add(newCatalog);

                await _unitOfWork.SaveAsync(cancellationToken);
            }
            finally
            {
                _unitOfWork.Dispose();
            }

            return Ok("Successfully created");
        }

        [HttpPut("UpdateCatalog")]
        public async Task<IActionResult> UpdateCity([FromBody] Catalog catalogUpdate, CancellationToken cancellationToken)
        {
            try
            {
                if (catalogUpdate == null)
                    return BadRequest(ModelState);

                if (!await _unitOfWork.CatalogRepository.AnyAsync(x => x.Id == catalogUpdate.Id, cancellationToken))
                    return NotFound("Such catalog is not created");

                if (await _unitOfWork.CatalogRepository.AnyAsync(x => x.Name.Trim().ToLower() == catalogUpdate.Name.Trim().ToLower()
                        && x.Id != catalogUpdate.Id, cancellationToken))
                    return BadRequest("Catalog with such name already exists");

                var cityToUpdate = (await _unitOfWork.CatalogRepository.FindAsync(x => x.Id == catalogUpdate.Id, cancellationToken)).First();

                foreach (var item in catalogUpdate.GetType().GetProperties())
                {
                    var curPropertyUpdateValue = cityToUpdate.GetType().GetProperty(item.Name)!.GetValue(catalogUpdate);
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

        [HttpDelete("DeleteCatalog")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            try
            {
                if (id <= 0)
                    return BadRequest();

                if (!await _unitOfWork.CatalogRepository.AnyAsync(x => x.Id == id, cancellationToken))
                    return NotFound($"No element with id {id}");

                var deletedCity = await _unitOfWork.CatalogRepository.GetAsync(id, cancellationToken);

                _unitOfWork.CatalogRepository.Remove(deletedCity!);

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
