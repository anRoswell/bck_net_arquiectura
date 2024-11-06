using Core.Messages;
using Core.QueryFilters;
using FluentValidation;

namespace Infrastructure.Validators
{
    public class RecoveryParamsValidator : AbstractValidator<RecoveryParams>
    {
        public RecoveryParamsValidator()
        {
            RuleFor(entity => entity.NewPassword)
                    .NotNull().WithMessage(ErrorMessage.NullError)
                    .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                    .Matches("^(?=.*\\d)(?=.*[a-z])(?=.*[.,*?_#$%!=¡?)(/\\-+])(?=.*[A-Z]).{8,50}$").WithMessage(ErrorMessage.MatchError)
                    .WithName("Contraseña");

            RuleFor(entity => entity.Token)
                .NotNull().WithMessage(ErrorMessage.NullError)
                .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                .WithName("Token");
        }
    }
}
