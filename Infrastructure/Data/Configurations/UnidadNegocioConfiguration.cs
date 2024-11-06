using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class UnidadNegocioConfiguration : IEntityTypeConfiguration<UnidadNegocio>
    {
        public void Configure(EntityTypeBuilder<UnidadNegocio> builder)
        {
            builder.HasKey(e => e.Id)
                    .HasName("PK__UnidadNe__F19913C332C3636C");

            builder.ToTable("UnidadNegocio", "cont");

            builder.Property(e => e.Id)
                .HasColumnName("idUnidadNegocio")
                .HasComment("Identificación del registro");

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

            builder.Property(e => e.UnDescripcion)
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("unDescripcion")
                .HasComment("Descripción de la unidad de negocio");

            builder.Property(e => e.UnEstado)
                .IsRequired()
                .HasColumnName("unEstado")
                .HasDefaultValueSql("((1))")
                .HasComment("Estado del registro");
        }
    }
}
