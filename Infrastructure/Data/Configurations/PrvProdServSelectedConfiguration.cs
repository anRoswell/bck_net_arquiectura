using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data.Configurations
{
    public class PrvProdServSelectedConfiguration : IEntityTypeConfiguration<PrvProdServSelected>
    {
        

        public void Configure(EntityTypeBuilder<PrvProdServSelected> builder)
        {
            builder.HasKey(e => e.Id)
                    .HasName("PK__PrvProdS__DB206E765D55648A");

            builder.ToTable("PrvProdServSelected", "prv");

            builder.Property(e => e.Id).HasColumnName("proSer_IdPrvProdServSelected");

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
