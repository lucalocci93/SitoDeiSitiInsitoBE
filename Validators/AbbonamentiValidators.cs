using FluentValidation;
using SitoDeiSiti.DTOs;

namespace SitoDeiSiti.Validators
{
    public class GetUserSubscriptionsValidator : AbstractValidator<Guid>
    {
        public GetUserSubscriptionsValidator()
        {
            RuleFor(x => x)
                .NotEmpty().WithMessage("L'utente non può essere vuoto")
                .NotNull().WithMessage("L'utente non può essere nullo");
        }
    }

    public class UpdateSubscriptionValidator : AbstractValidator<Subscription>
    {
        public UpdateSubscriptionValidator()
        {
            RuleFor(x => x)
                .NotNull().WithMessage("L'abbonamento non può essere nullo")
                .NotEmpty().WithMessage("L'abbonamento non può essere vuoto");
        }
    }
}
