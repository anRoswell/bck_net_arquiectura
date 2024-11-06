using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data.Configurations
{
    public class PrvDocumentosConfiguration : IEntityTypeConfiguration<PrvDocumento>
    {
        public void Configure(EntityTypeBuilder<PrvDocumento> builder)
        {
            builder.HasKey(e => e.Id)
                    .HasName("PK__PrvDocum__85852292807EBCE5");

            builder.ToTable("PrvDocumentos", "prv");

            builder.Property(e => e.Id).HasColumnName("prvdIdPrvDocumentos");

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

            builder.Property(e => e.PrvdCodDocumento)
                .HasColumnName("prvdCodDocumento")
                .HasComment("id del documento a relacionar");

            builder.Property(e => e.PrvdCodProveedor)
                .HasColumnName("prvdCodProveedor")
                .HasComment("Id de la tabla a relacionar");

            builder.Property(e => e.PrvdEstadoDocumento)
                .HasColumnName("prvdEstadoDocumento")
                .HasDefaultValueSql("((1))")
                .HasComment("Estado del documento");

            builder.Property(e => e.PrvdExpedicion)
                .HasColumnType("datetime")
                .HasColumnName("prvdExpedicion")
                .HasComment("Fecha de vencimiento del documento, si aplica");

            builder.Property(e => e.PrvdExtDocument)
                .IsRequired()
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("prvdExtDocument");

            builder.Property(e => e.PrvdNameDocument)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("prvdNameDocument");

            builder.Property(e => e.PrvdOriginalNameDocument)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("prvdOriginalNameDocument");

            builder.Property(e => e.PrvdSendNotification)
                .HasColumnName("prvdSendNotification")
                .HasComment("Este campo indica si ya ha sido envio correo electronico con la notificacion del cambio");

            builder.Property(e => e.PrvdSizeDocument).HasColumnName("prvdSizeDocument");

            builder.Property(e => e.PrvdUrlDocument)
                .IsRequired()
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("prvdUrlDocument");

            builder.Property(e => e.PrvdUrlRelDocument)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("prvdUrlRelDocument");

            builder.Property(e => e.PrvdValidationDocument)
                .HasColumnName("prvdValidationDocument")
                .HasComment("Este campo indica si el documento ha sido validado por el gestor de documentos al ser cargado por el proveedor");

            //builder.HasOne(d => d.PrvdCodProveedorNavigation)
            //    .WithMany(p => p.PrvDocumentos)
            //    .HasForeignKey(d => d.PrvdCodProveedor)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("FK_PrvDocumentos_Proveedores");
        }
    }
}
