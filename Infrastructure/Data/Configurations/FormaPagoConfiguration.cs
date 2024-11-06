using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data.Configurations
{
    class FormaPagoConfiguration : IEntityTypeConfiguration<FormasPago>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<FormasPago> builder)
        {
            builder.HasKey(e => e.Id)
                    .HasName("PK__FormasPa__6AAFE1C4F5E7F814");

            builder.ToTable("FormasPago", "par");

            //builder.Property(e => e.Id).HasColumnName("fpagIdFormasPago");

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

            builder.Property(e => e.FpagDescripcion)
                .IsRequired()
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("fpagDescripcion")
                .HasComment("Descripción de la forma de pago");

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

            builder.Property(e => e.fpagEstado)
                .IsRequired()
                .HasColumnName("fpagEstado")
                .HasDefaultValueSql("((1))")
                .HasComment("Estado en q se encuentra el estado, activo - inactivo");
        }
    }
}
