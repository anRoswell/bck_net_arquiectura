using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Linq;

namespace Infrastructure.Data.Configurations
{
    public class gnl_rutas_archivo_servidorConfiguration : IEntityTypeConfiguration<gnl_rutas_archivo_servidor>
    {
        public void Configure(EntityTypeBuilder<gnl_rutas_archivo_servidor> builder)
        {
            builder.ToTable("GNL_RUTAS_ARCHIVO_SERVIDOR", "AIRE");

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasColumnName("ID_RUTA_ARCHIVO_SERVIDOR");

            var properties = typeof(gnl_rutas_archivo_servidor).GetProperties().Where(x => x.Name != "Id");
            foreach (var property in properties)
            {
                builder.Property(property.Name).HasColumnName(property.Name.ToUpper());
            }
        }
    }
}
