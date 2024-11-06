using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class ReqListDocumentosConfiguration : IEntityTypeConfiguration<ReqListDocumentos>
    {
        public void Configure(EntityTypeBuilder<ReqListDocumentos> builder)
        {
            builder.HasKey(e => e.Id)
                    .HasName("PK__ReqListD__C18E147F707FF302");

            builder.ToTable("ReqListDocumentos", "req");

            builder.Property(e => e.Id).HasColumnName("rldocIdReqListDocumentos");

            builder.Property(e => e.CodDocumento).HasComment("Id del requerimiento");

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

            builder.Property(e => e.RlCodRequerimiento)
                .HasColumnName("rlCodRequerimiento")
                .HasComment("Id del requerimiento");

            builder.Property(e => e.RldocObligatorio)
                .IsRequired()
                .HasColumnName("rldocObligatorio")
                .HasDefaultValueSql("((1))")
                .HasComment("Estado del registro");

            builder.Property(e => e.RldocVigencia)
                .HasColumnName("rldocVigencia")
                .HasComment("Id del documento registrado para cargue en el requerimiento");
        }
    }
}
