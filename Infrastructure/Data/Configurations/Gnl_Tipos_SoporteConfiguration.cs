using Core.Entities;
using Core.Entities.SCRWebEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Linq;

namespace Infrastructure.Data.Configurations
{
    public class gnl_tipos_soporteConfiguration : IEntityTypeConfiguration<gnl_tipos_soporte>
    {
        public void Configure(EntityTypeBuilder<gnl_tipos_soporte> builder)
        {
            builder.ToTable("GNL_TIPOS_SOPORTE", "AIRE");

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasColumnName("ID_TIPO_SOPORTE");

            builder.HasKey(e => e.Codigox);
            builder.Property(e => e.Codigox).HasColumnName("CODIGO");

            var properties = typeof(gnl_tipos_soporte).GetProperties().Where(x => x.Name != "Id" && x.Name != "Codigox");
            foreach (var property in properties)
            {
                builder.Property(property.Name).HasColumnName(property.Name.ToUpper());
            }
        }
    }
}
