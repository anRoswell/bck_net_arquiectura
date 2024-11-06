using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class CiudadConfiguration : IEntityTypeConfiguration<Ciudade>
    {
        public void Configure(EntityTypeBuilder<Ciudade> builder)
        {
            builder.HasKey(e => e.CodigoCiudad);

            builder.ToTable("Ciudades", "GEO");

            builder.Property(e => e.CodigoCiudad)
                .HasMaxLength(5)
                .IsUnicode(false);

            builder.Property(e => e.Ciudad)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(e => e.CodDepartamento)
                .IsRequired()
                .HasMaxLength(10)
                .IsUnicode(false);

            //builder.HasOne(d => d.CodDepartamentoNavigation)
            //    .WithMany(p => p.Ciudades)
            //    .HasForeignKey(d => d.CodDepartamento)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("FK_Ciudades_Departamentos");
        }
    }
}
