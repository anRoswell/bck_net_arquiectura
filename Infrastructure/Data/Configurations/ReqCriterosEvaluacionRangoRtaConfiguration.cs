using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class ReqCriterosEvaluacionRangoRtaConfiguration : IEntityTypeConfiguration<ReqCriterosEvaluacionRangoRta>
    {
        public void Configure(EntityTypeBuilder<ReqCriterosEvaluacionRangoRta> builder)
        {
            builder.HasNoKey();
        }
    }
}
