using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class NoticiasConfiguration : IEntityTypeConfiguration<Noticia>
    {
        public void Configure(EntityTypeBuilder<Noticia> builder)
        {
            builder.ToTable("noticias", "noti");            

          //  builder.HasKey(e => e.NotIdNoticias)
            //(    .HasName("PK__noticias__65FA1A9B6A627F79");

            //builder.Property(e => e.NotIdNoticias)
                //.HasColumnName("notIdNoticias");

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

            builder.Property(e => e.NotDescripcion)
                .IsRequired()
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasColumnName("notDescripcion");

            builder.Property(e => e.NotEstado)
                .HasColumnName("notEstado")
                .HasDefaultValueSql("((1))");

            builder.Property(e => e.NotTitle)
                .IsRequired()
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("notTitle");

        }
    }
}
