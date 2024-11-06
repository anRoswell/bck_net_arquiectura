using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class ReqArtSerParticipanteConfiguration : IEntityTypeConfiguration<ReqArtSerParticipante>
    {
        public void Configure(EntityTypeBuilder<ReqArtSerParticipante> builder)
        {
            builder.HasKey(e => e.Id)
                    .HasName("PK__ReqArtSe__B1D03EBEE8E7169C");

            builder.ToTable("ReqArtSerParticipante", "req");

            builder.Property(e => e.Id).HasColumnName("IdReqArtSerParticipante");

            builder.Property(e => e.CodUser)
                .IsRequired()
                .HasMaxLength(30)
                .IsUnicode(false);

            builder.Property(e => e.CodUserUpdate)
                .IsRequired()
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasDefaultValueSql("('777777')");

            builder.Property(e => e.Descuento).HasColumnType("numeric(3, 2)");

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
                .HasDefaultValueSql("('0|0|0|')");

            builder.Property(e => e.InfoUpdate)
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasDefaultValueSql("('0|0|0|')");

            builder.Property(e => e.Iva).HasColumnType("numeric(2, 2)");

            builder.Property(e => e.Observacion)
                .HasMaxLength(200)
                .IsUnicode(false);

            builder.Property(e => e.CodOrdenApot)
                .HasColumnName("CodOrdenApot")
                .HasComment("Codigo de orden asociada al articulo, en la ERP");

            builder.Property(e => e.Valor).HasColumnType("numeric(18, 2)");
        }
    }
}
