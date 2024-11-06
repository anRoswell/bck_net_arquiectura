namespace Infrastructure.Data.Configurations
{
    using Core.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class sgr_auth_tokensConfiguration : IEntityTypeConfiguration<sgr_auth_tokens>
    {
        public void Configure(EntityTypeBuilder<sgr_auth_tokens> builder)
        {
            builder.ToTable("SGR_AUTH_TOKENS", "AIRE");

            builder.Ignore(p => p.Id).HasNoKey();

            builder.Property(p => p.auth_token)
                    .HasColumnName("AUTH_TOKEN");

            builder.Property(p => p.id_credencial)
                   .HasColumnName("ID_CREDENCIAL");

            builder.Property(p => p.fecha_registro)
                   .HasColumnName("FECHA_REGISTRO");

            builder.Property(p => p.fecha_vence)
                   .HasColumnName("FECHA_VENCE");

            builder.Property(p => p.ind_activo)
                   .HasColumnName("IND_ACTIVO");
        }
    }
}