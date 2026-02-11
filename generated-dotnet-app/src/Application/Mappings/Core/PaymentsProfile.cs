using AutoMapper;
using BookMyShow.Application.DTOs.Core.Payments;
using BookMyShow.Core.Entities.Core;

namespace BookMyShow.Application.Mappings.Core
{
    public class PaymentsProfile : Profile
    {
        public PaymentsProfile()
        {
            // ProcessPaymentDto -> Payment Entity
            CreateMap<ProcessPaymentDto, Payment>();
        }
    }
}