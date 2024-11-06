using Core.Messages;
using Core.QueryFilters;
using FluentValidation;

namespace Infrastructure.Validators
{
    public class QueryUpdateTmpSuspendidoValidator : AbstractValidator<QueryToken>
    {
        public QueryUpdateTmpSuspendidoValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.Token)
                .NotNull().WithMessage(ErrorMessage.NullError)
                .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                .WithName("Token");
        }
    }
}
