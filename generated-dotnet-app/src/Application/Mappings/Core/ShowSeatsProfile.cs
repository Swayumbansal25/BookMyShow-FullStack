using AutoMapper;
using BookMyShow.Core.Entities.Core;

namespace BookMyShow.Application.Mappings.Core
{
    public class ShowSeatsProfile : Profile
    {
        public ShowSeatsProfile()
        {
            // Currently, we map the entity directly, but this profile 
            // is ready for when you add ShowSeatResponseDto.
            CreateMap<ShowSeat, ShowSeat>(); 
        }
    }
}