using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class LogErroresConfiguration : IEntityTypeConfiguration<LogErrores>
    {
        public void Configure(EntityTypeBuilder<LogErrores> builder)
        {
            builder.HasNoKey();

            builder.Property(e => e.Controlador)
                .HasMaxLength(200)
                .IsUnicode(false);

            builder.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            builder.Property(e => e.Descripcion)
                .IsRequired()
                .IsUnicode(false);

            builder.Property(e => e.Funcion)
                .HasMaxLength(200)
                .IsUnicode(false);

            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder.Property(e => e.Origen)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.Property(e => e.Usuario)
                .HasMaxLength(14)
                .IsUnicode(false);
        }
    }
}
