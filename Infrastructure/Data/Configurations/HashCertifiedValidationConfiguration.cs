using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class HashCertifiedValidationConfiguration : IEntityTypeConfiguration<HashCertifiedValidation>
    {
        public void Configure(EntityTypeBuilder<HashCertifiedValidation> builder)
        {
            builder.HasKey(e => e.IdHashCertifiedValidation)
                    .HasName("PK_HashCertifiedValidation_1");

            builder.ToTable("HashCertifiedValidation", "cer");

            builder.HasIndex(e => e.CodigoHash, "Unique_CodigoHash")
                .IsUnique();

            builder.Property(e => e.CertifiedHtml)
                .HasColumnType("text")
                .HasComment("Html del certificado con que se generó el token");

            builder.Property(e => e.CodigoHash)
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasComment("Token");

            builder.Property(e => e.Estado).HasComment("Estado del token");

            builder.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())")
                .HasComment("Fecha del registro");
        }
    }
}
