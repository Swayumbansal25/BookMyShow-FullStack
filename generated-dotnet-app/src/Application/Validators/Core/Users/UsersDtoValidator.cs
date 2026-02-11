using BookMyShow.Application.DTOs.Core.Users;
using FluentValidation;

namespace BookMyShow.Application.Validators.Core.Users
{
    /// <summary>
    /// Validator for CreateUsersDto
    /// </summary>
    public class CreateUsersDtoValidator : AbstractValidator<CreateUsersDto>
    {
        public CreateUsersDtoValidator()
        {
RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("FullName is required")
                .MaximumLength(255).WithMessage("FullName cannot exceed 255 characters");

RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .MaximumLength(255).WithMessage("Email cannot exceed 255 characters");

RuleFor(x => x.PhoneNumber)
                .MaximumLength(255).WithMessage("PhoneNumber cannot exceed 255 characters")
                .When(x => !string.IsNullOrEmpty(x.PhoneNumber));

RuleFor(x => x.PasswordHash)
                .NotEmpty().WithMessage("PasswordHash is required")
                .MaximumLength(255).WithMessage("PasswordHash cannot exceed 255 characters");

RuleFor(x => x.Gender)
                .MaximumLength(255).WithMessage("Gender cannot exceed 255 characters")
                .When(x => !string.IsNullOrEmpty(x.Gender));

}
    }

    /// <summary>
    /// Validator for UpdateUsersDto
    /// </summary>
    public class UpdateUsersDtoValidator : AbstractValidator<UpdateUsersDto>
    {
        public UpdateUsersDtoValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required");

RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("FullName is required")
                .MaximumLength(255).WithMessage("FullName cannot exceed 255 characters");

RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .MaximumLength(255).WithMessage("Email cannot exceed 255 characters");

RuleFor(x => x.PhoneNumber)
                .MaximumLength(255).WithMessage("PhoneNumber cannot exceed 255 characters")
                .When(x => !string.IsNullOrEmpty(x.PhoneNumber));

RuleFor(x => x.PasswordHash)
                .NotEmpty().WithMessage("PasswordHash is required")
                .MaximumLength(255).WithMessage("PasswordHash cannot exceed 255 characters");

RuleFor(x => x.Gender)
                .MaximumLength(255).WithMessage("Gender cannot exceed 255 characters")
                .When(x => !string.IsNullOrEmpty(x.Gender));

}
    }
}