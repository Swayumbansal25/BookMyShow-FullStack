using AutoMapper;
using BookMyShow.Application.DTOs.Core.Theatres;
using BookMyShow.Core.Entities.Core;

namespace BookMyShow.Application.Mappings.Core
{
    public class TheatresProfile : Profile
    {
        public TheatresProfile()
        {
            CreateMap<CreateTheatresDto, Theatres>();
            CreateMap<UpdateTheatresDto, Theatres>()
                .ForMember(x => x.TheatreId, opt => opt.Ignore());
        }
    }
}
