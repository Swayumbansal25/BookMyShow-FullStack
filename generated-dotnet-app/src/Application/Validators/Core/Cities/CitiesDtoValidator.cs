using BookMyShow.Application.DTOs.Core.Cities;
using FluentValidation;

namespace BookMyShow.Application.Validators.Core.Cities
{
    public class CreateCitiesDtoValidator : AbstractValidator<CreateCitiesDto>
    {
        public CreateCitiesDtoValidator()
        {
            RuleFor(x => x.StateId).NotEmpty();
            RuleFor(x => x.CityName).NotEmpty().MaximumLength(150);
        }
    }

    public class UpdateCitiesDtoValidator : AbstractValidator<UpdateCitiesDto>
    {
        public UpdateCitiesDtoValidator()
        {
            RuleFor(x => x.CityId).NotEmpty();
            RuleFor(x => x.StateId).NotEmpty();
            RuleFor(x => x.CityName).NotEmpty().MaximumLength(150);
        }
    }
}
