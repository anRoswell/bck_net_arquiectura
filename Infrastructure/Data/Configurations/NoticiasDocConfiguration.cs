using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class NoticiasDocConfiguration : IEntityTypeConfiguration<NoticiasDoc>
    {
        public void Configure(EntityTypeBuilder<NoticiasDoc> builder)
        {
            builder.HasKey(e => e.Id)
                    .HasName("PK__Noticias__6499496BE1F1E889");

            builder.ToTable("NoticiasDoc", "noti");

            builder.Property(e => e.Id).HasColumnName("notdIdNoticiasDoc");

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

            builder.Property(e => e.NotdCodNoticias)
                .HasColumnName("notdCodNoticias")
                .HasComment("Id de la tabla a relacionar");

            builder.Property(e => e.NotdEstadoDocumento)
                .HasColumnName("notdEstadoDocumento")
                .HasDefaultValueSql("((1))")
                .HasComment("Estado del documento");

            builder.Property(e => e.NotdExtDocument)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("notdExtDocument");

            builder.Property(e => e.NotdNameDocument)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("notdNameDocument");

            builder.Property(e => e.NotdOriginalNameDocument)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("notdOriginalNameDocument");

            builder.Property(e => e.NotdSizeDocument).HasColumnName("notdSizeDocument");

            builder.Property(e => e.NotdUrlDocument)
                .IsRequired()
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("notdUrlDocument");

            builder.Property(e => e.NotdUrlRelDocument)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("notdUrlRelDocument");
        }
    }
}
