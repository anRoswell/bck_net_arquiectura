using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class IdentidadesLocaleConfiguration : IEntityTypeConfiguration<IdentidadesLocale>
    {
        public void Configure(EntityTypeBuilder<IdentidadesLocale> builder)
        {
            builder.HasKey(e => e.IdIdentidad);

            builder.ToTable("IdentidadesLocales", "par");

            builder.HasIndex(e => new { e.IdIdentidad, e.Consecutivo }, "UK_IdentidadesLocales")
                .IsUnique();

            builder.Property(e => e.CodUser)
                .IsRequired()
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasDefaultValueSql("((7777777))")
                .HasComment("Cedula del usuario que crea el registro");

            builder.Property(e => e.CodUserUpdate)
                .IsRequired()
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasDefaultValueSql("((7777777))")
                .HasComment("Cedula del usuario que actualiza el registro");

            builder.Property(e => e.DescripcionOpcional)
                .IsRequired()
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasDefaultValueSql("('Sin Descripcion')");

            builder.Property(e => e.Estado)
                .IsRequired()
                .HasDefaultValueSql("((1))");

            builder.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())")
                .HasComment("Fecha de creación del registro.");

            builder.Property(e => e.FechaRegistroUpdate)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())")
                .HasComment("Fecha de Actualización del registro.");

            builder.Property(e => e.IdGrupo)
                .IsRequired()
                .HasMaxLength(5)
                .IsUnicode(false);

            builder.Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(300)
                .IsUnicode(false);
        }
    }
}
