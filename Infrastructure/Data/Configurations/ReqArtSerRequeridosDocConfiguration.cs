using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class ReqArtSerRequeridosDocConfiguration : IEntityTypeConfiguration<ReqArtSerRequeridosDoc>
    {
        public void Configure(EntityTypeBuilder<ReqArtSerRequeridosDoc> builder)
        {
            builder.HasKey(e => e.RasrdIdReqArtSerRequeridosDoc)
                    .HasName("PK__ReqArtSerRequeridosDoc");

            builder.ToTable("ReqArtSerRequeridosDoc", "req");

            builder.Property(e => e.RasrdIdReqArtSerRequeridosDoc).HasColumnName("rasrdIdReqArtSerRequeridosDoc");

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

            builder.Property(e => e.RasrdCodReqArtSerRequeridos)
                .HasColumnName("rasrdCodReqArtSerRequeridos")
                .HasComment("Id de la tabla a relacionar");

            builder.Property(e => e.RasrdCodTipoDocArtSerRequeridos)
                .HasColumnName("rasrdCodTipoDocArtSerRequeridos")
                .HasComment("Codigo tipo de Documento");

            builder.Property(e => e.RasrdEstadoDocumento)
                .HasColumnName("rasrdEstadoDocumento")
                .HasDefaultValueSql("((1))")
                .HasComment("Estado del documento");

            builder.Property(e => e.RasrdExtDocument)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("rasrdExtDocument");

            builder.Property(e => e.RasrdNameDocument)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("rasrdNameDocument");

            builder.Property(e => e.RasrdOriginalNameDocument)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("rasrdOriginalNameDocument");

            builder.Property(e => e.RasrdSizeDocument).HasColumnName("rasrdSizeDocument");

            builder.Property(e => e.RasrdUrlDocument)
                .IsRequired()
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("rasrdUrlDocument");

            builder.Property(e => e.RasrdUrlRelDocument)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("rasrdUrlRelDocument");

            //builder.HasOne(d => d.RasrdCodTipoDocArtSerRequeridosNavigation)
            //    .WithMany(p => p.ReqArtSerRequeridosDocs)
            //    .HasForeignKey(d => d.RasrdCodTipoDocArtSerRequeridos)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("FK_ReqArtSerRequeridosDoc_ReqTipoDocArtSerRequeridos");
        }
    }
}
