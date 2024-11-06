using Core.ModelResponse;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class ResponseActionUrlConfiguration : IEntityTypeConfiguration<ResponseActionUrl>
    {
        public void Configure(EntityTypeBuilder<ResponseActionUrl> builder)
        {
            builder.HasNoKey();
        }
    }
}
