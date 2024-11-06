using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class DocReqProveedorOtroConfiguration : IEntityTypeConfiguration<DocReqProveedorOtro>
    {
        public void Configure(EntityTypeBuilder<DocReqProveedorOtro> builder)
        {
            builder.HasKey(e => e.Id)
                    .HasName("PK__DocReqPr__27966A10EA6B8F5E");

            builder.ToTable("DocReqProveedorOtros", "cont");

            builder.Property(e => e.Id)
                .HasColumnName("drpoIdDocReqProveedorOtros")
                .HasComment("Identificación del registro");

            builder.Property(e => e.CodArchivo).HasComment("Id de archivo del Documento");

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

            builder.Property(e => e.DrpoCodContrato)
                .HasColumnName("drpoCodContrato")
                .HasComment("Codigo del contrato");

            builder.Property(e => e.DrpoCodDocumento)
                .HasColumnName("drpoCodDocumento")
                .HasComment("Codigo del documento");

            builder.Property(e => e.DrpoNombreDocumento)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("drpoNombreDocumento")
                .HasComment("Nombre del documento 'Otros'");

            builder.Property(e => e.DrpoObligatorio)
                .IsRequired()
                .HasColumnName("drpoObligatorio")
                .HasDefaultValueSql("((1))")
                .HasComment("Estado del registro");

            builder.Property(e => e.DrpoVigencia)
                .HasColumnName("drpoVigencia")
                .HasComment("Id del documento registrado para cargue en el requerimiento");

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
