using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class ReqAdjudicacionDetalleConfiguration : IEntityTypeConfiguration<ReqAdjudicacionDetalle>
    {
        public void Configure(EntityTypeBuilder<ReqAdjudicacionDetalle> builder)
        {
            builder.HasKey(e => e.Id)
                    .HasName("PK__ReqAdjud__9BF185DDAE6BA119");

            builder.ToTable("ReqAdjudicacionDetalle", "req");

            builder.Property(e => e.Id).HasColumnName("radteIdReqAdjudicacionDetalle");

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

            builder.Property(e => e.RadteCantidadAdjudicada)
                .HasColumnName("radteCantidadAdjudicada")
                .HasComment("Cantidad Adjudicada");

            builder.Property(e => e.RadteCodArtSerRequerido)
                .HasColumnName("radteCodArtSerRequerido")
                .HasComment("Codigo del articulo adjudicado");

            builder.Property(e => e.RadteCodProveedor)
                .HasColumnName("radteCodProveedor")
                .HasComment("Codigo del proveedor adjudicado");

            builder.Property(e => e.RadteCodReqAdjudicacion)
                .HasColumnName("radteCodReqAdjudicacion")
                .HasComment("Codigo de la Adjudicacion");
        }
    }
}
