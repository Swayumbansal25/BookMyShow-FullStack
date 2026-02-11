using AutoMapper;
using BookMyShow.Application.DTOs.Core.Bookings;
using BookMyShow.Core.Entities.Core;

namespace BookMyShow.Application.Mappings.Core
{
    public class BookingsProfile : Profile
    {
        public BookingsProfile()
        {
            // CreateBookingDto -> Booking
            // Note: TotalAmount and Status are handled by the Service logic
            CreateMap<CreateBookingDto, Booking>();
        }
    }
}