using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class AplicacionConfiguration : IEntityTypeConfiguration<Aplicacion>
    {
        public void Configure(EntityTypeBuilder<Aplicacion> builder)
        {
            builder.ToTable("Aplicacion", "conf");

            builder.HasKey(e => e.Id)
                .HasName("PK__Aplicaci__F86DB38470CA38CB");

            builder.Property(e => e.Id)
            .HasColumnName("Apl_IdAplicacion");

            builder.Property(e => e.AplDescripcion)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Apl_Descripcion")
                .HasComment("Descripcion de la aplicación");

            builder.Property(e => e.AplEstado)
                .IsRequired()
                .HasColumnName("Apl_Estado")
                .HasDefaultValueSql("((1))")
                .HasComment("Estado del registro");

            builder.Property(e => e.AplNombre)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Apl_Nombre")
                .HasComment("Nombre de la aplicación");

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
        }
    }
}
