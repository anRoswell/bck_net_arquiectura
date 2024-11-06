using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class RepresentanteLegalEmpresaConfiguration : IEntityTypeConfiguration<RepresentantesLegalEmpresa>
    {
        public void Configure(EntityTypeBuilder<RepresentantesLegalEmpresa> builder)
        {
            builder.ToTable("RepresentantesLegalEmpresas", "par");

            builder.Property(e => e.Id).HasColumnName("rleIdRepresentantesLegalEmpresas");
            builder.HasKey(e => e.Id);

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
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            builder.Property(e => e.FechaRegistroUpdate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
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
            builder.Property(e => e.RleCodEmpresa).HasColumnName("rleCodEmpresa");
            builder.Property(e => e.RleEmailRteLegal)
                .IsRequired()
                .HasMaxLength(256)
                .HasColumnName("rleEmailRteLegal");
            builder.Property(e => e.RleEstado)
                .IsRequired()
                .HasDefaultValueSql("((1))")
                .HasColumnName("rleEstado");
            builder.Property(e => e.RleIdentificacionRteLegal)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("rleIdentificacionRteLegal");
            builder.Property(e => e.RleNombreRteLegal)
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("rleNombreRteLegal");
        }
    }
}