using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data.Configurations
{
    public class OrdenesMaestrasConfiguration : IEntityTypeConfiguration<OrdenesMaestras>
    {
        public void Configure(EntityTypeBuilder<OrdenesMaestras> builder)
        {
            builder.HasNoKey();
        }
    }
}
