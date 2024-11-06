using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class DocReqProveedorConfiguration : IEntityTypeConfiguration<DocReqProveedor>
    {
        public void Configure(EntityTypeBuilder<DocReqProveedor> builder)
        {
            builder.HasKey(e => e.Id);

            builder.ToTable("DocReqProveedor", "cont");

            builder.Property(e => e.Id)
                .HasColumnName("drpIdDocRequeridosProveedor")
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

            builder.Property(e => e.DrpCodContrato)
                .HasColumnName("drpCodContrato")
                .HasComment("Codigo de la tabla contrato");

            builder.Property(e => e.DrpCodDocumento)
                .HasColumnName("drpCodDocumento")
                .HasComment("Codigo de la tabla documentos");

            builder.Property(e => e.DrpCodPrvDocumento)
                .HasColumnName("drpCodPrvDocumento")
                .HasComment("Codigo de la documentos de proveedores");

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

            builder.Property(e => e.DrpAprobado)
                    .HasColumnName("drpAprobado")
                    .HasDefaultValueSql("((0))")
                    .HasComment("Si el documento es aprobado o no.");

            builder.Property(e => e.DrpObligatorio)
                   .HasColumnName("drpObligatorio")
                   .HasComment("Documento Obligatorio");

            builder.Property(e => e.DrpTipoVersion)
                .HasColumnName("drpTipoVersion")
                .HasComment("Versión del documento");
        }
    }
}
