using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class TipoMinutaConfiguration : IEntityTypeConfiguration<TipoMinuta>
    {
        public void Configure(EntityTypeBuilder<TipoMinuta> builder)
        {
            builder.ToTable("TipoMinuta", "cont");

            builder.Property(e => e.Id).HasComment("Id del registro");

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

            builder.Property(e => e.ExtDocument)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasComment("Extensión del documento");

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

            builder.Property(e => e.NameDocument)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasComment("Nombre del documento");

            builder.Property(e => e.Nombre)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasComment("nombre del tipo de minuta");

            builder.Property(e => e.OriginalNameDocument)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasComment("Nombre original del documento");

            builder.Property(e => e.SizeDocument).HasComment("Tamaño del documento");

            builder.Property(e => e.Url)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasComment("URLdel documento si es tipo URL");

            builder.Property(e => e.UrlDocument)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasComment("Ruta del docuemento");

            builder.Property(e => e.UrlRelDocument)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasComment("Ruta relativa del documento");
        }
    }
}