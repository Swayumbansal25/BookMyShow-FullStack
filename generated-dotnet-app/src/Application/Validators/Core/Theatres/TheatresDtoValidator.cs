using BookMyShow.Application.DTOs.Core.Theatres;
using FluentValidation;

namespace BookMyShow.Application.Validators.Core.Theatres
{
    public class CreateTheatresDtoValidator : AbstractValidator<CreateTheatresDto>
    {
        public CreateTheatresDtoValidator()
        {
            RuleFor(x => x.CityId).NotEmpty();
            RuleFor(x => x.TheatreName).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Address).NotEmpty();
        }
    }

    public class UpdateTheatresDtoValidator : AbstractValidator<UpdateTheatresDto>
    {
        public UpdateTheatresDtoValidator()
        {
            RuleFor(x => x.TheatreId).NotEmpty();
            RuleFor(x => x.CityId).NotEmpty();
            RuleFor(x => x.TheatreName).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Address).NotEmpty();
        }
    }
}
