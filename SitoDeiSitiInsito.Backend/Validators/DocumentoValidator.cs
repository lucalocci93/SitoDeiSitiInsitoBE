using FluentValidation;

namespace SitoDeiSiti.Validators
{
    public class GetUserDocumentValidator : AbstractValidator<Guid>
    {
        public GetUserDocumentValidator()
        {
            RuleFor(x => x)
                .NotEmpty()
                .WithMessage("L'ID dell'utente non può essere vuoto.");
        }
    }

    public class GetDocumentValidator : AbstractValidator<Guid>
    {
        public GetDocumentValidator()
        {
            RuleFor(x => x)
                .NotEmpty()
                .WithMessage("L'ID del documento non può essere vuoto.");
        }
    }
}
