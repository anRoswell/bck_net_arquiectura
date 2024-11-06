using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class AprobadoresContratoConfiguration : IEntityTypeConfiguration<AprobadoresContrato>
    {
        public void Configure(EntityTypeBuilder<AprobadoresContrato> builder)
        {
            builder.HasKey(e => e.Id)
                    .HasName("PK__Aprobado__8CBD155A1E171E7F");

            builder.ToTable("AprobadoresContrato", "cont");

            builder.Property(e => e.Id).HasColumnName("apcIdAprobadoresContrato");

            builder.Property(e => e.ApcCodContrato)
                    .HasColumnName("apcCodContrato")
                    .HasComment("Codigo del contrato");

            builder.Property(e => e.ApcAprobacion).HasColumnName("apcAprobacion");

            builder.Property(e => e.ApcCodRequisitor).HasColumnName("apcCodRequisitor");

            builder.Property(e => e.ApcCodTipoAprobadoresContrato).HasColumnName("apcCodTipoAprobadoresContrato");
            
            builder.Property(e => e.ApcJustificacion)
                    .IsUnicode(false)
                    .HasColumnName("apcJustificacion")
                    .HasComment("Justificación de rechazo de la aprobación");

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