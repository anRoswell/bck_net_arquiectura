using Core.Messages;
using Core.QueryFilters;
using FluentValidation;

namespace Infrastructure.Validators
{
    public class QuerySendMailMasiveValidator: AbstractValidator<QuerySendMailMasive>
    {
        public QuerySendMailMasiveValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.Asunto)
                .NotNull().WithMessage(ErrorMessage.NullError)
                .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                .Length(1, 256).WithMessage(ErrorMessage.LengthError)
                .WithName("Asunto");

            RuleFor(x => x.Body)
                .NotNull().WithMessage(ErrorMessage.NullError)
                .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                .WithName("Body");

            RuleFor(x => x.CCO)
                .NotNull().WithMessage(ErrorMessage.NullError)
                .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                .WithName("CCO");

            RuleFor(x => x.To)
                .NotNull().WithMessage(ErrorMessage.NullError)
                .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                .WithName("Para");
        }
    }
}
