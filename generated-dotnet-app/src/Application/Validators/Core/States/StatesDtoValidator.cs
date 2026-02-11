using BookMyShow.Application.DTOs.Core.States;
using FluentValidation;

namespace BookMyShow.Application.Validators.Core.States
{
    public class CreateStatesDtoValidator : AbstractValidator<CreateStatesDto>
    {
        public CreateStatesDtoValidator()
        {
            RuleFor(x => x.StateName)
                .NotEmpty().WithMessage("StateName is required")
                .MaximumLength(150).WithMessage("StateName cannot exceed 150 characters");
        }
    }

    public class UpdateStatesDtoValidator : AbstractValidator<UpdateStatesDto>
    {
        public UpdateStatesDtoValidator()
        {
            RuleFor(x => x.StateId)
                .NotEmpty().WithMessage("StateId is required");

            RuleFor(x => x.StateName)
                .NotEmpty().WithMessage("StateName is required")
                .MaximumLength(150).WithMessage("StateName cannot exceed 150 characters");
        }
    }
}
