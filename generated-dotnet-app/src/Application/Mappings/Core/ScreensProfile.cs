using AutoMapper;
using BookMyShow.Application.DTOs.Core.Screens;
using BookMyShow.Core.Entities.Core;

namespace BookMyShow.Application.Mappings.Core
{
    public class ScreensProfile : Profile
    {
        public ScreensProfile()
        {
            CreateMap<CreateScreenDto, Screen>();
            CreateMap<UpdateScreenDto, Screen>()
                .ForMember(x => x.ScreenId, opt => opt.Ignore());
        }
    }
}
