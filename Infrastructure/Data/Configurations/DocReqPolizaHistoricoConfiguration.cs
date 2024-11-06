using Core.Entities;
using ImageMagick;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data.Configurations
{
    public class DocReqPolizaHistoricoConfiguration : IEntityTypeConfiguration<DocReqPolizaHistorico>
    {
        public void Configure(EntityTypeBuilder<DocReqPolizaHistorico> entity)
        {
            entity.HasKey(e => e.DrpoIdDocReqPolizaHistorico)
                    .HasName("PK__DocReqPo__CEDF02FE4DB31243");

            entity.ToTable("DocReqPolizaHistorico", "cont");

            entity.Property(e => e.DrpoIdDocReqPolizaHistorico)
                .HasColumnName("drpoIdDocReqPolizaHistorico")
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

            entity.Property(e => e.DrpoAprobada).HasColumnName("drpoAprobada");

            entity.Property(e => e.DrpoCobertura)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("drpoCobertura")
                    .HasDefaultValueSql("((1))")
                    .HasComment("Si el documento es requerido o no");

            entity.Property(e => e.DrpoCodContratoHistorico)
                .HasColumnName("drpoCodContratoHistorico")
                .HasComment("Codigo de la tabla contrato");

            entity.Property(e => e.DrpoCodTipoDocumento)
                .HasColumnName("drpoCodTipoDocumento")
                .HasComment("Tipo de archivo de la poliza");

            entity.Property(e => e.DrpoEsRenovada)
                .HasColumnName("drpoEsRenovada")
                .HasComment("Si la poliza es renovada");

            entity.Property(e => e.DrpoEstado)
                .HasColumnName("drpoEstado")
                .HasDefaultValueSql("((1))")
                .HasComment("Estado del registro");

            entity.Property(e => e.DrpoExpedida).HasColumnName("drpoExpedida");

            entity.Property(e => e.DrpoTipoPoliza)
                .IsRequired()
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("drpoTipoPoliza")
                .HasComment("Titulo del documento");

            entity.Property(e => e.DrpoVigencia)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("drpoVigencia");

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

            entity.HasOne(d => d.DrpoContratoHistorico)
                .WithMany(p => p.DocReqPolizaHistoricos)
                .HasForeignKey(d => d.DrpoCodContratoHistorico)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DocReqPolizaHistorico_ContratoHistorico");

            entity.HasOne(d => d.Archivo)
                    .WithMany(p => p.DocReqPolizaHistoricos)
                    .HasForeignKey(d => d.CodArchivo)
                    .HasConstraintName("FK_DocReqPolizaHistorico_DocReqUpload");
        }
    }
}
