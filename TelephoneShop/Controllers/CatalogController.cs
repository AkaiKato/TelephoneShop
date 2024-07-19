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

            try
            {
                allCatalogs = await _unitOfWork.CatalogRepository.GetAllAsync(cancellationToken);
            }
            catch (Exception ex)
            {

            }

            return Ok(allCatalogs);
        }

        [HttpGet("{id}/Catalog")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            Catalog? catalog = null;
            try
            {
                if (id <= 0)
                    return BadRequest();

                if (!await _unitOfWork.CatalogRepository.AnyAsync(x => x.Id == id, cancellationToken))
                    return NotFound($"No element with id {id}");

                catalog = await _unitOfWork.CatalogRepository.GetAsync(id, cancellationToken);
            }
            catch (Exception ex)
            {

            }

            return Ok(new GetCatalog { Id = catalog!.Id, Name = catalog.Name, Description = catalog.Description, ParentCatalog = catalog.ParentCatalog});
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
            }
            catch (Exception ex)
            {

            }

            return Ok("Successfully created");
        }

        [HttpPut("UpdateCatalog")]
        public async Task<IActionResult> UpdateCatalog([FromBody] UpdateCatalog catalogUpdate, CancellationToken cancellationToken)
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

                var ttt = await _unitOfWork.CatalogRepository.FindAsync(x => x.Id == catalogUpdate.ParentCatalog, cancellationToken);
                var catalogToUpdate = await _unitOfWork.CatalogRepository.GetAsync(catalogUpdate.Id, cancellationToken);

                catalogToUpdate!.Name = catalogUpdate.Name;
                catalogToUpdate.Description = catalogUpdate.Description;
                
                catalogToUpdate.ParentCatalog = ttt.FirstOrDefault();
                //catalogToUpdate.ParentCatalog = catalogUpdate.ParentCatalog == null ? null : await _unitOfWork.CatalogRepository.GetAsync(catalogUpdate.ParentCatalog!.Value, cancellationToken);

                _unitOfWork.CatalogRepository.Update(catalogToUpdate);

                await _unitOfWork.SaveAsync(cancellationToken);
            }
            catch (Exception ex)
            {

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
            catch (Exception ex)
            {

            }

            return Ok("Successfully deleted");
        }
    }
}
