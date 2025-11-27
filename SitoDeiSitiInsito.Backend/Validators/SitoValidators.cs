using FluentValidation;

namespace SitoDeiSiti.Validators
{
    public class SitoValidators : AbstractValidator<int>
    {
        public SitoValidators()
        {
            RuleFor(x => x)
                .NotEmpty()
                .WithMessage("Id non specificato");
        }
    }
}
