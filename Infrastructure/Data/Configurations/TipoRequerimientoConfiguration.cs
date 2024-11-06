using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class TipoRequerimientoConfiguration : IEntityTypeConfiguration<TipoRequerimiento>
    {
        public void Configure(EntityTypeBuilder<TipoRequerimiento> builder)
        {
            builder.HasKey(e => e.Id)
                    .HasName("PK__TipoRequ__2C1E9B3B2004E31E");

            builder.ToTable("TipoRequerimientos", "req");

            builder.Property(e => e.Id).HasColumnName("rtreqIdTipoRequerimientos");

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

            builder.Property(e => e.RtreqDescripcion)
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("rtreqDescripcion");

            builder.Property(e => e.RtreqAbreviaturaErp)
                .IsRequired()
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("rtreqAbreviaturaErp")
                .HasComment("Abreviatura correspondiente en la ERP");

            builder.Property(e => e.RtreqEstado)
                .IsRequired()
                .HasColumnName("rtreqEstado")
                .HasDefaultValueSql("((1))");
        }
    }
}
