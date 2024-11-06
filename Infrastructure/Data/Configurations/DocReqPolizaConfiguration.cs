using Core.Entities;
using ImageMagick;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    class DocReqPolizaConfiguration : IEntityTypeConfiguration<DocReqPoliza>
    {
        public void Configure(EntityTypeBuilder<DocReqPoliza> builder)
        {
            builder.HasKey(e => e.Id);

            builder.ToTable("DocReqPoliza", "cont");

            builder.Property(e => e.Id)
                .HasColumnName("drpoIdDocReqPoliza")
                .HasComment("Identificación del registro");

            builder.Property(e => e.CodArchivo).HasComment("Id de archivo del Documento");

            builder.Property(e => e.CodUser)
                .IsRequired()
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasDefaultValueSql("('7777777')");

            builder.Property(e => e.CodUserUpdate)
                .IsRequired()
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasDefaultValueSql("('7777777')");

            builder.Property(e => e.DrpoAprobada).HasColumnName("drpoAprobada");

            builder.Property(e => e.DrpoCobertura)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("drpoCobertura")
                    .HasDefaultValueSql("((1))")
                    .HasComment("Si el documento es requerido o no");

            builder.Property(e => e.DrpoCodContrato)
                .HasColumnName("drpoCodContrato")
                .HasComment("Codigo de la tabla contrato");

            builder.Property(e => e.DrpoEstado)
                .HasColumnName("drpoEstado")
                .HasComment("Estado del registro");

            builder.Property(e => e.DrpoExpedida).HasColumnName("drpoExpedida");

            builder.Property(e => e.DrpoTipoPoliza)
                .IsRequired()
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("drpoTipoPoliza")
                .HasComment("Titulo del documento");

            builder.Property(e => e.DrpoVigencia)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("drpoVigencia");

            builder.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            builder.Property(e => e.FechaRegistroUpdate)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            builder.Property(e => e.Info)
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasDefaultValueSql("('0|0|0')");

            builder.Property(e => e.InfoUpdate)
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasDefaultValueSql("('0|0|0')");

            builder.Property(e => e.DrpoCodTipoDocumento)
                    .HasColumnName("drpoCodTipoDocumento")
                    .HasComment("Tipo de archivo de la poliza");

            builder.Property(e => e.DrpoEsRenovada)
                   .HasColumnName("drpoEsRenovada")
                   .HasComment("Si la poliza es renovada");

            builder.Property(e => e.DrpoFechaEmision)
                    .HasColumnType("datetime")
                    .HasColumnName("drpoFechaEmision");

            builder.Property(e => e.DrpoFechaVencimiento)
                .HasColumnType("datetime")
                .HasColumnName("drpoFechaVencimiento");
        }
    }
}
