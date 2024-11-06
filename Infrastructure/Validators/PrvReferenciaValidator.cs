using Core.Entities;
using Core.Messages;
using FluentValidation;

namespace Infrastructure.Validators
{

    public class PrvReferenciaValidator : AbstractValidator<PrvReferencia>
    {
        public PrvReferenciaValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            // La regla se ejecutará al crear, actualizar o por defecto
            RuleSet("CreateValidation, UpdateValidation, default", () =>
            {
                RuleFor(x => x.RefContacto)
                .NotNull().WithMessage(ErrorMessage.NullError)
                .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                .Length(1, 200).WithMessage(ErrorMessage.LengthError)
                .WithName("Contacto de Referencia");

                RuleFor(x => x.RefEmpresa)
                    .NotNull().WithMessage(ErrorMessage.NullError)
                    .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                    .Length(1, 150).WithMessage(ErrorMessage.LengthError)
                    .WithName("Empresa de Referencia");

                RuleFor(x => x.RefTelefono)
                    .NotNull().WithMessage(ErrorMessage.NullError)
                    .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                    .Length(1, 50).WithMessage(ErrorMessage.LengthError)
                    .WithName("Telefono de Referencia");
            });
        }
    }
}
