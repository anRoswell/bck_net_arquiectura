using Core.Entities;
using ImageMagick;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data.Configurations
{
    public class AprobadoresContratoHistoricoConfiguration : IEntityTypeConfiguration<AprobadoresContratoHistorico>
    {
        public void Configure(EntityTypeBuilder<AprobadoresContratoHistorico> builder)
        {
            builder.HasKey(e => e.ApcIdAprobadoresContratoHistorico)
                    .HasName("PK__Aprobado__BD531503759BD6FA");

            builder.ToTable("AprobadoresContratoHistorico", "cont");

            builder.Property(e => e.ApcIdAprobadoresContratoHistorico).HasColumnName("apcIdAprobadoresContratoHistorico");

            builder.Property(e => e.ApcAprobacion).HasColumnName("apcAprobacion");

            builder.Property(e => e.ApcCodContratoHistorico)
                .HasColumnName("apcCodContratoHistorico")
                .HasComment("Codigo del contrato historico");

            builder.Property(e => e.ApcCodRequisitor).HasColumnName("apcCodRequisitor");

            builder.Property(e => e.ApcCodTipoAprobadoresContrato).HasColumnName("apcCodTipoAprobadoresContrato");

            builder.Property(e => e.ApcJustificacion)
                .IsUnicode(false)
                .HasColumnName("apcJustificacion")
                .HasComment("Justificación de rechazo de la aprobación");

            builder.Property(e => e.CodUser)
                .IsRequired()
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasDefaultValueSql("('7777777')");

            builder.Property(e => e.CodUserUpdate)
                .IsRequired()
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasDefaultValueSql("('7777777')");

            builder.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            builder.Property(e => e.FechaRegistroUpdate)
                  .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            builder.Property(e => e.Info)
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasDefaultValueSql("('0|0|0')");

            builder.Property(e => e.InfoUpdate)
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasDefaultValueSql("('0|0|0')");

            builder.HasOne(d => d.ApcContratoHistorico)
                .WithMany(p => p.AprobadoresContratoHistoricos)
                .HasForeignKey(d => d.ApcCodContratoHistorico)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AprobadoresContratoHistorico_ContratoHistorico");
        }        
    }
}
