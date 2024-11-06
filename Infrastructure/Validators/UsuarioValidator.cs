using Core.DTOs;
using Core.Messages;
using FluentValidation;

namespace Infrastructure.Validators
{
    public class UsuarioValidator : AbstractValidator<UsuarioDto>
    {
        public UsuarioValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleSet("CreateValidation", () =>
            {
                RuleFor(entity => entity.UsrCedula)
                    .NotNull().WithMessage(ErrorMessage.NullError)
                    .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                    .Length(1, 15).WithMessage(ErrorMessage.LengthError)
                    .WithName("Cédula");

                RuleFor(entity => entity.UsrTusrCodTipoUsuario)
                    .GreaterThan(0).WithMessage(ErrorMessage.ValueWithValueError)
                    .WithName("Tipo de usuario");

                RuleFor(entity => entity.UsrNombres)
                    .NotNull().WithMessage(ErrorMessage.NullError)
                    .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                    .Length(1, 100).WithMessage(ErrorMessage.LengthError)
                    .WithName("Nombre");

                RuleFor(entity => entity.UsrApellidos)
                    .NotNull().WithMessage(ErrorMessage.NullError)
                    .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                    .Length(1, 100).WithMessage(ErrorMessage.LengthError)
                    .WithName("Apellido");

                RuleFor(entity => entity.UsrEmail)
                    .NotNull().WithMessage(ErrorMessage.NullError)
                    .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                    .EmailAddress().WithMessage(ErrorMessage.ValueError)
                    .Length(1, 256).WithMessage(ErrorMessage.LengthError)
                    .WithName("Email");

                RuleFor(entity => entity.UsrEmpresaProceso)
                    .NotNull().WithMessage(ErrorMessage.NullError)
                    .WithName("Empresa");

                //RuleFor(entity => entity.UsrEstado)
                //    .NotNull().WithMessage(ErrorMessage.NullError)
                //    .WithName("Estado");

                RuleFor(entity => entity.CodUser)
                    .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                    .NotNull().WithMessage(ErrorMessage.NullError)
                    .Length(1, 15).WithMessage(ErrorMessage.LengthError);
            });

            RuleSet("UpdateValidation", () =>
            {
                RuleFor(entity => entity.Id)
                    .GreaterThan(0).WithMessage(ErrorMessage.ValueWithValueError);

                RuleFor(entity => entity.UsrCedula)
                    .NotNull().WithMessage(ErrorMessage.NullError)
                    .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                    .Length(1, 15).WithMessage(ErrorMessage.LengthError)
                    .WithName("Cédula");

                RuleFor(entity => entity.UsrTusrCodTipoUsuario)
                    .GreaterThan(0).WithMessage(ErrorMessage.ValueWithValueError)
                    .WithName("Tipo de usuario");

                RuleFor(entity => entity.UsrNombres)
                    .NotNull().WithMessage(ErrorMessage.NullError)
                    .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                    .Length(1, 100).WithMessage(ErrorMessage.LengthError)
                    .WithName("Nombre");

                RuleFor(entity => entity.UsrApellidos)
                    .NotNull().WithMessage(ErrorMessage.NullError)
                    .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                    .Length(1, 100).WithMessage(ErrorMessage.LengthError)
                    .WithName("Apellido");

                RuleFor(entity => entity.UsrEmail)
                    .NotNull().WithMessage(ErrorMessage.NullError)
                    .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                    .EmailAddress().WithMessage(ErrorMessage.ValueError)
                    .Length(1, 256).WithMessage(ErrorMessage.LengthError)
                    .WithName("Email");

                RuleFor(entity => entity.UsrEmpresaProceso)
                    .NotNull().WithMessage(ErrorMessage.NullError)
                    .WithName("Empresa");

                RuleFor(entity => entity.UsrEstado)
                    .NotNull().WithMessage(ErrorMessage.NullError)
                    .WithName("Estado");

                RuleFor(entity => entity.CodUserUpdate)
                    .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                    .NotNull().WithMessage(ErrorMessage.NullError)
                    .Length(1, 15).WithMessage(ErrorMessage.LengthError);
            });

            RuleSet("DeleteValidation", () =>
            {
                RuleFor(entity => entity.Id)
                    .GreaterThan(0).WithMessage(ErrorMessage.ValueWithValueError);

                RuleFor(entity => entity.CodUserUpdate)
                    .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                    .NotNull().WithMessage(ErrorMessage.NullError)
                    .Length(1, 15).WithMessage(ErrorMessage.LengthError);
            });

            RuleSet("UpdatePasswordValidation", () =>
            {
                RuleFor(entity => entity.Id)
                    .GreaterThan(0).WithMessage(ErrorMessage.ValueWithValueError);

                RuleFor(entity => entity.UsrPasswordSetter)
                    .NotNull().WithMessage(ErrorMessage.NullError)
                    .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                    .Matches("^(?=.*\\d)(?=.*[a-z])(?=.*[.,*?_#$%!=¡?)(/\\-+])(?=.*[A-Z]).{8,50}$").WithMessage(ErrorMessage.MatchError)
                    .WithName("Contraseña");

                RuleFor(entity => entity.CodUserUpdate)
                    .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                    .NotNull().WithMessage(ErrorMessage.NullError)
                    .Length(1, 15).WithMessage(ErrorMessage.LengthError);
            });

            RuleSet("ChangePasswordValidation", () =>
            {
                RuleFor(entity => entity.Id)
                    .GreaterThan(0).WithMessage(ErrorMessage.ValueWithValueError);

                RuleFor(entity => entity.UsrCedula)
                    .NotNull().WithMessage(ErrorMessage.NullError)
                    .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                    .Length(1, 15).WithMessage(ErrorMessage.LengthError)
                    .WithName("Cédula");

                RuleFor(entity => entity.UsrPasswordSetter)
                    .NotNull().WithMessage(ErrorMessage.NullError)
                    .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                    .Matches("^(?=.*\\d)(?=.*[a-z])(?=.*[.,*?_#$%!=¡?)(/\\-+])(?=.*[A-Z]).{8,50}$").WithMessage(ErrorMessage.MatchError)
                    .WithName("Contraseña");

                RuleFor(entity => entity.OldPassword)
                    .NotNull().WithMessage(ErrorMessage.NullError)
                    .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                    .Matches("^(?=.*\\d)(?=.*[a-z])(?=.*[.,*?_#$%!=¡?)(/\\-+])(?=.*[A-Z]).{8,50}$").WithMessage(ErrorMessage.MatchError)
                    .WithName("Contraseña");

                RuleFor(entity => entity.CodUserUpdate)
                    .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                    .NotNull().WithMessage(ErrorMessage.NullError)
                    .Length(1, 15).WithMessage(ErrorMessage.LengthError);

                RuleFor(entity => entity.UsrPasswordSetter)
                    .Equal(entity => entity.OldPassword).WithMessage("La nueva contraseña no puede ser igual a la anterior");

                RuleFor(entity => entity.UsrPasswordSetter)
                    .Equal(entity => entity.UsrCedula).WithMessage("La nueva contraseña no puede ser igual a la identificación del usuario");
            });

            RuleSet("ResetPassValidator", () =>
            {
                RuleFor(entity => entity.Id)
                    .GreaterThan(0).WithMessage(ErrorMessage.ValueWithValueError);

                RuleFor(entity => entity.UsrCedula)
                    .NotNull().WithMessage(ErrorMessage.NullError)
                    .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                    .Length(1, 15).WithMessage(ErrorMessage.LengthError)
                    .WithName("Cédula");

                RuleFor(entity => entity.CodUserUpdate)
                    .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                    .NotNull().WithMessage(ErrorMessage.NullError)
                    .Length(1, 15).WithMessage(ErrorMessage.LengthError);
            });

            RuleSet("ForgottenPasswordValidator", () =>
            {

                RuleFor(entity => entity.UsrCedula)
                    .NotNull().WithMessage(ErrorMessage.NullError)
                    .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                    .Length(1, 15).WithMessage(ErrorMessage.LengthError)
                    .WithName("Cédula");

                RuleFor(entity => entity.UsrEmail)
                    .NotNull().WithMessage(ErrorMessage.NullError)
                    .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                    .EmailAddress().WithMessage(ErrorMessage.ValueError)
                    .Length(1, 256).WithMessage(ErrorMessage.LengthError)
                    .WithName("Email");
            });

            RuleSet("ChangeEmpresaValidator", () =>
            {
                RuleFor(entity => entity.Id)
                    .GreaterThan(0).WithMessage(ErrorMessage.ValueWithValueError);

                RuleFor(entity => entity.UsrCedula)
                    .NotNull().WithMessage(ErrorMessage.NullError)
                    .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                    .Length(1, 15).WithMessage(ErrorMessage.LengthError)
                    .WithName("Cédula");

                RuleFor(entity => entity.UsrEmpresaProceso)
                    .NotNull().WithMessage(ErrorMessage.NullError)
                    .WithName("Empresa");
            });
        }
    }
}
