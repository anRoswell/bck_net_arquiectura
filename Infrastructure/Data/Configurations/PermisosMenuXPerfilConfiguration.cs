using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class PermisosMenuXPerfilConfiguration : IEntityTypeConfiguration<PermisosMenuXperfil>
    {
        public void Configure(EntityTypeBuilder<PermisosMenuXperfil> builder)
        {
            builder.ToTable("PermisosMenuXPerfil", "usr");

            builder.HasKey(e => e.Id)
                .HasName("PK__Permisos__3B44A164BDA8A7F4");

            builder.Property(e => e.Id)
            .HasColumnName("Pmp_IdPermisosMenuXPerfil");

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

            builder.Property(e => e.PmpAplCodAplicacion)
                .HasColumnName("Pmp_Apl_CodAplicacion")
                .HasComment("Codigo de aplicación");

            builder.Property(e => e.PmpBorrar)
                .HasColumnName("Pmp_Borrar")
                .HasDefaultValueSql("((1))")
                .HasComment("Indica si puede borrar");

            builder.Property(e => e.PmpConsultar)
                .HasColumnName("Pmp_Consultar")
                .HasDefaultValueSql("((1))")
                .HasComment("Indica si puede consultar");

            builder.Property(e => e.PmpEditar)
                .HasColumnName("Pmp_Editar")
                .HasDefaultValueSql("((1))")
                .HasComment("Indica si puede editar");

            builder.Property(e => e.PmpEjecutar)
                .HasColumnName("Pmp_Ejecutar")
                .HasDefaultValueSql("((1))")
                .HasComment("Indica si puede ejecutar accion");

            builder.Property(e => e.PmpGrabar)
                .HasColumnName("Pmp_Grabar")
                .HasDefaultValueSql("((1))")
                .HasComment("Indica si puede grabar");

            builder.Property(e => e.PmpLeer)
                .HasColumnName("Pmp_Leer")
                .HasDefaultValueSql("((1))")
                .HasComment("Indica si puede leer");

            builder.Property(e => e.PmpMenCodMenu)
                .HasColumnName("Pmp_Men_CodMenu")
                .HasComment("Codigo del menu");

            builder.Property(e => e.PmpPrfCodPerfil)
                .HasColumnName("Pmp_Prf_CodPerfil")
                .HasComment("Codigo de perfil");

            //builder.HasOne(d => d.PmpAplCodAplicacionNavigation)
            //    .WithMany(p => p.PermisosMenuXperfils)
            //    .HasForeignKey(d => d.PmpAplCodAplicacion)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("FK_PermisosMenuXPerfil_Aplicacion");

            //builder.HasOne(d => d.PmpMenCodMenuNavigation)
            //    .WithMany(p => p.PermisosMenuXperfils)
            //    .HasForeignKey(d => d.PmpMenCodMenu)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("FK_PermisosMenuXPerfil_Menu");
        }
    }
}
