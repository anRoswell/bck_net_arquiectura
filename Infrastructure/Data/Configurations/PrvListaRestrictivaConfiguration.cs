using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class PrvListaRestrictivaConfiguration : IEntityTypeConfiguration<PrvListaRestrictiva>
    {
        public void Configure(EntityTypeBuilder<PrvListaRestrictiva> builder)
        {
            builder.HasKey(e => e.PrvPlrIdPrvListaRestrictiva)
                    .HasName("PK__PrvLista__19C7BF369011B286");

            builder.ToTable("PrvListaRestrictiva", "prv");

            builder.Property(e => e.PrvPlrIdPrvListaRestrictiva).HasColumnName("prvPlrIdPrvListaRestrictiva");

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

            builder.Property(e => e.PrvPlrCodPrvTipoUsListaRtva)
                .HasColumnName("prvPlrCodPrvTipoUsListaRtva")
                .HasComment("Codigo tipo de usuario a consultar en inspektor");

            builder.Property(e => e.PrvPlrCodTraza)
                .HasColumnName("prvPlrCodTraza")
                .HasComment("Id del proveedor creado");

            builder.Property(e => e.PrvPlrIdentificacion)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("prvPlrIdentificacion")
                .HasComment("Identificacion del proveedor");

            builder.Property(e => e.PrvPlrLista)
                .IsRequired()
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasColumnName("prvPlrLista")
                .HasComment("Indica si la firma fue digital");

            builder.Property(e => e.PrvPlrNoConsulta)
                .HasColumnName("prvPlrNoConsulta")
                .HasComment("Numero consecutivo consulta");

            builder.Property(e => e.PrvPlrNombreProveedor)
                .IsRequired()
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("prvPlrNombreProveedor")
                .HasComment("Nombre del proveedor");

            builder.Property(e => e.PrvPlrNumeroTipoLista)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("prvPlrNumeroTipoLista")
                .HasComment("Lista restrictiva");

            builder.Property(e => e.PrvPlrPrioridad)
                .IsRequired()
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("prvPlrPrioridad")
                .HasComment("Estado en el q se encuentra el registro");

            builder.Property(e => e.PrvPlrTipoDocumento)
                .IsRequired()
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasColumnName("prvPlrTipoDocumento")
                .HasComment("Indica si el documento se encuentra firmado electronicamente por las personas correspondientes");
        }
    }
}
