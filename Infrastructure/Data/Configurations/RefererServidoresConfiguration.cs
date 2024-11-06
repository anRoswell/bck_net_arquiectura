using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class RefererServidoresConfiguration : IEntityTypeConfiguration<RefererServidore>
    {
        public void Configure(EntityTypeBuilder<RefererServidore> builder)
        {
            builder.ToTable("RefererServidores", "conf");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .HasColumnName("IdRefererServidores");

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

            builder.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())")
                .HasComment("Fecha de creación del registro.");

            builder.Property(e => e.FechaRegistroUpdate)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())")
                .HasComment("Fecha de Actualización del registro.");

            builder.Property(e => e.RequestHeadersReferer)
                .IsRequired()
                .HasMaxLength(1000);
        }
    }
}
