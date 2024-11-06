using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data.Configurations
{
    public class AlcanceConfiguration : IEntityTypeConfiguration<Alcance>
    {
        public void Configure(EntityTypeBuilder<Alcance> builder)
        {
            builder.HasKey(e => e.Id)
                    .HasName("PK__Alcance__C04AE5A789C7A580");

            builder.ToTable("Alcance", "noti");

            builder.Property(e => e.Id).HasColumnName("Id");

            builder.Property(e => e.AlDescripcion)
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("alDescripcion");

            builder.Property(e => e.AlEstado)
                .IsRequired()
                .HasColumnName("alEstado")
                .HasDefaultValueSql("((1))");

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
        }        
    }
}
