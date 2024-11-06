using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Linq;

namespace Infrastructure.Data.Configurations
{
    public class gnl_soportesConfiguration : IEntityTypeConfiguration<gnl_soportes>
    {
        

        public void Configure(EntityTypeBuilder<gnl_soportes> builder)
        {
            builder.ToTable("GNL_SOPORTES", "AIRE");

            builder.HasKey(e => e.Id);            
            builder.Property(e => e.Id).HasColumnName("ID_SOPORTE");
            

            //var properties = typeof(gnl_soportes).GetProperties().Where(x => x.Name != "Id");
            //foreach (var property in properties)
            //{
            //    builder.Property(property.Name).HasColumnName(property.Name.ToUpper());
            //}

            //builder.Property(e => e.ActionMethod)
            //    .HasMaxLength(50)
            //    .IsUnicode(false)
            //    .HasComment("Tipo de Metodo ejecutado");

            //builder.Property(e => e.CodUser)
            //    .IsRequired()
            //    .HasMaxLength(15)
            //    .IsUnicode(false)
            //    .HasComment("Cedula del usuario que crea el registro");

            //builder.Property(e => e.ControllerName)
            //    .HasMaxLength(100)
            //    .IsUnicode(false)
            //    .HasComment("Nombre del controlador ejecutado");

            //builder.Property(e => e.FechaRegistro)
            //    .HasColumnType("datetime")
            //    .HasDefaultValueSql("(getdate())")
            //    .HasComment("Fecha de creación del registro.");

            //builder.Property(e => e.Grupo)
            //    .HasMaxLength(100)
            //    .IsUnicode(false)
            //    .HasComment("Grupo Empresarial al que se le realiza la peticion");

            //builder.Property(e => e.RequestHeadersReferer)
            //    .IsRequired()
            //    .HasMaxLength(1000)
            //    .HasDefaultValueSql("(N'NoSeDetectoReferer')");

            //builder.Property(e => e.Token)
            //    .HasMaxLength(1000)
            //    .HasComment("Token con que se consume la API");
        }
    }
}
