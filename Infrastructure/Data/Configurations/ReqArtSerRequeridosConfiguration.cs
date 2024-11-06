using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class ReqArtSerRequeridosConfiguration : IEntityTypeConfiguration<ReqArtSerRequeridos>
    {
        public void Configure(EntityTypeBuilder<ReqArtSerRequeridos> builder)
        {
            //builder.HasNoKey();
            builder.HasKey(e => e.RasrIdReqArtSerRequeridos)
                    .HasName("PK__ReqArtSe__AF946C97ECE704F0");

            builder.ToTable("ReqArtSerRequeridos", "req");

            builder.Property(e => e.RasrIdReqArtSerRequeridos).HasColumnName("rasrIdReqArtSerRequeridos");

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

            builder.Property(e => e.RasrCantidadApot)
                .HasColumnName("rasrCantidadApot")
                .HasComment("Cantidad del articulo o servicio");

            builder.Property(e => e.RasrCodProdServ)
                .HasColumnName("rasrCodProdServ")
                .HasComment("Id de la categoría");

            builder.Property(e => e.RasrCodRequerimiento)
                .HasColumnName("rasrCodRequerimiento")
                .HasComment("Id del requerimiento");

            builder.Property(e => e.RasrCodigoArticuloApot)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("rasrCodigoArticuloApot")
                .HasDefaultValueSql("('ZZZZ')");

            builder.Property(e => e.RasrDepartamentoApot)
                .IsRequired()
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("rasrDepartamentoApot")
                .HasDefaultValueSql("('ZZZZ')");

            builder.Property(e => e.RasrDescripcionDteAlternaApot)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("rasrDescripcionDteAlternaApot");

            builder.Property(e => e.RasrDescripcionDteApot)
                .IsRequired()
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("rasrDescripcionDteApot")
                .HasDefaultValueSql("('ZZZZ')")
                .HasComment("Descripcion del articulo o servicio");

            builder.Property(e => e.RasrFechaCreacionApot)
                .HasColumnType("date")
                .HasColumnName("rasrFechaCreacionApot")
                .HasDefaultValueSql("(getdate())");

            builder.Property(e => e.RasrFichaTecnica)
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("rasrFichaTecnica")
                .HasComment("Url de la ficha tecnica");

            builder.Property(e => e.RasrItem)
                .HasColumnName("rasrItem")
                .HasComment("Nro del item");

            builder.Property(e => e.RasrLineaApot).HasColumnName("rasrLineaApot");

            builder.Property(e => e.RasrObservacionApot)
                .IsRequired()
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("rasrObservacionApot");

            builder.Property(e => e.RasrObservacionDteApot)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("rasrObservacionDteApot");

            builder.Property(e => e.RasrSolicitudApot).HasColumnName("rasrSolicitudApot");

            builder.Property(e => e.RasrTipoFichaTecnica).HasColumnName("rasrTipoFichaTecnica");

            builder.Property(e => e.RasrUnidadMedidaApot)
                .IsRequired()
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasColumnName("rasrUnidadMedidaApot")
                .HasDefaultValueSql("('ZZZZ')")
                .HasComment("Medida del articulo o servicio");

            //builder.HasOne(d => d.RasrCodRequerimientoNavigation)
            //    .WithMany(p => p.ReqArtSerRequeridos)
            //    .HasForeignKey(d => d.RasrCodRequerimiento)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("FK_ReqArtSerRequeridos_Requerimientos");
        }
    }
}
