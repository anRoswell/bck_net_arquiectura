using Core.Entities;
using Core.Messages;
using FluentValidation;

namespace Infrastructure.Validators
{
    public class PrvSocioValidator : AbstractValidator<PrvSocio>
    {
        public PrvSocioValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleSet("CreateValidation, UpdateValidation, default", () =>
            {
                RuleFor(x => x.SocCodCiudad)
                     .NotNull().WithMessage(ErrorMessage.NullError)
                    .WithName("Ciudad de Socio");

                RuleFor(x => x.SocDireccion)
                    .NotNull().WithMessage(ErrorMessage.NullError)
                    .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                    .Length(1, 250).WithMessage(ErrorMessage.LengthError)
                    .WithName("Dirección de Socio");

                RuleFor(x => x.SocIdentificacion)
                    .NotNull().WithMessage(ErrorMessage.NullError)
                    .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                    .Length(1, 15).WithMessage(ErrorMessage.LengthError)
                    .WithName("Identificación de Socio");

                RuleFor(x => x.SocDigVerificacion)
                    .NotNull().WithMessage(ErrorMessage.NullError)
                    .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                    .Length(1, 1).WithMessage(ErrorMessage.LengthError)
                    .WithName("Digito verificación de Socio");

                RuleFor(x => x.SocNombre)
                    .NotNull().WithMessage(ErrorMessage.NullError)
                    .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                    .Length(1, 200).WithMessage(ErrorMessage.LengthError)
                    .WithName("Nombre de Socio");
            });
        }
    }
}
