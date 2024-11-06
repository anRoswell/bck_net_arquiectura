using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class EmpresasSelectedCertEspConfiguration : IEntityTypeConfiguration<EmpresasSelectedCertEsp>
    {
        public void Configure(EntityTypeBuilder<EmpresasSelectedCertEsp> builder)
        {
            builder.HasKey(e => e.Id)
                    .HasName("PK__Empresas__E8FE1F334650DA16");

            builder.ToTable("EmpresasSelectedCertEsp", "cer");

            builder.Property(e => e.Id).HasColumnName("escIdEmpresasSelectedCertEsp");

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

            builder.Property(e => e.EscCodCertificadosEspeciales).HasColumnName("escCodCertificadosEspeciales");

            builder.Property(e => e.EscCodEmpresa).HasColumnName("escCodEmpresa");

            builder.Property(e => e.EscEstado)
                .IsRequired()
                .HasColumnName("escEstado")
                .HasDefaultValueSql("((1))");

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

            //builder.HasOne(d => d.EscCodCertificadosEspecialesNavigation)
            //    .WithMany(p => p.EmpresasSelectedCertEsps)
            //    .HasForeignKey(d => d.EscCodCertificadosEspeciales)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("FK_EmpresasSelectedCertEsp_CertificadosEspeciales");
        }
    }
}
