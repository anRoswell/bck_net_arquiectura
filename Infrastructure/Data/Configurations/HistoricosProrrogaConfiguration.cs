using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data.Configurations
{
    class HistoricosProrrogaConfiguration : IEntityTypeConfiguration<HistoricosProrroga>
    {
        public void Configure(EntityTypeBuilder<HistoricosProrroga> builder)
        {
            builder.ToTable("HistoricosProrroga", "cont");

            builder.Property(e => e.Id).ValueGeneratedOnAdd();

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

            builder.Property(e => e.VigenciaAnterior).HasColumnType("datetime");

            builder.Property(e => e.VigenciaHasta).HasColumnType("datetime"); 
        }
    }
}
