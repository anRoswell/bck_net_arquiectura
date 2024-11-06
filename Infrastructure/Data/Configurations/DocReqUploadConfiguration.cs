using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class DocReqUploadConfiguration : IEntityTypeConfiguration<DocReqUpload>
    {
        public void Configure(EntityTypeBuilder<DocReqUpload> builder)
        {
            builder.HasKey(e => e.Id);

            builder.ToTable("DocReqUpload", "cont");

            builder.Property(e => e.Id)
                .HasColumnName("drpuIdDocReqUpload")
                .HasComment("Identificación del registro");

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

            builder.Property(e => e.DrcoExtDocument)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("drcoExtDocument")
                .HasComment("Extensión del documento");

            builder.Property(e => e.DrcoNameDocument)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("drcoNameDocument")
                .HasComment("Nombre del documento");

            builder.Property(e => e.DrcoOriginalNameDocument)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("drcoOriginalNameDocument")
                .HasComment("Nombre original del documento");

            builder.Property(e => e.DrcoSizeDocument)
                .HasColumnName("drcoSizeDocument")
                .HasComment("Tamaño del documento");

            builder.Property(e => e.DrcoUrlDocument)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("drcoUrlDocument")
                .HasComment("Ruta del docuemento");

            builder.Property(e => e.DrcoUrlRelDocument)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("drcoUrlRelDocument")
                .HasComment("Ruta relativa del documento");

            builder.Property(e => e.DrpuUrl)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("drpuUrl")
                .HasComment("URLdel documento si es tipo URL");

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
