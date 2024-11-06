using Core.Entities;
using ImageMagick;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data.Configurations
{
    public class DocReqProveedorOtrosHistoricoConfiguration : IEntityTypeConfiguration<DocReqProveedorOtrosHistorico>
    {
        public void Configure(EntityTypeBuilder<DocReqProveedorOtrosHistorico> entity)
        {
            entity.HasKey(e => e.DrpoIdDocReqProveedorOtrosHistorico)
                    .HasName("PK__DocReqPr__51CB612F4A92C613");

            entity.ToTable("DocReqProveedorOtrosHistorico", "cont");

            entity.Property(e => e.DrpoIdDocReqProveedorOtrosHistorico)
                .HasColumnName("drpoIdDocReqProveedorOtrosHistorico")
                .HasComment("Identificación del registro");

            entity.Property(e => e.CodArchivo).HasComment("Id de archivo del Documento");

            entity.Property(e => e.CodUser)
                .IsRequired()
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasDefaultValueSql("('7777777')")
                .HasComment("Cedula del usuario que crea el registro");

            entity.Property(e => e.CodUserUpdate)
                .IsRequired()
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasDefaultValueSql("('7777777')")
                .HasComment("Cedula del ultimo usuario que actualizó el registro");

            entity.Property(e => e.DrpoCodContratoHistorico)
                .HasColumnName("drpoCodContratoHistorico")
                .HasComment("Codigo del contrato");

            entity.Property(e => e.DrpoCodDocumento)
                .HasColumnName("drpoCodDocumento")
                .HasComment("Codigo del documento");

            entity.Property(e => e.DrpoNombreDocumento)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("drpoNombreDocumento")
                .HasComment("Nombre del documento 'Otros'");

            entity.Property(e => e.DrpoObligatorio)
                .IsRequired()
                .HasColumnName("drpoObligatorio")
                .HasDefaultValueSql("((1))")
                .HasComment("Estado del registro");

            entity.Property(e => e.DrpoVigencia)
                .HasColumnName("drpoVigencia")
                .HasComment("Id del documento registrado para cargue en el requerimiento");

            entity.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())")
                .HasComment("Fecha de creación del registro.");

            entity.Property(e => e.FechaRegistroUpdate)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())")
                .HasComment("Fecha de la ultima actualización del registro.");

            entity.Property(e => e.Info)
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasDefaultValueSql("('0|0|0')")
                .HasComment("En este campo almacenamos la direccion ip, navegador y version del navegador del cliente.");

            entity.Property(e => e.InfoUpdate)
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasDefaultValueSql("('0|0|0')")
                .HasComment("En este campo almacenamos la ultima direccion ip, navegador y version del navegador del cliente.");

            entity.HasOne(d => d.Archivo)
                    .WithMany(p => p.DocReqProveedorOtrosHistoricos)
                    .HasForeignKey(d => d.CodArchivo)
                    .HasConstraintName("FK_DocReqProveedorOtrosHistorico_DocReqUpload");

            entity.HasOne(d => d.DrpoContratoHistorico)
                .WithMany(p => p.DocReqProveedorOtrosHistoricos)
                .HasForeignKey(d => d.DrpoCodContratoHistorico)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DocReqProveedorOtrosHistorico_ContratoHistorico");
        }
    }
}
