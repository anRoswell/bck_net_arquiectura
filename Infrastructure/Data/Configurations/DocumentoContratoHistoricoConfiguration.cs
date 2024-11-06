using Core.Entities;
using ImageMagick;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data.Configurations
{
    public class DocumentoContratoHistoricoConfiguration : IEntityTypeConfiguration<DocumentoContratoHistorico>
    {
        public void Configure(EntityTypeBuilder<DocumentoContratoHistorico> entity)
        {
            entity.HasKey(e => e.DcIdDocumentoContratoHistorico)
                   .HasName("PK__Document__3F141E1F013E7592");

            entity.ToTable("DocumentoContratoHistorico", "cont");

            entity.Property(e => e.DcIdDocumentoContratoHistorico)
                .HasColumnName("dcIdDocumentoContratoHistorico")
                .HasComment("Identificación del registro");

            entity.Property(e => e.CodArchivo).HasComment("Id de archivo del Documento");

            entity.Property(e => e.CodContratoHistorico).HasComment("Codigo de la tabla contrato");

            entity.Property(e => e.CodDocumento).HasComment("Codigo del tipo de documento");

            entity.Property(e => e.CodTipoDocumento).HasComment("Tipo de documento URL o archivo");

            entity.Property(e => e.CodUser)
                .IsRequired()
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasDefaultValueSql("('7777777')");

            entity.Property(e => e.CodUserUpdate)
                .IsRequired()
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasDefaultValueSql("('7777777')");

            entity.Property(e => e.DcPermisos)
                .IsRequired()
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("dcPermisos")
                .HasComment("Permisos que a tener el proveedor sobre el documento");

            entity.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.FechaRegistroUpdate)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.Info)
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasDefaultValueSql("('0|0|0')");

            entity.Property(e => e.InfoUpdate)
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasDefaultValueSql("('0|0|0')");

            entity.HasOne(d => d.Archivo)
                  .WithMany(p => p.DocumentoContratoHistoricos)
                  .HasForeignKey(d => d.CodArchivo)
                  .HasConstraintName("FK_DocumentoContratoHistorico_DocReqUpload");

            entity.HasOne(d => d.DcContratoHistorico)
                .WithMany(p => p.DocumentoContratoHistoricos)
                .HasForeignKey(d => d.CodContratoHistorico)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DocumentoContratoHistorico_ContratoHistorico");
        }
    }
}
