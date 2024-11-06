using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class ParametrosInicialesConfiguration : IEntityTypeConfiguration<ParametrosIniciales>
    {
        public void Configure(EntityTypeBuilder<ParametrosIniciales> builder)
        {
            builder.HasNoKey();
        }
    }
}
