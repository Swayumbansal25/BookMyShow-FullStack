using AutoMapper;
using BookMyShow.Application.DTOs.Core.States;
using BookMyShow.Core.Entities.Core;

namespace BookMyShow.Application.Mappings.Core
{
    public class StatesProfile : Profile
    {
        public StatesProfile()
        {
            CreateMap<CreateStatesDto, States>();
            CreateMap<UpdateStatesDto, States>()
                .ForMember(dest => dest.StateId, opt => opt.Ignore());
        }
    }
}
