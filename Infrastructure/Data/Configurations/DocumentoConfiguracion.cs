using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class DocumentoConfiguracion : IEntityTypeConfiguration<Documento>
    {
        public void Configure(EntityTypeBuilder<Documento> builder)
        {
            builder.HasKey(e => e.Id)
                    .HasName("PK__Document__073A4CEDE0596C1F");

            builder.ToTable("Documentos", "par");

            builder.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("cocIdDocumentos");

            builder.Property(e => e.CocCodModuloDocumentos)
                .HasColumnName("cocCodModuloDocumentos")
                .HasComment("Codigo modulo documento");

            builder.Property(e => e.CocDescripcion)
                .IsRequired()
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("cocDescripcion")
                .HasComment("Descripcion a mostrar como texto informativo");

            builder.Property(e => e.CocEstado)
                .IsRequired()
                .HasColumnName("cocEstado")
                .HasDefaultValueSql("((1))")
                .HasComment("Indica el estado del registro");

            builder.Property(e => e.CocNombreDocumento)
                .IsRequired()
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("cocNombreDocumento")
                .HasComment("Descripción del registro");

            builder.Property(e => e.CocVigencia)
                .HasColumnName("cocVigencia")
                .HasComment("Indica si el documento tiene vigencia");

            builder.Property(e => e.CocVigenciaMaxima)
                .HasColumnName("cocVigenciaMaxima")
                .HasComment("Indica vigencia maxima, en dias");

            builder.Property(e => e.CoclimitLoad)
                .HasColumnName("coclimitLoad")
                .HasComment("Indica si este documento tiene limite de cargas");

            builder.Property(e => e.Cocrequiered)
                .HasColumnName("cocrequiered")
                .HasComment("Indica si el documento es requerido");

            builder.Property(e => e.CodUser)
                .IsRequired()
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasDefaultValueSql("('7777777')")
                .HasComment("Cedula del usuario que crea el registro");

            builder.Property(e => e.CodUserUpdate)
                .IsRequired()
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasDefaultValueSql("('7777777')")
                .HasComment("Cedula del ultimo usuario que actualizó el registro");

            builder.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())")
                .HasComment("Fecha de creación del registro.");

            builder.Property(e => e.FechaRegistroUpdate)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())")
                .HasComment("Fecha de la ultima actualización del registro.");

            builder.Property(e => e.Info)
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasDefaultValueSql("('0|0|0')")
                .HasComment("En este campo almacenamos la direccion ip, navegador y version del navegador del cliente.");

            builder.Property(e => e.InfoUpdate)
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasDefaultValueSql("('0|0|0')")
                .HasComment("En este campo almacenamos la ultima direccion ip, navegador y version del navegador del cliente.");
        }
    }
}
