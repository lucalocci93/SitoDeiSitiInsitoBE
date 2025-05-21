using FluentValidation;
using SitoDeiSiti.DTOs;

namespace SitoDeiSiti.Validators
{
    public class GetEventoValidators : AbstractValidator<Guid>
    {
        public GetEventoValidators()
        {
            RuleFor(x => x)
                .NotEmpty()
                .WithMessage("L'ID dell'evento non può essere vuoto.");
        }
    }

    public class SubscribeEventValidator : AbstractValidator<EventSubscription>
    {
        public SubscribeEventValidator()
        {
            RuleFor(x => x)
                .NotNull()
                .WithMessage("L'oggetto di iscrizione non può essere nullo.");
            RuleFor(x => x.EventId)
                .NotEmpty()
                .WithMessage("L'ID dell'evento non può essere vuoto.");
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("L'ID dell'utente non può essere vuoto.");
        }
    }

    public class GetEventSubscriptionByUserValidator : AbstractValidator<Guid>
    {
        public GetEventSubscriptionByUserValidator()
        {
            RuleFor(x => x)
                .NotEmpty()
                .WithMessage("L'ID dell'utente non può essere vuoto.");
        }
    }

    public class DeleteSubscriptionValidator : AbstractValidator<EventSubscription>
    {
        public DeleteSubscriptionValidator()
        {
            RuleFor(x => x.EventId)
                .NotEmpty()
                .WithMessage("L'ID dell'evento non può essere vuoto.");
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("L'ID dell'utente non può essere vuoto.");
            RuleFor(x => x.Categories)
                .NotEmpty()
                .WithMessage("La lista delle categorie non può essere vuota.");
        }
    }
}
