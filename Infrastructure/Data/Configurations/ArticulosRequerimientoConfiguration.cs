using Core.CustomEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class ArticulosRequerimientoConfiguration : IEntityTypeConfiguration<ArticulosRequerimiento>
    {
        public void Configure(EntityTypeBuilder<ArticulosRequerimiento> builder)
        {
            builder.HasNoKey();
        }
    }
}
