using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class DepartamentoConfiguration : IEntityTypeConfiguration<Departamento>
    {
        public void Configure(EntityTypeBuilder<Departamento> builder)
        {
            builder.HasKey(e => e.CodigoDepartamento);

            builder.ToTable("Departamentos", "GEO");

            builder.Property(e => e.CodigoDepartamento)
                .HasMaxLength(10)
                .IsUnicode(false);

            builder.Property(e => e.CodigoDane)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(e => e.Departamento1)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Departamento");
        }
    }
}
