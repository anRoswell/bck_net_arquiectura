using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data.Configurations
{
    public class TipoAprobadoresContratoConfiguration : IEntityTypeConfiguration<TipoAprobadoresContrato>
    {
        public void Configure(EntityTypeBuilder<TipoAprobadoresContrato> builder)
        {
            builder.HasKey(e => e.Id)
                    .HasName("PK__TipoApro__BCEE4196C0B22B2C");

            builder.ToTable("TipoAprobadoresContrato", "cont");

            builder.Property(e => e.Id).HasColumnName("tacIdTipoAprobadoresContrato");

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

            builder.Property(e => e.TacDescripcion)
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("tacDescripcion");

            builder.Property(e => e.TacEstado)
                .IsRequired()
                .HasColumnName("tacEstado")
                .HasDefaultValueSql("((1))");
        }
    }
}
