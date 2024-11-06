using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data.Configurations
{
    public class DocumentoContratoConfiguration : IEntityTypeConfiguration<DocumentoContrato>
    {
        public void Configure(EntityTypeBuilder<DocumentoContrato> builder)
        {
            builder.HasKey(e => e.Id)
                    .HasName("PK__Document__572A36FCCAF587C9");

            builder.ToTable("DocumentoContrato", "cont");

            builder.Property(e => e.Id)
                .HasColumnName("dcIdDocumentoContrato")
                .HasComment("Identificación del registro");

            builder.Property(e => e.CodArchivo).HasComment("Id de archivo del Documento");

            builder.Property(e => e.CodContrato).HasComment("Codigo de la tabla contrato");

            builder.Property(e => e.CodDocumento).HasComment("Codigo del tipo de documento");

            builder.Property(e => e.CodTipoDocumento).HasComment("Tipo de documento URL o archivo");

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

            builder.Property(e => e.DcPermisos)
                .IsRequired()
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("dcPermisos")
                .HasComment("Permisos que a tener el proveedor sobre el documento");

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
        }
    }
}
