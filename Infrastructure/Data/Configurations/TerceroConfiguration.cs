using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class TerceroConfiguration : IEntityTypeConfiguration<Tercero>
    {
        public void Configure(EntityTypeBuilder<Tercero> builder)
        {
            builder.HasNoKey().ToView("Terceros", "prv");

            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("prvIdProveedores")
                .HasComment("Nit del proveedor");

            builder.Property(e => e.CodUser)
                .IsRequired()
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasDefaultValueSql("('7777777')")
                .HasComment("Cedula del usuario que crea el registro");

            builder.Property(e => e.CodUserUpdate)
                .IsRequired()
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasDefaultValueSql("('7777777')")
                .HasComment("Cedula del ultimo usuario que actualizó el registro");

            builder.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())")
                .HasComment("Fecha de creación del registro.");

            builder.Property(e => e.FechaRegistroUpdate)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())")
                .HasComment("Fecha de la ultima actualización del registro.");

            builder.Property(e => e.Info)
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasDefaultValueSql("('0|0|0')")
                .HasComment("En este campo almacenamos la direccion ip, navegador y version del navegador del cliente.");

            builder.Property(e => e.InfoUpdate)
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasDefaultValueSql("('0|0|0')")
                .HasComment("En este campo almacenamos la ultima direccion ip, navegador y version del navegador del cliente.");

            builder.Property(e => e.PrvCodBanco)
                .HasColumnName("prvCodBanco")
                .HasComment("Id del banco");

            builder.Property(e => e.PrvCodCiudad)
                .IsRequired()
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("prvCodCiudad")
                .HasComment("Codigo dane de la ciudad.");

            builder.Property(e => e.PrvCodTipoCuenta)
                .HasColumnName("prvCodTipoCuenta")
                .HasComment("Id del tipo de cuenta");

            builder.Property(e => e.PrvCodTipoProveeedor)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("prvCodTipoProveeedor")
                    .HasComment("Id tipo de proveedor seleccionado");

            builder.Property(e => e.PrvContacto)
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("prvContacto")
                .HasComment("Contactos del proveedor separados por comas (,)");

            builder.Property(e => e.PrvCpaCodCondicionesPago)
                .HasColumnName("prvCpaCodCondicionesPago")
                .HasComment("Id de la condicion de pago seleccionada (Cpa => Condiciones pago)");

            builder.Property(e => e.PrvCpaContadoCual)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("prvCpaContadoCual")
                .HasComment("Si el pago es de contado indica porcentaje de descuento");

            builder.Property(e => e.PrvCpaCual)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("prvCpaCual")
                .HasComment("En caso de seleccionar otro, describir cual");

            builder.Property(e => e.PrvDeclaramientoInhabilidadesInteres)
                .HasColumnName("prvDeclaramientoInhabilidadesInteres")
                .HasComment("Declaramiento");

            builder.Property(e => e.PrvDireccion)
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("prvDireccion")
                .HasComment("Dirección");

            builder.Property(e => e.PrvDtllesBanNroCuenta)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("prvDtllesBanNroCuenta")
                .HasComment("Numero de cuenta");

            builder.Property(e => e.PrvExperienciaSector)
                .HasColumnName("prvExperienciaSector")
                .HasComment("Consulta si tiene experiencia en el sector (Si/No)");

            builder.Property(e => e.PrvFechaEnvio)
                .HasColumnType("datetime")
                .HasColumnName("prvFechaEnvio");

            builder.Property(e => e.PrvMail)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("prvMail")
                .HasDefaultValueSql("('NoTieneCorreo')")
                .HasComment("Correo electrónico del proveedor");

            builder.Property(e => e.PrvMailAlterno)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("prvMailAlterno")
                    .HasComment("Correo Alterno Proveedor");

            builder.Property(e => e.PrvNit)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("prvNit");

            builder.Property(e => e.PrvNombreProveedor)
                .IsRequired()
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("prvNombreProveedor")
                .HasComment("Nombre o Razon Social del proveedor");

            builder.Property(e => e.PrvPoliticaTratamientoDatosPersonales)
                .HasColumnName("prvPoliticaTratamientoDatosPersonales")
                .HasComment("id de la refer");

            builder.Property(e => e.PrvProveeedor)
                .HasColumnName("prvProveeedor")
                .HasComment("Proveedor (Juridico - Natural)");

            builder.Property(e => e.PrvRteLegalApellido)
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("prvRteLegalApellido")
                .HasComment("Apellido del representante legal");

            builder.Property(e => e.PrvRteLegalCodCiudad)
                .IsRequired()
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("prvRteLegal_CodCiudad")
                .HasComment("Ciudad representante legal");

            builder.Property(e => e.PrvRteLegalDigVerificacion)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("prvRteLegalDigVerificacion");

            builder.Property(e => e.PrvRteLegalEmail)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("prvRteLegalEmail")
                .HasDefaultValueSql("('NoTieneCorreo')")
                .HasComment("Email del representante legal");

            builder.Property(e => e.PrvRteLegalIdentificacion)
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("prvRteLegalIdentificacion")
                .HasComment("Identificación del representante legal");

            builder.Property(e => e.PrvRteLegalNombre)
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("prvRteLegalNombre")
                .HasComment("Nombre del representante legal");

            builder.Property(e => e.PrvRteLegalSuplenteApellido)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("prvRteLegalSuplenteApellido")
                    .HasComment("Apellidos del representante legal suplente");

            builder.Property(e => e.PrvRteLegalSuplenteCodCiudad)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("prvRteLegalSuplente_CodCiudad")
                .HasComment("Código de ciudad del representante legal suplente");

            builder.Property(e => e.PrvRteLegalSuplenteDigVerificacion)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("prvRteLegalSuplenteDigVerificacion")
                .HasComment("Digito de verificación del representante legal suplente");

            builder.Property(e => e.PrvRteLegalSuplenteEmail)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("prvRteLegalSuplenteEmail")
                .HasComment("Telefono movil del representante legal suplente");

            builder.Property(e => e.PrvRteLegalSuplenteIdentificacion)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("prvRteLegalSuplenteIdentificacion")
                .HasComment("Identificación del representante legal suplente");

            builder.Property(e => e.PrvRteLegalSuplenteNombre)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("prvRteLegalSuplenteNombre")
                .HasComment("Nombre del representante legal suplente");

            builder.Property(e => e.PrvRteLegalSuplenteTelefonoMovil)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("prvRteLegalSuplenteTelefonoMovil")
                .HasComment("Telefono movil del representante legal suplente");

            builder.Property(e => e.PrvRteLegalTelefonoMovil)
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("prvRteLegalTelefonoMovil")
                .HasComment("Telefono movil del representante legal");

            builder.Property(e => e.PrvTelefono)
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("prvTelefono")
                .HasComment("Telefono del proveedor");

            builder.Property(e => e.PrvTipoProveedorCual)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("prvTipoProveedorCual");

            builder.HasIndex(e => e.PrvMail, "IX_Proveedores_Mail")
                    .IsUnique();

            builder.HasIndex(e => e.PrvNit, "IX_Proveedores_Nit")
                .IsUnique();

            builder.Property(e => e.PrvCodEstado)
                .HasColumnName("prvCodEstado")
                .HasDefaultValueSql("((1))");

            builder.Property(e => e.PrvDigitoVerificacion)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("prvDigitoVerificacion");

            builder.Property(e => e.PrvListaRestrictiva).HasColumnName("prvListaRestrictiva");

            builder.Property(e => e.PrvRevFiscalApellido)
                .HasMaxLength(200)
                .HasColumnName("prvRevFiscalApellido")
                .IsFixedLength(true);

            builder.Property(e => e.PrvRevFiscalCodCiudad)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("prvRevFiscal_CodCiudad");

            builder.Property(e => e.PrvRevFiscalDigVerificacion)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("prvRevFiscalDigVerificacion");

            builder.Property(e => e.PrvRevFiscalEmail)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("prvRevFiscalEmail");

            builder.Property(e => e.PrvRevFiscalIdentificacion)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("prvRevFiscalIdentificacion");

            builder.Property(e => e.PrvRevFiscalNombre)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("prvRevFiscalNombre");

            builder.Property(e => e.PrvRevFiscalTelefonoMovil)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("prvRevFiscalTelefonoMovil");

            builder.Property(e => e.PrvValidationNumber)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("prvValidationNumber")
                .HasComment("campo utilizado para cuando el proveedores al crearce es rechazado y necesita editar la informacion registrada.");

            builder.Property(e => e.PrvJustificacionInspektor)
                    .IsUnicode(false)
                    .HasColumnName("prvJustificacionInspektor")
                    .HasComment("Justificación de Aprobacion o Rechazo de Inspektor");

            builder.Property(e => e.PrvUsuarioApRzInspektor)
                    .HasColumnName("prvUsuarioApRzInspektor")
                    .HasComment("Usuario de Aprobacion o Rechazo de Inspektor");

            builder.Property(e => e.PrvJustificacionCorreccion)
                    .IsUnicode(false)
                    .HasColumnName("prvJustificacionCorreccion")
                    .HasComment("Justificación de correccion de proveedor");
        }
    }
}