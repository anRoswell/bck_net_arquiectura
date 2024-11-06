using Core.CustomEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class ParticipantesReqConfiguration : IEntityTypeConfiguration<ParticipantesReq>
    {
        public void Configure(EntityTypeBuilder<ParticipantesReq> builder)
        {
            builder.HasNoKey();
        }
    }
}
