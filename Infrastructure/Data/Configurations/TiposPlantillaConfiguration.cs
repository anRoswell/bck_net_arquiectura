using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data.Configurations
{
    public class TiposPlantillaConfiguration : IEntityTypeConfiguration<TiposPlantilla>
    {
        public void Configure(EntityTypeBuilder<TiposPlantilla> builder)
        { 
            builder.ToTable("TiposPlantilla", "noti");

            builder.HasKey(e => e.Id)
                .HasName("PK__TiposPla__997D34E86D1CC52B");

            builder.Property(e => e.Id)
                .HasColumnName("tpIdTiposPlantilla");

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

            builder.Property(e => e.TpnContenido)
                .IsRequired()
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasColumnName("tpnContenido");

            builder.Property(e => e.TpnDescripcion)
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("tpnDescripcion");

            builder.Property(e => e.TpnEstado).HasColumnName("tpnEstado");
        }

    }
}
