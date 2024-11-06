using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data.Configurations
{
    public class TipoPasosContratoConfiguration : IEntityTypeConfiguration<TipoPasosContrato>
    {
        public void Configure(EntityTypeBuilder<TipoPasosContrato> builder)
        {
            builder.HasKey(e => e.TpcIdTipoPasosContrato)
                    .HasName("PK__TipoPaso__7E0C52073A18F049");

            builder.ToTable("TipoPasosContrato", "cont");

            builder.Property(e => e.TpcIdTipoPasosContrato).HasColumnName("tpcIdTipoPasosContrato");

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

            builder.Property(e => e.PcnEstado)
                .IsRequired()
                .HasColumnName("pcnEstado")
                .HasDefaultValueSql("((1))")
                .HasComment("Estado del registro");

            builder.Property(e => e.TpcCodFlujoContrato)
                .HasColumnName("tpcCodFlujoContrato")
                .HasComment("Codigo del flujo de contrato");

            builder.Property(e => e.TpcDescripcion)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("tpcDescripcion")
                .HasComment("Descripcion del tipo de paso de contrato");
        }
    }
}
