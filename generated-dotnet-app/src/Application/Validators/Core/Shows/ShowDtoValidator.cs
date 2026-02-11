using BookMyShow.Application.DTOs.Core.Shows;
using FluentValidation;

namespace BookMyShow.Application.Validators.Core.Shows
{
    public class CreateShowDtoValidator : AbstractValidator<CreateShowDto>
    {
        public CreateShowDtoValidator()
        {
            RuleFor(x => x.MovieId).GreaterThan(0);
            RuleFor(x => x.ScreenId).GreaterThan(0);
            RuleFor(x => x.Price).GreaterThanOrEqualTo(0);
            RuleFor(x => x.EndTime).GreaterThan(x => x.StartTime);
        }
    }

    public class UpdateShowDtoValidator : AbstractValidator<UpdateShowDto>
    {
        public UpdateShowDtoValidator()
        {
            RuleFor(x => x.MovieId).GreaterThan(0);
            RuleFor(x => x.ScreenId).GreaterThan(0);
            RuleFor(x => x.Price).GreaterThanOrEqualTo(0);
            RuleFor(x => x.EndTime).GreaterThan(x => x.StartTime);
        }
    }
}
