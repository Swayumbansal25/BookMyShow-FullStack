using AutoMapper;
using BookMyShow.Application.DTOs.Core.Seats;
using BookMyShow.Core.Entities.Core;

namespace BookMyShow.Application.Mappings.Core
{
    public class SeatsProfile : Profile
    {
        public SeatsProfile()
        {
            // CreateSeatDto -> Seat
            CreateMap<CreateSeatDto, Seat>();

            // UpdateSeatDto -> Seat
            CreateMap<UpdateSeatDto, Seat>()
                .ForMember(dest => dest.SeatId, opt => opt.Ignore()); // Usually ID is passed in the URL
        }
    }
}