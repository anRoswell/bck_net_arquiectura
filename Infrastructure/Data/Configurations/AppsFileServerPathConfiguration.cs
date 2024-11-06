using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class AppsFileServerPathConfiguration : IEntityTypeConfiguration<AppsFileServerPath>
    {
        public void Configure(EntityTypeBuilder<AppsFileServerPath> builder)
        {
            builder.ToTable("AppsFileServerPaths", "conf");

            builder.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");

            builder.Property(e => e.Estado).HasComment("Estado del fileServer");

            builder.Property(e => e.Observacion)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasComment("Descripcion File Server");

            builder.Property(e => e.PathRed)
                .IsRequired()
                .HasMaxLength(255)
                .HasDefaultValueSql("((1))")
                .HasComment("Ruta principal del File Server de la app");

            builder.Property(e => e.PathRedArchivo)
                .IsRequired()
                .HasMaxLength(255)
                .HasComment("Ruta Relativa de Red para Archivos");

            builder.Property(e => e.PathWeb)
                .IsRequired()
                .HasMaxLength(255)
                .HasComment("Ruta Web principal de la app");

            builder.Property(e => e.PathWebArchivo)
                .IsRequired()
                .HasMaxLength(255)
                .HasComment("Ruta Relativa Web para Archivos");

            //builder.HasOne(d => d.CodAplicacionesNavigation)
            //    .WithMany(p => p.AppsFileServerPaths)
            //    .HasForeignKey(d => d.CodAplicaciones)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("FK_AppsFileServerPaths_Aplicacion");
        }
    }
}
