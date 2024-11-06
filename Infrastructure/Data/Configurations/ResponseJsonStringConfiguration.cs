using Core.ModelResponse;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class ResponseJsonStringConfiguration : IEntityTypeConfiguration<ResponseJsonString>
    {
        public void Configure(EntityTypeBuilder<ResponseJsonString> builder)
        {
            builder.HasNoKey();
        }
    }
}
