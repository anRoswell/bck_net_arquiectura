using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data.Configurations
{
    public class ReqPolizasSeguroConfiguration : IEntityTypeConfiguration<ReqPolizasSeguro>
    {
        public void Configure(EntityTypeBuilder<ReqPolizasSeguro> builder)
        {
            builder.HasNoKey();
        }
    }
}
