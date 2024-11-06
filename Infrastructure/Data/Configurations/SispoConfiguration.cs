using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class EstadoCuentasXPorPagarConfiguration : IEntityTypeConfiguration<EstadoCuentasXPorPagar>
    {
        public void Configure(EntityTypeBuilder<EstadoCuentasXPorPagar> builder)
        {
            builder.HasNoKey();
        }
    }
}
