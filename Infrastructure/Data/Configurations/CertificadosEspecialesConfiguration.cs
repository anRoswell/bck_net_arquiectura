using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class CertificadosEspecialesConfiguration : IEntityTypeConfiguration<CertificadosEspeciale>
    {
        public void Configure(EntityTypeBuilder<CertificadosEspeciale> builder)
        {
            builder.HasKey(e => e.Id)
                    .HasName("PK__Certific__F0FF115A2A8D5879");

            builder.ToTable("CertificadosEspeciales", "cer");

            builder.Property(e => e.Id).HasColumnName("cerIdCertificadosEspeciales");

            builder.Property(e => e.CerCodProveedor).HasColumnName("cerCodProveedor");

            builder.Property(e => e.CerCodTipoCertificado)
                .HasColumnName("cerCodTipoCertificado")
                .HasComment("Individual por empresa, corporativo");

            builder.Property(e => e.CerDescripcion)
                .IsRequired()
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("cerDescripcion")
                .HasComment("Descripción de la solicitud");

            builder.Property(e => e.CerDestinatario)
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("cerDestinatario")
                .HasComment("Destinatario");

            builder.Property(e => e.CerEstado)
                .HasColumnName("cerEstado")
                .HasDefaultValueSql("((1))")
                .HasComment("Estado del registro");

            builder.Property(e => e.CerIncluirGarantia)
                .HasColumnName("cerIncluirGarantia")
                .HasComment("Incluir garantias?");

            builder.Property(e => e.CerHtmlPdf).HasColumnName("cerHtmlPdf");

            builder.Property(e => e.CerPeriodo)
                .HasColumnName("cerPeriodo")
                .HasComment("Id del periodo del certificado a descargar");

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
