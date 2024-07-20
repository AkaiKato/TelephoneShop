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
            IEnumerable<Catalog>? allCatalogs = Enumerable.Empty<Catalog>();


            allCatalogs = await _unitOfWork.CatalogRepository.GetAllAsync(cancellationToken);


            return Ok(allCatalogs);
        }

        [HttpGet("{id}/Catalog")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            Catalog? catalog = null;

            if (id <= 0)
                return BadRequest();

            if (!await _unitOfWork.CatalogRepository.AnyAsync(x => x.Id == id, cancellationToken))
                return NotFound($"No element with id {id}");

            catalog = await _unitOfWork.CatalogRepository.GetAsync(id, cancellationToken);


            return Ok(new GetCatalog { Id = catalog!.Id, Name = catalog.Name, Description = catalog.Description, ParentCatalog = catalog.ParentCatalog });
        }

        [HttpPost("CreateCatalog")]
        public async Task<IActionResult> CreateCityAsync([FromBody] CreateCatalog catalogCreate, CancellationToken cancellationToken)
        {

            if (catalogCreate == null)
                return BadRequest(ModelState);

            if (await _unitOfWork.CatalogRepository.AnyAsync(x => x.Name.Trim().ToLower() == catalogCreate.Name.Trim().ToLower(), cancellationToken))
                return BadRequest("Such catalog is already created");

            Catalog? parentCatalog = null;

            if (catalogCreate.ParentCatalog != null)
            {
                parentCatalog = await _unitOfWork.CatalogRepository.GetAsync(catalogCreate.ParentCatalog.Value, cancellationToken);

                if (parentCatalog == null)
                    return NotFound("No such parent catalog");
            }

            var newCatalog = new Catalog
            {
                Name = catalogCreate.Name,
                Description = catalogCreate.Description,
                ParentCatalog = parentCatalog,
            };

            _unitOfWork.CatalogRepository.Add(newCatalog);

            await _unitOfWork.SaveAsync(cancellationToken);


            return Ok("Successfully created");
        }

        [HttpPut("UpdateCatalog")]
        public async Task<IActionResult> UpdateCatalog([FromBody] UpdateCatalog catalogUpdate, CancellationToken cancellationToken)
        {

            if (catalogUpdate == null)
                return BadRequest(ModelState);

            if (!await _unitOfWork.CatalogRepository.AnyAsync(x => x.Id == catalogUpdate.Id, cancellationToken))
                return NotFound("Such catalog is not created");

            if (await _unitOfWork.CatalogRepository.AnyAsync(x => x.Name.Trim().ToLower() == catalogUpdate.Name.Trim().ToLower()
                    && x.Id != catalogUpdate.Id, cancellationToken))
                return BadRequest("Catalog with such name already exists");

            Catalog? parentCatalog = null;
            if (catalogUpdate.ParentCatalog != null)
                parentCatalog = await _unitOfWork.CatalogRepository.GetAsync(catalogUpdate.ParentCatalog.Value, cancellationToken);

            var catalogToUpdate = await _unitOfWork.CatalogRepository.GetAsync(catalogUpdate.Id, cancellationToken);

            catalogToUpdate!.Name = catalogUpdate.Name;
            catalogToUpdate.Description = catalogUpdate.Description;

            catalogToUpdate.ParentCatalog = parentCatalog;

            _unitOfWork.CatalogRepository.Update(catalogToUpdate);

            await _unitOfWork.SaveAsync(cancellationToken);

            return Ok("Successfully updated");
        }

        [HttpDelete("DeleteCatalog")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            if (id <= 0)
                return BadRequest();

            if (!await _unitOfWork.CatalogRepository.AnyAsync(x => x.Id == id, cancellationToken))
                return NotFound($"No element with id {id}");

            var deletedCity = await _unitOfWork.CatalogRepository.GetAsync(id, cancellationToken);

            _unitOfWork.CatalogRepository.Remove(deletedCity!);

            await _unitOfWork.SaveAsync(cancellationToken);

            return Ok("Successfully deleted");
        }
    }
}
