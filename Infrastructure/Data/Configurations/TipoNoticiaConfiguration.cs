using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data.Configurations
{
    public class TipoNoticiaConfiguration : IEntityTypeConfiguration<TipoNoticia>
    {
        public void Configure(EntityTypeBuilder<TipoNoticia> builder)
        {
            builder.HasKey(e => e.TnIdTipoNoticias)
                    .HasName("PK__TipoNoti__EBA6656665A06718");

            builder.ToTable("TipoNoticias", "noti");

            builder.Property(e => e.TnIdTipoNoticias).HasColumnName("tnIdTipoNoticias");

            builder.Property(e => e.CodUser)
                .IsRequired()
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasDefaultValueSql("('DEFAULT (''7777777'')')");

            builder.Property(e => e.CodUserUpdate)
                .IsRequired()
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasDefaultValueSql("('DEFAULT (''7777777'')')");

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
                .HasDefaultValueSql("('DEFAULT (''0|0|0'')')");

            builder.Property(e => e.InfoUpdate)
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasDefaultValueSql("('DEFAULT (''0|0|0'')')");

            builder.Property(e => e.TnDescripcion)
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("tnDescripcion");

            builder.Property(e => e.TnEstado).HasColumnName("tnEstado");
        }
    }
}
