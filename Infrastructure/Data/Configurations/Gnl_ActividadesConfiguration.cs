using Core.Entities;
using Core.Entities.SCRWebEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Linq;

namespace Infrastructure.Data.Configurations
{
    public class gnl_actividadesConfiguration : IEntityTypeConfiguration<gnl_actividades>
    {
        public void Configure(EntityTypeBuilder<gnl_actividades> builder)
        {
            builder.ToTable("GNL_ACTIVIDADES", "AIRE");

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasColumnName("ID_ACTIVIDAD");

            builder.HasKey(e => e.Codigox);
            builder.Property(e => e.Codigox).HasColumnName("PREFIJO");

            var properties = typeof(gnl_actividades).GetProperties().Where(x => x.Name != "Id" && x.Name != "Codigox");
            foreach (var property in properties)
            {
                builder.Property(property.Name).HasColumnName(property.Name.ToUpper());
            }
        }
    }
}
