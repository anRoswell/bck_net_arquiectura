using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class RespuestasCertEspecialeConfiguration : IEntityTypeConfiguration<RespuestasCertEspeciale>
    {
        public void Configure(EntityTypeBuilder<RespuestasCertEspeciale> builder)
        {
            builder.HasKey(e => e.Id)
                    .HasName("PK__Respuest__6ECA5B45A9C16233");

            builder.ToTable("RespuestasCertEspeciales", "cer");

            builder.Property(e => e.Id).HasColumnName("rcesIdRespuestasCertEspeciales");

            builder.Property(e => e.CodUser)
                .IsRequired()
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasDefaultValueSql("('7777777')")
                .HasComment("Cedula del usuario que crea el registro");

            builder.Property(e => e.CodUserUpdate)
                .IsRequired()
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasDefaultValueSql("('7777777')")
                .HasComment("Cedula del ultimo usuario que actualizó el registro");

            builder.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())")
                .HasComment("Fecha de creación del registro.");

            builder.Property(e => e.FechaRegistroUpdate)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())")
                .HasComment("Fecha de la ultima actualización del registro.");

            builder.Property(e => e.Info)
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasDefaultValueSql("('0|0|0')")
                .HasComment("En este campo almacenamos la direccion ip, navegador y version del navegador del cliente.");

            builder.Property(e => e.InfoUpdate)
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasDefaultValueSql("('0|0|0')")
                .HasComment("En este campo almacenamos la ultima direccion ip, navegador y version del navegador del cliente.");

            builder.Property(e => e.RcesCodCertificadosEspeciales).HasColumnName("rcesCodCertificadosEspeciales");

            builder.Property(e => e.RcesEstado)
                .IsRequired()
                .HasColumnName("rcesEstado")
                .HasDefaultValueSql("((1))");

            builder.Property(e => e.RcesObservacion)
                .IsRequired()
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("rcesObservacion");

            //builder.HasOne(d => d.RcesCodCertificadosEspecialesNavigation)
            //    .WithMany(p => p.RespuestasCertEspeciales)
            //    .HasForeignKey(d => d.RcesCodCertificadosEspeciales)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("FK_RespuestasCertEspeciales_CertificadosEspeciales");
        }
    }
}
