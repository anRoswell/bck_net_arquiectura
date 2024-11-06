using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuario", "usr");

            builder.HasKey(e => e.Id)
                .HasName("PK__Usuario__B117F7DEA731BE29");

            builder.Property(e => e.Id)
            .HasColumnName("Usr_IdUsuario");

            //builder.HasIndex(e => e.UsrCedula, "IX_CedulaUsuario")
            //        .IsUnique();

            //builder.HasIndex(e => e.UsrEmail, "IX_Email")
            //    .IsUnique();

            builder.Property(e => e.CodArchivo)
                .IsRequired()
                .HasMaxLength(450)
                .HasComment("Se carga una imagen con el logotipo de la empresa, esta debe de ser de maximo 128x128. Conecta con la tabla maestra de archivos.");

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

            builder.Property(e => e.UsrApellidos)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Usr_Apellidos")
                .HasComment("Apellidos del usuario");

            builder.Property(e => e.UsrCedula)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("Usr_Cedula")
                .HasComment("Identificacion del usuario");

            builder.Property(e => e.UsrEmail)
                .IsRequired()
                .HasMaxLength(256)
                .HasColumnName("Usr_Email")
                .HasComment("Email del usuario");

            builder.Property(e => e.UsrEmpresaProceso)
                .HasColumnName("Usr_EmpresaProceso")
                .HasComment("Empresa seleccionada por el usuario como predeterminada");

            builder.Property(e => e.UsrEstado)
                .IsRequired()
                .HasColumnName("Usr_Estado")
                .HasComment("Estado del registro");

            builder.Property(e => e.UsrForcePasswordChange)
                .IsRequired()
                .HasColumnName("Usr_ForcePasswordChange")
                .HasDefaultValueSql("((1))")
                .HasComment("Indica si se debe forzar el cambio de contraseña ");

            builder.Property(e => e.UsrNombres)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Usr_Nombres")
                .HasComment("Nombres del usuario");

            builder.Property(e => e.UsrPassword)
                .IsRequired()
                .HasMaxLength(150)
                .HasColumnName("Usr_Password")
                .HasComment("Contraseña del usuario");

            builder.Property(e => e.UsrTmpSuspendido)
                .HasColumnName("Usr_TmpSuspendido")
                .HasComment("Indica si el usuario esta suspendido por cambiar informacion como correo o representante legal");

            builder.Property(e => e.UsrTusrCodTipoUsuario)
                .HasColumnName("Usr_Tusr_CodTipoUsuario")
                .HasComment("Codigo de tipo usuario");

            builder.Property(e => e.UsrUltimaFechaPassword)
                    .HasColumnType("date")
                    .HasColumnName("Usr_UltimaFechaPassword");

            //builder.HasOne(d => d.UsrTusrCodTipoUsuarioNavigation)
            //    .WithMany(p => p.Usuarios)
            //    .HasForeignKey(d => d.UsrTusrCodTipoUsuario)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("FK_Usuario_TipoUsuario");
        }
    }
}
