using AutoMapper;
using Domain.DTO.Create;
using TelephoneShop.Models;

namespace Domain.DTO
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() 
        {
            CreateMap<CreateCity, Cities>();
            CreateMap<CreateCatalog, Catalog>();
        }
    }
}
