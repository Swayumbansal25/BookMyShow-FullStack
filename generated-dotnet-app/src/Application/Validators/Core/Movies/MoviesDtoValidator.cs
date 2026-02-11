using BookMyShow.Application.DTOs.Core.Movies;
using FluentValidation;

namespace BookMyShow.Application.Validators.Core.Movies
{
    public class CreateMoviesDtoValidator : AbstractValidator<CreateMoviesDto>
    {
        public CreateMoviesDtoValidator()
        {
            RuleFor(x => x.MovieName).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Language).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Genre).NotEmpty().MaximumLength(100);
            RuleFor(x => x.DurationMinutes).GreaterThan(0);
            RuleFor(x => x.ReleaseDate).NotEmpty();
        }
    }

    public class UpdateMoviesDtoValidator : AbstractValidator<UpdateMoviesDto>
    {
        public UpdateMoviesDtoValidator()
        {
            RuleFor(x => x.MovieId).NotEmpty();
            RuleFor(x => x.MovieName).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Language).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Genre).NotEmpty().MaximumLength(100);
            RuleFor(x => x.DurationMinutes).GreaterThan(0);
            RuleFor(x => x.ReleaseDate).NotEmpty();
        }
    }
}
