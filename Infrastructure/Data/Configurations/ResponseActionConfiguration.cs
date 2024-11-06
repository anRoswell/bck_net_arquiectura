using Core.ModelResponse;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class ResponseActionConfiguration : IEntityTypeConfiguration<ResponseAction>
    {
        public void Configure(EntityTypeBuilder<ResponseAction> builder)
        {
            builder.HasNoKey();
        }
    }
}
