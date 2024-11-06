using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data.Configurations
{
    class ReqRtaCriteriosEvaluacionConfiguration : IEntityTypeConfiguration<ReqRtaCriteriosEvaluacion>
    {
        public void Configure(EntityTypeBuilder<ReqRtaCriteriosEvaluacion> builder)
        {
            builder.HasNoKey();
        }
    }
}
