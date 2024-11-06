using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data.Configurations
{
    public class PasosContratoConfiguration : IEntityTypeConfiguration<PasosContrato>
    {
        public void Configure(EntityTypeBuilder<PasosContrato> builder)
        {
            builder.Ignore(e => e.Id);

            builder.HasKey(e => e.PcnIdPasosContrato)
                   .HasName("PK__PasosCon__51BA7D98AB734311");

            builder.ToTable("PasosContrato", "cont");

            builder.Property(e => e.PcnIdPasosContrato).HasColumnName("pcnIdPasosContrato");

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

            builder.Property(e => e.PcnCodContrato)
                .HasColumnName("pcnCodContrato")
                .HasComment("Codigo del contrato");

            builder.Property(e => e.PcnCodEstadoContrato)
                .HasColumnName("pcnCodEstadoContrato")
                .HasComment("Codigo del estado del contrato, en el momento");

            builder.Property(e => e.PcnCodTipoPaso)
                .HasColumnName("pcnCodTipoPaso")
                .HasComment("Codigo del tipo de paso");

            builder.Property(e => e.PcnConsecutivoFlujo)
                .HasColumnName("pcnConsecutivoFlujo")
                .HasComment("Consecutivo al que pertenece el paso en el flujo");
        }
    }
}
