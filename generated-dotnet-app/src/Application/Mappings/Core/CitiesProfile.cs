using AutoMapper;
using BookMyShow.Application.DTOs.Core.Cities;
using BookMyShow.Core.Entities.Core;

namespace BookMyShow.Application.Mappings.Core
{
    public class CitiesProfile : Profile
    {
        public CitiesProfile()
        {
            CreateMap<CreateCitiesDto, Cities>();
            CreateMap<UpdateCitiesDto, Cities>()
                .ForMember(x => x.CityId, opt => opt.Ignore());
        }
    }
}
