namespace Infrastructure.Data.Configurations
{
    using Core.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class gos_soporteConfiguration : IEntityTypeConfiguration<gos_soporte>
    {
        public void Configure(EntityTypeBuilder<gos_soporte> builder)
        {
            builder.ToTable("GOS_SOPORTES", "AIRE");

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasColumnName("ID_SOPORTE");

            builder.Property(p => p.id_tipo_soporte)
                   .HasColumnName("ID_TIPO_SOPORTE");

            builder.Property(p => p.nombre)
                   .HasColumnName("NOMBRE");

            builder.Property(p => p.peso)
                   .HasColumnName("PESO");

            builder.Property(p => p.formato)
                   .HasColumnName("FORMATO");

            builder.Property(p => p.ind_url_externo)
                   .HasColumnName("IND_URL_EXTERNO");

            builder.Property(p => p.url)
                   .HasColumnName("URL");

            builder.Property(p => p.id_usuario_registra)
                   .HasColumnName("ID_USUARIO_REGISTRA");

            builder.Property(p => p.fecha_registra)
                   .HasColumnName("FECHA_REGISTRA")
                   .HasDefaultValueSql("SYSDATE");
        }
    }
}