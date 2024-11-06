using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class PathsPortalConfiguration : IEntityTypeConfiguration<PathsPortal>
    {
        public void Configure(EntityTypeBuilder<PathsPortal> builder)
        {
            builder.ToTable("PathsPortal", "conf");

            builder.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID")
                .HasComment("Estos codigos no pueden ser modificados porque conectan con la tabla conf.ArchivosAdjuntos campos CodPathsInterno y CodPathsWeb");

            builder.Property(e => e.Clasificar)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValueSql("('Sin Clasificar')")
                .HasComment("clasificar si por ejemplo es un path para un formulario en específico.");

            builder.Property(e => e.CodUser)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasDefaultValueSql("((7777777))")
                .HasComment("Cedula del usuario que crea el registro");

            builder.Property(e => e.CodUserUpdate)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasDefaultValueSql("((7777777))")
                .HasComment("Cedula del usuario que actualiza el registro");

            builder.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())")
                .HasComment("Fecha de creación del registro.");

            builder.Property(e => e.FechaRegistroUpdate)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())")
                .HasComment("Fecha de Actualización del registro.");

            builder.Property(e => e.Observaciones)
                .IsUnicode(false)
                .HasDefaultValueSql("('Sin Observacion')")
                .HasComment("Observación de la URL");

            builder.Property(e => e.Path)
                .IsRequired()
                .HasMaxLength(300)
                .HasComment("URL del servicio");

            builder.Property(e => e.Tipo)
                .IsRequired()
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasDefaultValueSql("('Red')")
                .HasComment("Indicar si es un link de red o web");
        }
    }
}
