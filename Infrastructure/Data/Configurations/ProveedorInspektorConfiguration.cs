using Core.CustomEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class ProveedorInspektorConfiguration : IEntityTypeConfiguration<ProveedorInspektor>
    {
        public void Configure(EntityTypeBuilder<ProveedorInspektor> builder)
        {
            builder.HasNoKey();
        }
    }
}
