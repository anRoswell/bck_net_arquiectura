using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class ReqDocumentosRequeridoConfiguration : IEntityTypeConfiguration<ReqDocumentosRequerido>
    {
        public void Configure(EntityTypeBuilder<ReqDocumentosRequerido> builder)
        {
            builder.HasKey(e => e.Id)
                    .HasName("PK__ReqDocum__0470C7F72B6661B6");

            builder.ToTable("ReqDocumentosRequeridos", "req");

            builder.Property(e => e.Id)
                .HasColumnName("rdrIdReqDocumentosRequeridos")
                .HasComment("Identificación del registro");

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

            builder.Property(e => e.RdrCodProveedor)
                .HasColumnName("rdrCodProveedor")
                .HasComment("Codigo del proveedor");

            builder.Property(e => e.RdrCodReqListDocumentos)
                .HasColumnName("rdrCodReqListDocumentos")
                .HasComment("Codigo de la tabla a relacionar");

            builder.Property(e => e.RdrEstadoDocumento)
                .HasColumnName("rdrEstadoDocumento")
                .HasDefaultValueSql("((1))")
                .HasComment("Estado del documento");

            builder.Property(e => e.RdrExtDocument)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("rdrExtDocument")
                .HasComment("Extesión del documento");

            builder.Property(e => e.RdrNameDocument)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("rdrNameDocument")
                .HasComment("Nombre del documento");

            builder.Property(e => e.RdrOriginalNameDocument)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("rdrOriginalNameDocument")
                .HasComment("Nombre original del documento");

            builder.Property(e => e.RdrSizeDocument)
                .HasColumnName("rdrSizeDocument")
                .HasComment("Tamaño del documento");

            builder.Property(e => e.RdrUrlDocument)
                .IsRequired()
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("rdrUrlDocument")
                .HasComment("Ruta del documento");

            builder.Property(e => e.RdrUrlRelDocument)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("rdrUrlRelDocument")
                .HasComment("Ruta relativa del documento");
        }
    }
}
