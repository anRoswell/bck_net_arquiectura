using Core.DTOs;
using Core.Entities;
using Core.Messages;
using FluentValidation;

namespace Infrastructure.Validators
{
    public class TerceroActualizarValidator : AbstractValidator<TerceroActualizarDto>
    {
        public TerceroActualizarValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleSet("CreateValidation", () =>
            {
                RuleFor(entity => entity.CodUser)
                    .NotNull().WithMessage(ErrorMessage.NullError)
                    .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                    .Length(1, 15).WithMessage(ErrorMessage.LengthError);

                /*** ListEmpresas ***/
                // Validacion a nivel de lista
                RuleFor(entity => entity.ListEmpresas)
                    .NotNull().WithMessage(ErrorMessage.NullError)
                    .Must(emps => emps.Count >= 1).WithMessage(ErrorMessage.CountError)
                    /*.ForEach(element => {
                        element.GreaterThan(0).WithMessage(ErrorMessage.ValueWithValueError);
                    })*/
                    .WithName("Empresas Proveedor");

                // Validacion a nivel de valor de lista
                RuleForEach(entity => entity.ListEmpresas)
                    .Must(emp => emp >= 1).WithMessage(ErrorMessage.ValueWithValueError)
                    .WithName("Empresas Proveedor");

                /*** ListProdServicios ***/
                // Validacion a nivel de lista
                RuleFor(entity => entity.ListProdServicios)
                    .NotNull().WithMessage(ErrorMessage.NullError)
                    .Must(prods => prods.Count >= 1).WithMessage(ErrorMessage.CountError)
                    .WithName("Productos y Servicios Proveedor");

                // Validacion a nivel de valor de lista
                RuleForEach(entity => entity.ListProdServicios)
                    .Must(prod => prod >= 1).WithMessage(ErrorMessage.ValueWithValueError)
                    .WithName("Productos y Servicios Proveedor");

                RuleFor(entity => entity.PrvCodBanco)
                    .GreaterThan(0).WithMessage(ErrorMessage.ValueWithValueError)
                    .WithName("Código Banco");

                RuleFor(entity => entity.PrvCodCiudad)
                    .NotNull().WithMessage(ErrorMessage.NullError)
                    .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                    .WithName("Ciudad Proveedor");

                RuleFor(entity => entity.PrvCodTipoCuenta)
                    .GreaterThan(0).WithMessage(ErrorMessage.ValueWithValueError)
                    .WithName("Tipo de Cuenta");

                RuleFor(entity => entity.PrvCodTipoProveeedor)
                    .NotNull().WithMessage(ErrorMessage.NullError)
                    .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                    .Length(1, 200).WithMessage(ErrorMessage.LengthError)
                    .WithName("Tipo de Proveedor");

                RuleFor(entity => entity.PrvContacto)
                    .NotNull().WithMessage(ErrorMessage.NullError)
                    .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                    .Length(1, 200).WithMessage(ErrorMessage.LengthError)
                    .WithName("Contacto");

                RuleFor(entity => entity.PrvCpaCodCondicionesPago)
                    .GreaterThan(0).WithMessage(ErrorMessage.ValueWithValueError)
                    .WithName("Condición de Pago");

                RuleFor(entity => entity.PrvDeclaramientoInhabilidadesInteres)
                    .NotNull().WithMessage(ErrorMessage.NullError)
                    .WithName("Declaramiento Inhabilidades Interes");

                RuleFor(entity => entity.PrvDireccion)
                    .NotNull().WithMessage(ErrorMessage.NullError)
                    .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                    .Length(1, 200).WithMessage(ErrorMessage.LengthError)
                    .WithName("Dirección Proveedor");

                RuleFor(entity => entity.PrvDtllesBanNroCuenta)
                    .NotNull().WithMessage(ErrorMessage.NullError)
                    .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                    .Length(1, 100).WithMessage(ErrorMessage.LengthError)
                    .WithName("Número de Cuenta Proveedor");

                RuleFor(entity => entity.PrvExperienciaSector)
                    .NotNull().WithMessage(ErrorMessage.NullError)
                    .WithName("Experiencia en el Sector");

                RuleFor(entity => entity.PrvMail)
                    .NotNull().WithMessage(ErrorMessage.NullError)
                    .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                    .EmailAddress().WithMessage(ErrorMessage.ValueError)
                    .Length(1, 100).WithMessage(ErrorMessage.LengthError)
                    .WithName("Email Proveedor");

                RuleFor(entity => entity.PrvMailAlterno)
                    .NotNull().WithMessage(ErrorMessage.NullError)
                    .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                    .EmailAddress().WithMessage(ErrorMessage.ValueError)
                    .Length(1, 100).WithMessage(ErrorMessage.LengthError)
                    .WithName("Email Alterno Proveedor");

                RuleFor(entity => entity.PrvNit)
                    .NotNull().WithMessage(ErrorMessage.NullError)
                    .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                    .Length(1, 15).WithMessage(ErrorMessage.LengthError)
                    .WithName("Nit");

                //RuleFor(entity => entity.PrvDigitoVerificacion)
                //    .NotNull().WithMessage(ErrorMessage.NullError)
                //    .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                //    .Length(1, 1).WithMessage(ErrorMessage.LengthError)
                //    .WithName("Digito verificación Proveedor");

                RuleFor(entity => entity.PrvNombreProveedor)
                    .NotNull().WithMessage(ErrorMessage.NullError)
                    .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                    .Length(1, 300).WithMessage(ErrorMessage.LengthError)
                    .WithName("Nombre Proveedor");

                RuleFor(entity => entity.PrvPoliticaTratamientoDatosPersonales)
                    .NotNull().WithMessage(ErrorMessage.NullError)
                    .WithName("Políticas de Tratamiento Datos Personales");

                RuleFor(entity => entity.PrvProveeedor)
                    .NotNull().WithMessage(ErrorMessage.NullError)
                    .WithName("Segmento Proveedor");

                /*** PrvReferencias ***/
                // Validacion a nivel de lista
                RuleFor(entity => entity.PrvReferencias)
                    .NotNull().WithMessage(ErrorMessage.NullError)
                    .Must(refs => refs.Count >= 1).WithMessage(ErrorMessage.CountError)
                    .WithName("Referencia Proveedor");

                // Validacion a nivel de valor de lista
                RuleForEach(entity => entity.PrvReferencias)
                    .NotNull().WithMessage(ErrorMessage.NullError)
                    .SetValidator(new PrvReferenciaValidator()); // Llama a PrvReferenciaValidator

                //RuleFor(entity => entity.PrvRteLegalApellido)
                //    .NotNull().WithMessage(ErrorMessage.NullError)
                //    .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                //    .Length(1, 200).WithMessage(ErrorMessage.LengthError)
                //    .WithName("Apellido Rte Legal");

                //RuleFor(entity => entity.PrvRteLegalCodCiudad)
                //    .NotNull().WithMessage(ErrorMessage.NullError)
                //    .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                //    .Length(1, 5).WithMessage(ErrorMessage.LengthError)
                //    .WithName("Ciudad Rte Legal");

                //RuleFor(entity => entity.PrvRteLegalEmail)
                //    .NotNull().WithMessage(ErrorMessage.NullError)
                //    .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                //    .EmailAddress().WithMessage(ErrorMessage.ValueError)
                //    .Length(1, 100).WithMessage(ErrorMessage.LengthError)
                //    .WithName("Email Rte Legal");

                //RuleFor(entity => entity.PrvRteLegalIdentificacion)
                //    .NotNull().WithMessage(ErrorMessage.NullError)
                //    .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                //    .Length(1, 15).WithMessage(ErrorMessage.LengthError)
                //    .WithName("Identificación Rte Legal");

                //RuleFor(entity => entity.PrvRteLegalDigVerificacion)
                //    .NotNull().WithMessage(ErrorMessage.NullError)
                //    .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                //    .Length(1, 1).WithMessage(ErrorMessage.LengthError)
                //    .WithName("Digito verificación Rte Legal");

                //RuleFor(entity => entity.PrvRteLegalNombre)
                //    .NotNull().WithMessage(ErrorMessage.NullError)
                //    .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                //    .Length(1, 200).WithMessage(ErrorMessage.LengthError)
                 //   .WithName("Nombre Rte Legal");

                //RuleFor(entity => entity.PrvRteLegalTelefonoMovil)
                //    .NotNull().WithMessage(ErrorMessage.NullError)
                //    .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                //    .Length(1, 10).WithMessage(ErrorMessage.LengthError)
                //    .WithName("Telefono Rte Legal");

                //RuleFor(entity => entity.PrvRevFiscalCodCiudad)
                //    .NotNull().WithMessage(ErrorMessage.NullError)
                //    .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                //    .Length(1, 5).WithMessage(ErrorMessage.LengthError)
                //    .WithName("Ciudad Revisor Fiscal");

                //RuleFor(entity => entity.PrvRevFiscalEmail)
                //    .NotNull().WithMessage(ErrorMessage.NullError)
                //    .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                //    .EmailAddress().WithMessage(ErrorMessage.ValueError)
                //    .Length(1, 100).WithMessage(ErrorMessage.LengthError)
                //    .WithName("Email Revisor Fiscal");

                //RuleFor(entity => entity.PrvRevFiscalIdentificacion)
                //    .NotNull().WithMessage(ErrorMessage.NullError)
                //    .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                //    .Length(1, 15).WithMessage(ErrorMessage.LengthError)
                //    .WithName("Identificación Revisor Fiscal");

                //RuleFor(entity => entity.PrvRevFiscalDigVerificacion)
                //    .NotNull().WithMessage(ErrorMessage.NullError)
                //    .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                //    .Length(1, 1).WithMessage(ErrorMessage.LengthError)
                //    .WithName("Digito verificación Revisor Fiscal");

                //RuleFor(entity => entity.PrvRevFiscalNombre)
                //    .NotNull().WithMessage(ErrorMessage.NullError)
                //    .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                //    .Length(1, 200).WithMessage(ErrorMessage.LengthError)
                //    .WithName("Nombre Revisor Fiscal");

                //RuleFor(entity => entity.PrvRevFiscalTelefonoMovil)
                //    .NotNull().WithMessage(ErrorMessage.NullError)
                //    .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                //    .Length(1, 10).WithMessage(ErrorMessage.LengthError)
                //    .WithName("Teléfono Revisor Fiscal");

                /*** PrvSocios ***/
                // Validacion a nivel de lista
                //RuleFor(entity => entity.PrvSocios)
                //    .NotNull().WithMessage(ErrorMessage.NullError)
                //    .Must(prods => prods.Count >= 1).WithMessage(ErrorMessage.CountError)
                //    .WithName("Socio Proveedor");

                // Validacion a nivel de valor de lista
                //RuleForEach(entity => entity.PrvSocios)
                //    .NotNull().WithMessage(ErrorMessage.NullError)
                //    .SetValidator(new PrvSocioValidator()); // Llama a PrvSocioValidator

                RuleFor(entity => entity.PrvTelefono)
                    .NotNull().WithMessage(ErrorMessage.NullError)
                    .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                    .Length(1, 10).WithMessage(ErrorMessage.LengthError)
                    .WithName("Telefono Proveedor");
            });

            RuleSet("UpdateValidation", () =>
            {
                RuleFor(entity => entity.Id)
                    .GreaterThan(0).WithMessage(ErrorMessage.ValueWithValueError);

                RuleFor(entity => entity.CodUser)
                    .NotNull().WithMessage(ErrorMessage.NullError)
                    .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                    .Length(1, 15).WithMessage(ErrorMessage.LengthError);

                /*** ListEmpresas ***/
                // Validacion a nivel de lista
                RuleFor(entity => entity.ListEmpresas)
                    .NotNull().WithMessage(ErrorMessage.NullError)
                    .Must(emps => emps.Count >= 1).WithMessage(ErrorMessage.CountError)
                    /*.ForEach(element => {
                        element.GreaterThan(0).WithMessage(ErrorMessage.ValueWithValueError);
                    })*/
                    .WithName("Empresas Proveedor");

                // Validacion a nivel de valor de lista
                RuleForEach(entity => entity.ListEmpresas)
                    .Must(emp => emp >= 1).WithMessage(ErrorMessage.ValueWithValueError)
                    .WithName("Empresas Proveedor");

                /*** ListProdServicios ***/
                // Validacion a nivel de lista
                RuleFor(entity => entity.ListProdServicios)
                    .NotNull().WithMessage(ErrorMessage.NullError)
                    .Must(prods => prods.Count >= 1).WithMessage(ErrorMessage.CountError)
                    .WithName("Productos y Servicios Proveedor");

                // Validacion a nivel de valor de lista
                RuleForEach(entity => entity.ListProdServicios)
                    .Must(prod => prod >= 1).WithMessage(ErrorMessage.ValueWithValueError)
                    .WithName("Productos y Servicios Proveedor");

                RuleFor(entity => entity.PrvCodBanco)
                    .GreaterThan(0).WithMessage(ErrorMessage.ValueWithValueError)
                    .WithName("Código Banco");

                RuleFor(entity => entity.PrvCodCiudad)
                    .NotNull().WithMessage(ErrorMessage.NullError)
                    .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                    .WithName("Ciudad Proveedor");

                RuleFor(entity => entity.PrvCodTipoCuenta)
                    .GreaterThan(0).WithMessage(ErrorMessage.ValueWithValueError)
                    .WithName("Tipo de Cuenta");

                RuleFor(entity => entity.PrvCodTipoProveeedor)
                    .NotNull().WithMessage(ErrorMessage.NullError)
                    .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                    .Length(1, 200).WithMessage(ErrorMessage.LengthError)
                    .WithName("Tipo de Proveedor");

                RuleFor(entity => entity.PrvContacto)
                    .NotNull().WithMessage(ErrorMessage.NullError)
                    .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                    .Length(1, 200).WithMessage(ErrorMessage.LengthError)
                    .WithName("Contacto");

                RuleFor(entity => entity.PrvCpaCodCondicionesPago)
                    .GreaterThan(0).WithMessage(ErrorMessage.ValueWithValueError)
                    .WithName("Condición de Pago");

                RuleFor(entity => entity.PrvDeclaramientoInhabilidadesInteres)
                    .NotNull().WithMessage(ErrorMessage.NullError)
                    .WithName("Declaramiento Inhabilidades Interes");

                RuleFor(entity => entity.PrvDireccion)
                    .NotNull().WithMessage(ErrorMessage.NullError)
                    .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                    .Length(1, 200).WithMessage(ErrorMessage.LengthError)
                    .WithName("Dirección Proveedor");

                RuleFor(entity => entity.PrvDtllesBanNroCuenta)
                    .NotNull().WithMessage(ErrorMessage.NullError)
                    .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                    .Length(1, 100).WithMessage(ErrorMessage.LengthError)
                    .WithName("Número de Cuenta Proveedor");

                RuleFor(entity => entity.PrvExperienciaSector)
                    .NotNull().WithMessage(ErrorMessage.NullError)
                    .WithName("Experiencia en el Sector");

                RuleFor(entity => entity.PrvMail)
                    .NotNull().WithMessage(ErrorMessage.NullError)
                    .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                    .EmailAddress().WithMessage(ErrorMessage.ValueError)
                    .Length(1, 100).WithMessage(ErrorMessage.LengthError)
                    .WithName("Email Proveedor");

                RuleFor(entity => entity.PrvNit)
                    .NotNull().WithMessage(ErrorMessage.NullError)
                    .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                    .Length(1, 15).WithMessage(ErrorMessage.LengthError)
                    .WithName("Nit");

                //RuleFor(entity => entity.PrvDigitoVerificacion)
                //    .NotNull().WithMessage(ErrorMessage.NullError)
                //    .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                //    .Length(1, 1).WithMessage(ErrorMessage.LengthError)
                //    .WithName("Digito verificación Proveedor");

                RuleFor(entity => entity.PrvNombreProveedor)
                    .NotNull().WithMessage(ErrorMessage.NullError)
                    .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                    .Length(1, 300).WithMessage(ErrorMessage.LengthError)
                    .WithName("Nombre Proveedor");

                RuleFor(entity => entity.PrvPoliticaTratamientoDatosPersonales)
                    .NotNull().WithMessage(ErrorMessage.NullError)
                    .WithName("Políticas de Tratamiento Datos Personales");

                RuleFor(entity => entity.PrvProveeedor)
                    .NotNull().WithMessage(ErrorMessage.NullError)
                    .WithName("Segmento Proveedor");

                /*** PrvReferencias ***/
                // Validacion a nivel de lista
                RuleFor(entity => entity.PrvReferencias)
                    .NotNull().WithMessage(ErrorMessage.NullError)
                    .Must(refs => refs.Count >= 1).WithMessage(ErrorMessage.CountError)
                    .WithName("Referencia Proveedor");

                // Validacion a nivel de valor de lista
                RuleForEach(entity => entity.PrvReferencias)
                    .NotNull().WithMessage(ErrorMessage.NullError)
                    .SetValidator(new PrvReferenciaValidator()); // Llama a PrvReferenciaValidator

                //RuleFor(entity => entity.PrvRteLegalApellido)
                //    .NotNull().WithMessage(ErrorMessage.NullError)
                //    .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                //    .Length(1, 200).WithMessage(ErrorMessage.LengthError)
                //    .WithName("Apellido Rte Legal");

                //RuleFor(entity => entity.PrvRteLegalCodCiudad)
                //    .NotNull().WithMessage(ErrorMessage.NullError)
                //    .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                //    .Length(1, 5).WithMessage(ErrorMessage.LengthError)
                //    .WithName("Ciudad Rte Legal");

                //RuleFor(entity => entity.PrvRteLegalEmail)
                //    .NotNull().WithMessage(ErrorMessage.NullError)
                //    .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                //    .EmailAddress().WithMessage(ErrorMessage.ValueError)
                //    .Length(1, 100).WithMessage(ErrorMessage.LengthError)
                //    .WithName("Email Rte Legal");

                //RuleFor(entity => entity.PrvRteLegalIdentificacion)
                //    .NotNull().WithMessage(ErrorMessage.NullError)
                //    .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                //    .Length(1, 15).WithMessage(ErrorMessage.LengthError)
                //    .WithName("Identificación Rte Legal");

                //RuleFor(entity => entity.PrvRteLegalDigVerificacion)
                //    .NotNull().WithMessage(ErrorMessage.NullError)
                //    .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                //    .Length(1, 1).WithMessage(ErrorMessage.LengthError)
                //    .WithName("Digito verificación Rte Legal");

                //RuleFor(entity => entity.PrvRteLegalNombre)
                //    .NotNull().WithMessage(ErrorMessage.NullError)
                //    .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                //    .Length(1, 200).WithMessage(ErrorMessage.LengthError)
                //    .WithName("Nombre Rte Legal");

                //RuleFor(entity => entity.PrvRteLegalTelefonoMovil)
                //    .NotNull().WithMessage(ErrorMessage.NullError)
                //    .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                //    .Length(1, 10).WithMessage(ErrorMessage.LengthError)
                //    .WithName("Telefono Rte Legal");

                //RuleFor(entity => entity.PrvRevFiscalCodCiudad)
                //    .NotNull().WithMessage(ErrorMessage.NullError)
                //    .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                //    .Length(1, 5).WithMessage(ErrorMessage.LengthError)
                //    .WithName("Ciudad Revisor Fiscal");

                //RuleFor(entity => entity.PrvRevFiscalEmail)
                //    .NotNull().WithMessage(ErrorMessage.NullError)
                //    .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                //    .EmailAddress().WithMessage(ErrorMessage.ValueError)
                //    .Length(1, 100).WithMessage(ErrorMessage.LengthError)
                //    .WithName("Email Revisor Fiscal");

                //RuleFor(entity => entity.PrvRevFiscalIdentificacion)
                //    .NotNull().WithMessage(ErrorMessage.NullError)
                //    .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                //    .Length(1, 15).WithMessage(ErrorMessage.LengthError)
                //    .WithName("Identificación Revisor Fiscal");

                //RuleFor(entity => entity.PrvRevFiscalDigVerificacion)
                //    .NotNull().WithMessage(ErrorMessage.NullError)
                //    .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                //    .Length(1, 1).WithMessage(ErrorMessage.LengthError)
                //    .WithName("Digito verificación Revisor Fiscal");

                //RuleFor(entity => entity.PrvRevFiscalNombre)
                //    .NotNull().WithMessage(ErrorMessage.NullError)
                //    .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                //    .Length(1, 200).WithMessage(ErrorMessage.LengthError)
                //    .WithName("Nombre Revisor Fiscal");

                //RuleFor(entity => entity.PrvRevFiscalTelefonoMovil)
                //    .NotNull().WithMessage(ErrorMessage.NullError)
                //    .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                //    .Length(1, 10).WithMessage(ErrorMessage.LengthError)
                //    .WithName("Teléfono Revisor Fiscal");

                /*** PrvSocios ***/
                // Validacion a nivel de lista
                //RuleFor(entity => entity.PrvSocios)
                //    .NotNull().WithMessage(ErrorMessage.NullError)
                //    .Must(prods => prods.Count >= 1).WithMessage(ErrorMessage.CountError)
                //    .WithName("Socio Proveedor");

                // Validacion a nivel de valor de lista
                //RuleForEach(entity => entity.PrvSocios)
                //    .NotNull().WithMessage(ErrorMessage.NullError)
                //    .SetValidator(new PrvSocioValidator()); // Llama a PrvSocioValidator

                RuleFor(entity => entity.PrvTelefono)
                    .NotNull().WithMessage(ErrorMessage.NullError)
                    .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                    .Length(1, 10).WithMessage(ErrorMessage.LengthError)
                    .WithName("Telefono Proveedor");
            });
        }
    }
}
