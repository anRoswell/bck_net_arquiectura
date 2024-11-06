using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class ActividadesPendientesContratoConfiguration : IEntityTypeConfiguration<ActividadesPendientesContrato>
    {
        public void Configure(EntityTypeBuilder<ActividadesPendientesContrato> builder)
        {
            builder.HasNoKey();
        }
    }
}
