using Core.CustomEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class PathArchivosArtSerReqConfiguration : IEntityTypeConfiguration<PathArchivosArtSerReq>
    {
        public void Configure(EntityTypeBuilder<PathArchivosArtSerReq> builder)
        {
            builder.HasNoKey();
        }
    }
}
