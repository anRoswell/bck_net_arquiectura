using Core.Entities;
using Core.Messages;
using FluentValidation;

namespace Infrastructure.Validators
{
    public class UserLoginValidator : AbstractValidator<UserLogin>
    {
        public UserLoginValidator()
        {
            RuleFor(entity => entity.Email)
                .EmailAddress().WithMessage(ErrorMessage.ValueError)
                .Length(1, 256).WithMessage(ErrorMessage.LengthError)
                .WithName("Email");

            RuleFor(entity => entity.Password)
                .NotNull().WithMessage(ErrorMessage.NullError)
                .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                //.Matches("^(?=.*\\d)(?=.*[a-z])(?=.*[.,*?_#$%!=¡?)(/\\-+])(?=.*[A-Z]).{8,50}$").WithMessage(ErrorMessage.MatchError)
                .WithName("Contraseña");
        }
    }
}
