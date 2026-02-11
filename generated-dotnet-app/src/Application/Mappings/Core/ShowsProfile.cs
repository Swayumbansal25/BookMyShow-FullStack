using AutoMapper;
using BookMyShow.Application.DTOs.Core.Shows;
using BookMyShow.Core.Entities.Core;

namespace BookMyShow.Application.Mappings.Core
{
    public class ShowsProfile : Profile
    {
        public ShowsProfile()
        {
            CreateMap<CreateShowDto, Show>();
            CreateMap<UpdateShowDto, Show>();
        }
    }
}
