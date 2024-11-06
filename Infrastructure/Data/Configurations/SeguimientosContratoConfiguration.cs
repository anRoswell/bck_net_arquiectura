using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data.Configurations
{
    class SeguimientosContratoConfiguration : IEntityTypeConfiguration<SeguimientosContrato>
    {
        public void Configure(EntityTypeBuilder<SeguimientosContrato> builder)
        {
            builder.HasKey(e => e.ScoIdSeguimiento)
                    .HasName("PK__Seguimie__F29EFEAF9412A0EF");

            builder.ToTable("SeguimientosContrato", "cont");

            builder.Property(e => e.ScoIdSeguimiento)
                .HasColumnName("scoIdSeguimiento")
                .HasComment("Id del seguimiento");

            builder.Property(e => e.CodArchivo)
                .HasColumnName("codArchivo")
                .HasComment("codigo del archivo adjunto");

            builder.Property(e => e.ScoCodContrato)
                .HasColumnName("scoCodContrato")
                .HasComment("Codigo del contrato");

            builder.Property(e => e.ScoFecha)
                .HasColumnType("datetime")
                .HasColumnName("scoFecha")
                .HasComment("Fecha del seguimiento");

            builder.Property(e => e.ScoObservacion)
                .HasMaxLength(500)
                .HasColumnName("scoObservacion")
                .HasComment("Observacion del seguimiento");

            builder.Property(e => e.ScoPagosEfectuados)
                .HasColumnType("numeric(18, 2)")
                .HasColumnName("scoPagosEfectuados")
                .HasComment("Valor del pago efectuado");

            builder.Property(e => e.ScoTipo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("scoTipo");

            builder.Property(e => e.CodUser)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

            builder.Property(e => e.CodUserUpdate)
                .IsRequired()
                .HasMaxLength(30)
                .IsUnicode(false);

            builder.Property(e => e.FechaRegistro).HasColumnType("datetime");

            builder.Property(e => e.FechaRegistroUpdate).HasColumnType("datetime");

            builder.Property(e => e.Info)
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode(false);

            builder.Property(e => e.InfoUpdate)
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode(false);
        }
    }
}
