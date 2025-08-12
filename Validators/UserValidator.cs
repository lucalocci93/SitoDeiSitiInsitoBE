using FluentValidation;
using SitoDeiSiti.DTOs;
using System.Runtime.CompilerServices;

namespace SitoDeiSiti.Validators
{
    public class CreateNewUserValidator : AbstractValidator<User>
    {
        public CreateNewUserValidator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty()
                .WithMessage("Il nome non può essere vuoto")
                .Length(2, 50)
                .WithMessage("Il nome deve avere tra 2 e 50 caratteri");
            RuleFor(x => x.Cognome)
                .NotEmpty()
                .WithMessage("Il cognome non può essere vuoto")
                .Length(2, 50)
                .WithMessage("Il cognome deve avere tra 2 e 50 caratteri");
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("L'email non può essere vuota")
                .EmailAddress()
                .WithMessage("L'email non è valida");
            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("La password non può essere vuota")
                .MinimumLength(6)
                .WithMessage("La password deve avere almeno 6 caratteri");
        }
    }

    public class AuthValidator : AbstractValidator<User>
    {
        public AuthValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("L'email non può essere vuota").WithErrorCode("EmptyEmail")
                .EmailAddress()
                .WithMessage("L'email non è valida").WithErrorCode("NotValidEmail");
            RuleFor(x => x.Password)
                .NotEqual("e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855")
                .WithMessage("La password non può essere vuota").WithErrorCode("EmptyPassword")
                .MinimumLength(6)
                .WithMessage("La password deve avere almeno 6 caratteri");
        }
    }
}
