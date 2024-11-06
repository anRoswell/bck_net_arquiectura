using Core.Entities;
using ImageMagick;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data.Configurations
{
    public class DocReqProveedorHistoricoConfiguration : IEntityTypeConfiguration<DocReqProveedorHistorico>
    {
        public void Configure(EntityTypeBuilder<DocReqProveedorHistorico> entity)
        {
            entity.HasKey(e => e.DrpIdDocRequeridosProveedorHistorico)
                    .HasName("PK__DocReqPr__FDC18A216C597677");

            entity.ToTable("DocReqProveedorHistorico", "cont");

            entity.Property(e => e.DrpIdDocRequeridosProveedorHistorico)
                .HasColumnName("drpIdDocRequeridosProveedorHistorico")
                .HasComment("Identificación del registro");

            entity.Property(e => e.CodArchivo).HasComment("Id de archivo del Documento");

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

            entity.Property(e => e.DrpAprobado)
                .HasColumnName("drpAprobado")
                .HasDefaultValueSql("((0))")
                .HasComment("Si el documento es aprobado o no.");

            entity.Property(e => e.DrpCodContratoHistorico)
                .HasColumnName("drpCodContratoHistorico")
                .HasComment("Codigo de la tabla contrato");

            entity.Property(e => e.DrpCodDocumento)
                .HasColumnName("drpCodDocumento")
                .HasComment("Codigo de la tabla documentos");

            entity.Property(e => e.DrpCodPrvDocumento)
                .HasColumnName("drpCodPrvDocumento")
                .HasComment("Codigo de la documentos de proveedores");

            entity.Property(e => e.DrpObligatorio)
                .HasColumnName("drpObligatorio")
                .HasComment("Documento Obligatorio");

            entity.Property(e => e.DrpTipoVersion)
                .HasColumnName("drpTipoVersion")
                .HasComment("Versión del documento");

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
                    .WithMany(p => p.DocReqProveedorHistoricos)
                    .HasForeignKey(d => d.CodArchivo)
                    .HasConstraintName("FK_DocReqProveedorHistorico_DocReqUpload");

            entity.HasOne(d => d.DrpContratoHistorico)
                .WithMany(p => p.DocReqProveedorHistoricos)
                .HasForeignKey(d => d.DrpCodContratoHistorico)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DocReqProveedorHistorico_ContratoHistorico");
        }
    }
}
