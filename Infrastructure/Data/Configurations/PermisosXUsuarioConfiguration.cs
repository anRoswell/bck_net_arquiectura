using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data.Configurations
{
    class PermisosXUsuarioConfiguration : IEntityTypeConfiguration<PermisosXUsuario>
    {
        public void Configure(EntityTypeBuilder<PermisosXUsuario> builder)
        {
            builder.HasNoKey();
        }
    }
}
