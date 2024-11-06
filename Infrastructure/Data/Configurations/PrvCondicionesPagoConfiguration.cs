using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data.Configurations
{
    public class PrvCondicionesPagoConfiguration : IEntityTypeConfiguration<PrvCondicionesPago>
    {
        public void Configure(EntityTypeBuilder<PrvCondicionesPago> builder)
        {
            builder.HasKey(e => e.Id)
                    .HasName("PK__PrvCondi__E958C1CF89611E6C");

            builder.ToTable("PrvCondicionesPago", "prv");

            builder.Property(e => e.Id).HasColumnName("condPagoIdPrvCondicionesPago");

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

            builder.Property(e => e.CondDescripcion)
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("condDescripcion");

            builder.Property(e => e.CondPagoEstado)
                .IsRequired()
                .HasColumnName("condPagoEstado")
                .HasDefaultValueSql("((1))");

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
        }
    }
}
