using Core.CustomEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class AdjudicacionOrdenesConfiguration : IEntityTypeConfiguration<AdjudicacionOrdenes>
    {
        public void Configure(EntityTypeBuilder<AdjudicacionOrdenes> builder)
        {
            builder.HasNoKey();
        }
    }
}
