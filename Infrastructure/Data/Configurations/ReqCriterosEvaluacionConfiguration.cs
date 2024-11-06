using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data.Configurations
{
    public class ReqCriterosEvaluacionConfiguration : IEntityTypeConfiguration<ReqCriterosEvaluacion>
    {
        public void Configure(EntityTypeBuilder<ReqCriterosEvaluacion> builder)
        {
            builder.HasNoKey();
        }
    }
}
