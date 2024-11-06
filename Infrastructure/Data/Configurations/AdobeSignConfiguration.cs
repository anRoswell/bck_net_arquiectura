using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class AdobeSignConfiguration : IEntityTypeConfiguration<PrvAdobeSign>
    {
        public void Configure(EntityTypeBuilder<PrvAdobeSign> builder)
        {
            builder.ToTable("PrvAdobeSign", "prv");

            builder.Property(e => e.Id).HasColumnName("id");

            builder.Property(e => e.AdsgnIdAdobeSign)
                .IsRequired()
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasColumnName("adsgnIdAdobeSign");

            builder.Property(e => e.AdsgnJson)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasColumnName("adsgnJson");

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
