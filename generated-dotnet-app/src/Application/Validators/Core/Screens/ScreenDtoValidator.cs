using BookMyShow.Application.DTOs.Core.Screens;
using FluentValidation;

namespace BookMyShow.Application.Validators.Core.Screens
{
    public class CreateScreenDtoValidator : AbstractValidator<CreateScreenDto>
    {
        public CreateScreenDtoValidator()
        {
            RuleFor(x => x.ScreenName)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.TheatreId)
                .GreaterThan(0);

            RuleFor(x => x.TotalSeats)
                .GreaterThan(0);
        }
    }

    public class UpdateScreenDtoValidator : AbstractValidator<UpdateScreenDto>
    {
        public UpdateScreenDtoValidator()
        {
            RuleFor(x => x.ScreenName)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.TheatreId)
                .GreaterThan(0);

            RuleFor(x => x.TotalSeats)
                .GreaterThan(0);
        }
    }
}
